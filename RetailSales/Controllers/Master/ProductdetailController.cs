using DocumentFormat.OpenXml.Spreadsheet;
using Irony.Parsing;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using RetailSales.Interface.Master;
using RetailSales.Models;
using RetailSales.Models.Inventory;
using RetailSales.Models.Master;
using RetailSales.Services;
using RetailSales.Services.Inventory;
using RetailSales.Services.Master;
using RetailSales.Services.Purchase;
using System.Data;

namespace RetailSales.Controllers.Master
{
    public class ProductdetailController : Controller
    {
        IProductdetailService  ProductdetailService;

        public ProductdetailController(IProductdetailService _ProductdetailService)
        {
            ProductdetailService = _ProductdetailService;
        }

        public IActionResult Productdetail(string id)
        {
            Productdetail ic = new Productdetail();

            ic.Categorylst = BindCategory();
            ic.Productlst = BindProduct("");
            ic.Uomlst = BindUOM();
            ic.Hsnlst = BindHsn();

            if (id == null)
            {

            }
            else
            {
                DataTable dt = new DataTable();
                dt = ProductdetailService.GetEditProductdetail(id);
                if (dt.Rows.Count > 0)
                {
                    ic.ID = dt.Rows[0]["ID"].ToString();
                    ic.Categorylst = BindCategory();
                    ic.Product = dt.Rows[0]["PRODUCT_CATEGORY"].ToString();
                    ic.Productlst = BindProduct(ic.Product);
                    ic.ProName = dt.Rows[0]["PRODUCT_ID"].ToString();
                    ic.Varint = dt.Rows[0]["PRODUCT_VARIANT"].ToString();
                    ic.Uom = dt.Rows[0]["UOM"].ToString();
                    ic.Hsncode = dt.Rows[0]["HSN_CODE"].ToString();
                    ic.Minqty = dt.Rows[0]["MIN_QTY"].ToString();
                    ic.Rate = dt.Rows[0]["RATE"].ToString();
                    ic.Productdescription = dt.Rows[0]["PRODUCT_DESCRIPTION"].ToString();  

                }
            }
            return View(ic);
        }
        [HttpPost]
        public ActionResult Productdetail(Productdetail cy, string id)
        {

            try
            {
                cy.ID = id;
                string Strout = ProductdetailService.ProductdetailCRUD(cy);
                if (string.IsNullOrEmpty(Strout))
                {
                    if (cy.ID == null)
                    {
                        TempData["notice"] = "Productdetail Inserted Successfully...!";
                    }
                    else
                    {
                        TempData["notice"] = "Productdetail Updated Successfully...!";
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

            return View(cy);
        }

        public IActionResult ListProductdetail()
        {
            return View();
        }

        public IActionResult ConversionFactor(string id)
        {
            Productdetail ic = new Productdetail();
            

            DataTable dt = new DataTable();
            DataTable dtt = new DataTable();

            dt = ProductdetailService.GetProductdetail(id);
            if (dt.Rows.Count > 0)
            {
                ic.Product = dt.Rows[0]["PRODUCT_NAME"].ToString();
                ic.ProName = dt.Rows[0]["PROD_NAME"].ToString();
                ic.Varint = dt.Rows[0]["PRODUCT_VARIANT"].ToString();

            }

            List<ProductDetailTable> TData = new List<ProductDetailTable>();
            ProductDetailTable tda = new ProductDetailTable();

            dtt = ProductdetailService.GetProductdetailTable(id);
            if (dtt.Rows.Count > 0)
            {
                tda.Src = dtt.Rows[0]["SRC_UOM"].ToString();
                tda.Des = dtt.Rows[0]["DEST_UOM"].ToString();
                tda.CF = dtt.Rows[0]["CF"].ToString();
                tda.Isvalid = "Y";
            }
            else 
            {
                for (int i = 0; i < 1; i++)
                {
                    tda = new ProductDetailTable();
                    tda.SUOMlst = BindSUOM();
                    tda.DUOMlst = BindDUOM();
                    //tda.CF = dtt.Rows[i]["CONVRT_FACTOR"].ToString();
                    tda.ProID = id;
                    tda.Isvalid = "Y";
                    TData.Add(tda);
                }
            }
            if (id == null)
            {

            }
            else
            {
                dtt = ProductdetailService.GetEditProductdetailTable(id);
                //if (dtt.Rows.Count > 0)
                for (int i = 0; i < dtt.Rows.Count; i++)
                {
                    tda = new ProductDetailTable();
                    tda.ProID = dtt.Rows[i]["PRO_ID"].ToString();
                    tda.SUOMlst = BindSUOM();
                    tda.DUOMlst = BindDUOM();
                    tda.Src = dtt.Rows[i]["SRC_UOM"].ToString();
                    tda.Des = dtt.Rows[i]["DEST_UOM"].ToString();
                    tda.CF = dtt.Rows[i]["CF"].ToString();
                    tda.Isvalid = "Y";
                    TData.Add(tda);
                }
            }
            ic.ProductDetailTablelst = TData;
            return View(ic);
        }
        [HttpPost]
        public ActionResult ConversionFactor(Productdetail cy, string id)
        {
            try
            {
                cy.ID = id;
                string Strout = ProductdetailService.CFCRUD(cy,id);
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

        public ActionResult MyListProductdetailgrid(string strStatus)
        {
            List<Productdetailgrid> Reg = new List<Productdetailgrid>();
            DataTable dtUsers = new DataTable();
            strStatus = strStatus == "" ? "Y" : strStatus;
            dtUsers = ProductdetailService.GetAllProductDeatilsGRID(strStatus);
            for (int i = 0; i < dtUsers.Rows.Count; i++)
            {

                string DeleteRow = string.Empty;
                string EditRow = string.Empty;
                string CF = string.Empty;

                if (dtUsers.Rows[i]["IS_ACTIVE"].ToString() == "Y")
                {
                    EditRow = "<a href=Productdetail?id=" + dtUsers.Rows[i]["ID"].ToString() + "><img src='../Images/edit.png' alt='Edit'  /></a>";
                    DeleteRow = "<a href=DeleteMR?id=" + dtUsers.Rows[i]["ID"].ToString() + "><img src='../Images/Inactive.png' alt='Deactivate'  /></a>";
                    CF = "<a href=ConversionFactor?id=" + dtUsers.Rows[i]["ID"].ToString() + " class='fancyboxs' data-fancybox-type='iframe'><img src='../Images/file.png' alt='View Details' width='20' /></a>";
                }
                else
                {
                    EditRow = "";
                    DeleteRow = "<a href=Remove?tag=Del&id=" + dtUsers.Rows[i]["ID"].ToString() + "><img src='../Images/reactive.png' alt='Reactive' width='28' /></a>";
                    CF = "";
                }
                Reg.Add(new Productdetailgrid
                {
                    id = dtUsers.Rows[i]["ID"].ToString(),
                    product = dtUsers.Rows[i]["PRODUCT_NAME"].ToString(),
                    proname = dtUsers.Rows[i]["PROD_NAME"].ToString(),
                    varint = dtUsers.Rows[i]["PRODUCT_VARIANT"].ToString(),
                    editrow = EditRow,
                    delrow = DeleteRow,
                    cf = CF,

                });
            }

            return Json(new
            {
                Reg
            });
        }

        public List<SelectListItem> BindCategory()
        {
            try
            {
                DataTable dtDesg = ProductdetailService.GetCategory();
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

        public JsonResult GetProductJSON(string productid)
        {
            //EnqItem model = new EnqItem();
            //  model.ItemGrouplst = BindItemGrplst(value);
            return Json(BindProduct(productid));
        }

        public List<SelectListItem> BindProduct(string productid)
        {
            try
            {
                DataTable dtDesg = ProductdetailService.GetProduct(productid);
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["PROD_NAME"].ToString(), Value = dtDesg.Rows[i]["PRO_NAME_BASICID"].ToString() });
                }
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<SelectListItem> BindUOM()
        {
            try
            {
                DataTable dtDesg = ProductdetailService.GetUom();
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

        public List<SelectListItem> BindHsn()
        {
            try
            {
                DataTable dtDesg = ProductdetailService.GetHsn();
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

        public JsonResult GetUOMGrpJSON()
        {
            ProductDetailTable model = new ProductDetailTable();
            model.SUOMlst = BindSUOM();
            return Json(BindSUOM());
        }

        public JsonResult GetDUOMJSON(string id)
        {
            //StockAdjustmentItem model = new StockAdjustmentItem();
            //model.ItemVariantlst = BindItemVariant();
            return Json(BindDUOM());
        }

        private List<SelectListItem> BindSUOM()
        {
            try
            {
                DataTable dtDesg = ProductdetailService.GetSUOM();
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["UOM_CODE"].ToString(), Value = dtDesg.Rows[i]["UOM_CODE"].ToString() });
                }
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private List<SelectListItem> BindDUOM()
        {
            try
            {
                DataTable dtDesg = ProductdetailService.GetDUOM();
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["UOM_CODE"].ToString(), Value = dtDesg.Rows[i]["UOM_CODE"].ToString() });
                }
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ActionResult DeleteMR(string tag, string id)
        {

            string flag = ProductdetailService.StatusChange(tag, id);
            if (string.IsNullOrEmpty(flag))
            {

                return RedirectToAction("ListProductdetail");
            }
            else
            {
                TempData["notice"] = flag;
                return RedirectToAction("ListProductdetail");
            }
        }
        public ActionResult Remove(string tag, string id)
        {

            string flag = ProductdetailService.RemoveChange(tag, id);
            if (string.IsNullOrEmpty(flag))
            {

                return RedirectToAction("ListProductdetail");
            }
            else
            {
                TempData["notice"] = flag;
                return RedirectToAction("ListProductdetail");
            }
        }

       

    }
}

