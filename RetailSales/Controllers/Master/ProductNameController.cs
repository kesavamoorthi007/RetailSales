using System.Data;
using DocumentFormat.OpenXml.VariantTypes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Reporting.Map.WebForms.BingMaps;
using RetailSales.Interface.Master;
using RetailSales.Models;
using RetailSales.Models.Master;
using RetailSales.Services.Master;
using RetailSales.Services.Purchase;

namespace RetailSales.Controllers.Master
{
    public class ProductNameController : Controller
    {
        IProductNameService ProductNameService;
        IConfiguration? _configuratio;
        private string? _connectionString;
        DataTransactions datatrans;

        public ProductNameController(IProductNameService _ProductNameService, IConfiguration _configuratio)
        {
            ProductNameService = _ProductNameService;
            _connectionString = _configuratio.GetConnectionString("MySqlConnection");
            datatrans = new DataTransactions(_connectionString);
        }
        public IActionResult ProductName(string id)
        {
            ProductName ic = new ProductName();
            ic.Categorylst = BindCategory();

            List<ProductNameItem> TData = new List<ProductNameItem>();
            ProductNameItem tda = new ProductNameItem();

            if (id == null)
            {
                for (int i = 0; i < 1; i++)
                {
                    tda = new ProductNameItem();
                    tda.UOMlst = BindUOM();
                    tda.HSNlst = BindHsn();
                    tda.ShopBinlist = BindShopBin();
                    tda.GodownBinlist = BindGodownBin();
                    tda.locationlist = BindLocation();
                    tda.Rate = "0";
                    tda.Isvalid = "Y";
                    TData.Add(tda);
                }
            }
            else
            {

                DataTable dt = new DataTable();
                dt = ProductNameService.GetEditProductName(id);
                if (dt.Rows.Count > 0)
                {
                    ic.Categorylst = BindCategory();
                    ic.Category = dt.Rows[0]["PRODUCT_CATEGORY"].ToString();
                    ic.ProName = dt.Rows[0]["PROD_NAME"].ToString();
                    ic.Description = dt.Rows[0]["DESCRIPTION"].ToString();
                    ic.ID = id;
                }
                DataTable dtt = new DataTable();
                dtt = ProductNameService.GetEditProductNameItem(id);
                if (dtt.Rows.Count > 0)
                {
                    for (int i = 0; i < dtt.Rows.Count; i++)
                    {
                        tda = new ProductNameItem();
                        tda.ShopBinlist = BindShopBin();
                        tda.GodownBinlist = BindGodownBin();
                        tda.locationlist = BindLocation();
                        tda.ShopBin= dtt.Rows[i]["SHOP_BIN"].ToString();
                        tda.GodownBin= dtt.Rows[i]["GODOWN_BIN"].ToString();
                        tda.Variant = dtt.Rows[i]["PRODUCT_VARIANT"].ToString();
                        tda.Location= dtt.Rows[i]["LOCATION"].ToString();
                        tda.UOMlst = BindUOM();
                        tda.Uom = dtt.Rows[i]["UOM"].ToString();
                        tda.HSNlst = BindHsn();
                        tda.Hsn = dtt.Rows[i]["HSN_CODE"].ToString();
                        tda.MinQty = dtt.Rows[i]["MIN_QTY"].ToString();
                        tda.Rate = dtt.Rows[i]["RATE"].ToString();
                        tda.ProdDesc = dtt.Rows[i]["PRODUCT_DESCRIPTION"].ToString();
                        tda.ID = dtt.Rows[i]["ID"].ToString();
                        tda.Isvalid = "Y";
                        TData.Add(tda);
                    }
                }
            }
            ic.ProductNameLst = TData;
            return View(ic);
        }
        [HttpPost]

        public ActionResult ProductName(ProductName cy, string id)
        {
            ViewBag.PageTitle = "ProductName";
            try
            {
                cy.ID = id;
                string Strout = ProductNameService.ProductNameCRUD(cy);
                if (string.IsNullOrEmpty(Strout))
                {
                    if (cy.ID == null)
                    {
                        TempData["notice"] = "Product Inserted Successfully...!";
                    }
                    else
                    {
                        TempData["notice"] = "Product Updated Successfully...!";
                    }
                    return RedirectToAction("ListProductName");
                }

                else
                {
                    ViewBag.PageTitle = "Edit Product";
                    TempData["notice"] = Strout;
                    return RedirectToAction("ProductName");
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return View(cy);
        }

        public List<SelectListItem> BindShopBin()
        {
            try
            {
                DataTable dtDesg = datatrans.GetData("SELECT BINMASTER.ID,BINMASTER.BINID,BINMASTER.IS_ACTIVE FROM BINMASTER WHERE LOCID='1' AND BINMASTER.IS_ACTIVE = 'Y'");
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["BINID"].ToString(), Value = dtDesg.Rows[i]["ID"].ToString() });
                }
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<SelectListItem> BindLocation()
        {
            try
            {
                DataTable dtDesg = datatrans.GetData("SELECT ID,LOCATION_NAME FROM LOCATION WHERE IS_ACTIVE='Y' UNION ALL SELECT CAST(0 AS INT), CAST('BOTH' AS VARCHAR(100)) order by 2") ;
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["LOCATION_NAME"].ToString(), Value = dtDesg.Rows[i]["LOCATION_NAME"].ToString() });
                }
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<SelectListItem> BindGodownBin()
        {
            try
            {
                DataTable dtDesg = datatrans.GetData("SELECT BINMASTER.ID,BINMASTER.BINID,BINMASTER.IS_ACTIVE FROM BINMASTER WHERE LOCID='2' AND BINMASTER.IS_ACTIVE = 'Y'");
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["BINID"].ToString(), Value = dtDesg.Rows[i]["ID"].ToString() });
                }
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public JsonResult deletevariant(string ItemId)
        {
            var result ="";
            string cnt = datatrans.GetDataString("SELECT count(*) as cunt FROM PODETAIL WHERE VARIANT = '"+ ItemId + "'");
            if (Convert.ToInt32(cnt) > 0)
            {
                result = "The variant already linked with the Transaction.So do not delete.";
            }
            else
            {
                result = ProductNameService.VariantDelete(ItemId);
            }
             return Json(result);
        }

        public IActionResult ListProductName()
        {
            return View();
        }

        public IActionResult ViewProductName(string id)
        {
            ProductName ic = new ProductName();
            DataTable dt = new DataTable();
            DataTable dtt = new DataTable();

            dt = ProductNameService.GetProductName(id);
            if (dt.Rows.Count > 0)
            {
                ic.Category = dt.Rows[0]["PRODUCT_NAME"].ToString();
                ic.ProName = dt.Rows[0]["PROD_NAME"].ToString();
                ic.Description = dt.Rows[0]["DESCRIPTION"].ToString();
                ic.ID = id;


            }

            List<ProductNameItem> TData = new List<ProductNameItem>();
            ProductNameItem tda = new ProductNameItem();



            dtt = ProductNameService.GetProductNameItem(id);
            if (dtt.Rows.Count > 0)
            {
                for (int i = 0; i < dtt.Rows.Count; i++)
                {
                    tda = new ProductNameItem();
                    tda.Variant = dtt.Rows[i]["PRODUCT_VARIANT"].ToString();
                    tda.Uom = dtt.Rows[i]["UOM_CODE"].ToString();
                    tda.Hsn = dtt.Rows[i]["HSCODE"].ToString();
                    tda.MinQty = dtt.Rows[i]["MIN_QTY"].ToString();
                    tda.Rate = dtt.Rows[i]["RATE"].ToString();
                    tda.ProdDesc = dtt.Rows[i]["PRODUCT_DESCRIPTION"].ToString();
                    tda.ID = dtt.Rows[i]["ID"].ToString();
                    tda.Location= dtt.Rows[i]["LOCATION"].ToString();
                    tda.GodownBin= dtt.Rows[i]["GODOWN_BIN"].ToString();
                    tda.ShopBin = dtt.Rows[i]["SHOP_BIN"].ToString();
                    tda.Isvalid = "Y";
                    TData.Add(tda);

                }
            }
            ic.ProductNameLst = TData;
            return View(ic);
        }

        public JsonResult GetHSNGrpJSON()
        {
            ProductNameItem model = new ProductNameItem();
            model.HSNlst = BindHsn();
            return Json(BindHsn());
        }
        public JsonResult GetshopbinJSON()
        {
            ProductNameItem model = new ProductNameItem();
            model.ShopBinlist= BindShopBin();
            return Json(BindShopBin());
        }
        public JsonResult GetgodownbinJSON()
        {
            ProductNameItem model = new ProductNameItem();
            model.GodownBinlist = BindGodownBin();
            return Json(BindGodownBin());
        }
        public JsonResult GetlocationJSON()
        {
            ProductNameItem model = new ProductNameItem();
            model.locationlist = BindLocation();
            return Json(BindLocation());
        }
        public JsonResult GetUOMGrpJSON()
        {
            ProductNameItem model = new ProductNameItem();
            model.UOMlst = BindUOM();
            return Json(BindUOM());
        }

        public List<SelectListItem> BindUOM()
        {
            try
            {
                DataTable dtDesg = ProductNameService.GetUom();
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["UOM_CODE"].ToString(), Value = dtDesg.Rows[i]["ID"].ToString() });
                }
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<SelectListItem> BindSUOM(string id)
        {
            try
            {
                DataTable dtDesg = datatrans.GetData("");
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["UOM_CODE"].ToString(), Value = dtDesg.Rows[i]["ID"].ToString() });
                }
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public IActionResult AddCF(string id)
        {
            Productdetail ic = new Productdetail();


            DataTable dt = new DataTable();
            DataTable dtt = new DataTable();

            dt = datatrans.GetData(" SELECT PRO_DETAIL.ID,PRODUCT.PRODUCT_NAME,PRO_NAME.PROD_NAME,PRO_DETAIL.PRODUCT_VARIANT,PRO_DETAIL.IS_ACTIVE FROM PRO_DETAIL LEFT OUTER JOIN PRODUCT ON PRODUCT.ID=PRO_DETAIL.PRODUCT_CATEGORY LEFT OUTER JOIN PRO_NAME ON PRO_NAME.PRO_NAME_BASICID=PRO_DETAIL.PRODUCT_ID WHERE PRO_DETAIL.ID='" + id + "'");
            if (dt.Rows.Count > 0)
            {
                ic.Product = dt.Rows[0]["PRODUCT_NAME"].ToString();
                ic.ProName = dt.Rows[0]["PROD_NAME"].ToString();
                ic.Varint = dt.Rows[0]["PRODUCT_VARIANT"].ToString();

            }

            List<ProductDetailTable> TData = new List<ProductDetailTable>();
            ProductDetailTable tda = new ProductDetailTable();

          
          
                dtt = datatrans.GetData("SELECT UOM_CONVERT.PRO_ID,UOM.UOM_CODE,DEST_UOM,CF FROM UOM_CONVERT LEFT OUTER JOIN UOM ON UOM.ID=UOM_CONVERT.SRC_UOM LEFT OUTER JOIN PRO_DETAIL ON PRO_DETAIL.ID=UOM_CONVERT.PRO_ID WHERE UOM_CONVERT.PRO_ID = '" + id + "' ");
                if (dtt.Rows.Count > 0) 
                { 
                    for (int i = 0; i < dtt.Rows.Count; i++)
                    {
                        tda = new ProductDetailTable();
                        tda.ProID = dtt.Rows[i]["PRO_ID"].ToString();
                        //tda.SUOMlst = BindUOM();
                        tda.DUOMlst = BindUOM();
                        tda.UOM = dtt.Rows[i]["UOM_CODE"].ToString();
                        tda.Des = dtt.Rows[i]["DEST_UOM"].ToString();
                        tda.CF = dtt.Rows[i]["CF"].ToString();
                        tda.Isvalid = "Y";
                        TData.Add(tda);
                    }
                 }
                else
                {

                    for (int i = 0; i < 1; i++)
                    {
                        tda = new ProductDetailTable();
                        //tda.SUOMlst = BindUOM();
                        tda.DUOMlst = BindUOM();
                        tda.ProID = id;
                        tda.Isvalid = "Y";
                        TData.Add(tda);
                    }
                }
            
            ic.ProductDetailTablelst = TData;
            return View(ic);
        }
        [HttpPost]
        public ActionResult AddCF(Productdetail cy, string id)
        {
            try
            {
                cy.ID = id;
                string Strout = ProductNameService.CFCRUD(cy, id);
                if (string.IsNullOrEmpty(Strout))
                {
                    if (cy.ID == null)
                    {
                        TempData["notice"] = "UOM Converted Successfully...!";
                    }
                    else
                    {
                        TempData["notice"] = "UOM Converted Successfully...!";
                    }
                    return RedirectToAction("ListProductdetail");
                }

                else
                {
                    ViewBag.PageTitle = "Edit Productdetail";
                    TempData["notice"] = Strout;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return RedirectToAction("ListProductdetail");
        }
        public List<SelectListItem> BindHsn()
        {
            try
            {
                DataTable dtDesg = ProductNameService.GetHsn();
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["HSCODE"].ToString(), Value = dtDesg.Rows[i]["HSNMASTID"].ToString() });
                }
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<SelectListItem> BindCategory()
        {
            try
            {
                DataTable dtDesg = ProductNameService.GetCategory();
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["PRODUCT_NAME"].ToString(), Value = dtDesg.Rows[i]["ID"].ToString() });
                }
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ActionResult MyListProductNamegrid(string strStatus)
        {
            List<ProsuctNamegrid> Reg = new List<ProsuctNamegrid>();
            DataTable dtUsers = new DataTable();
            strStatus = strStatus == "" ? "Y" : strStatus;
            dtUsers = ProductNameService.GetAllProductNameGRID(strStatus);
            for (int i = 0; i < dtUsers.Rows.Count; i++)
            {

                string Delete = string.Empty;
                string Edit = string.Empty;
                string View = string.Empty;

                if (dtUsers.Rows[i]["IS_ACTIVE"].ToString() == "Y")
                {
                    View = "<a href=ViewProductName?id=" + dtUsers.Rows[i]["PRO_NAME_BASICID"].ToString() + " class='fancybox' data-fancybox-type='iframe'><img src='../Images/file.png' alt='View Details' width='20' /></a>";
                    Edit = "<a href=ProductName?id=" + dtUsers.Rows[i]["PRO_NAME_BASICID"].ToString() + "><img src='../Images/edit.png' alt='Edit'  /></a>";
                    Delete = "DeleteMR?id=" + dtUsers.Rows[i]["PRO_NAME_BASICID"].ToString() + "";
                }
                else
                {
                    View = "";
                    Edit = "";
                    Delete = "Remove?tag=Del&id=" + dtUsers.Rows[i]["PRO_NAME_BASICID"].ToString() + "";
                }
                Reg.Add(new ProsuctNamegrid
                {
                    id = dtUsers.Rows[i]["PRO_NAME_BASICID"].ToString(),
                    category = dtUsers.Rows[i]["PRODUCT_NAME"].ToString(),
                    proname = dtUsers.Rows[i]["PROD_NAME"].ToString(),
                    view = View,
                    edit = Edit,
                    delete = Delete,

                });
            }

            return Json(new
            {
                Reg
            });

        }


        public ActionResult DeleteMR(string tag, string id)
        {
            string flag = "";
            DataTable te = datatrans.GetData("SELECT ITEM FROM DPBASIC B,DPDETAIL D WHERE B.DPBASICID=D.DPBASICID AND D.PRODUCT='" + id + "' UNION SELECT ITEM FROM GRN_BASIC B,GRN_DETAIL D WHERE B.GRN_BASIC_ID=D.GRN_BASIC_ID AND D.PRODUCT='" + id +"'");
            if(te.Rows.Count>0)
            {
                TempData["notice"] = "This Product Can not be Delete";

            }
            else
            {
                flag = ProductNameService.StatusChange(tag, id);

            }
            if (string.IsNullOrEmpty(flag))
            {

                return RedirectToAction("ListProductName");
            }
            else
            {
                TempData["notice"] = flag;
                return RedirectToAction("ListProductName");
            }
        }
        public ActionResult Remove(string tag, string id)
        {

            string flag = ProductNameService.RemoveChange(tag, id);
            if (string.IsNullOrEmpty(flag))
            {

                return RedirectToAction("ListProductName");
            }
            else
            {
                TempData["notice"] = flag;
                return RedirectToAction("ListProductName");
            }
        }
    }
}
