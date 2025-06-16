using DocumentFormat.OpenXml.InkML;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using RetailSales.Interface.Master;
using RetailSales.Interface.Sales;
using RetailSales.Models;
using RetailSales.Models.Master;
using RetailSales.Services.Master;
using RetailSales.Services.Purchase;
using RetailSales.Services.Sales;
using System.Data;
using System.Runtime.InteropServices;

namespace RetailSales.Controllers.Master
{
    public class RateController : Controller
    {
        IRateService RateService;
        IConfiguration? _configuratio;
        private string? _connectionString;
        DataTransactions datatrans;
        public RateController(IRateService _RateService, IConfiguration _configuratio)
        {
            RateService = _RateService;
            _connectionString = _configuratio.GetConnectionString("MySqlConnection");
            datatrans = new DataTransactions(_connectionString);
        }
        

        public IActionResult RateView(string id)
        {
            Rate ic = new Rate();

            DataTable dt = new DataTable();
            DataTable dtt = new DataTable();

            dt = RateService.GetRateView(id);
            if (dt.Rows.Count > 0)
            {
                ic.Product = dt.Rows[0]["PRODUCT_NAME"].ToString();
                ic.ProName = dt.Rows[0]["PROD_NAME"].ToString();
                ic.Varint = dt.Rows[0]["PRODUCT_VARIANT"].ToString();

            }

            List<RateListItem> TData = new List<RateListItem>();
            RateListItem tda = new RateListItem();
            string rate = datatrans.GetDataString("SELECT TOP 1 UNIT_COST FROM INVENTORY_ITEM WHERE VARIANT='" + id + "' ORDER BY DOC_DATE DESC");


            dtt = RateService.GetOldRate(id);
            if (dtt.Rows.Count > 0)
            {
                for (int i = 0; i < dtt.Rows.Count; i++)
                {
                    tda = new RateListItem();
                    tda.DUOMlst = BindUOM();
                    tda.FromRange = dtt.Rows[i]["FROM_RANGE"].ToString();
                    tda.ToRange = dtt.Rows[i]["TO_RANGE"].ToString();
                    tda.DestUom = dtt.Rows[i]["UOM"].ToString();
                    tda.ProdRate = dtt.Rows[i]["RATE"].ToString();
                    tda.Percentage = dtt.Rows[i]["PERCENTAGE"].ToString();
                    tda.SalesRate = dtt.Rows[i]["SALES_RATE"].ToString();
                    tda.Isvalid = "Y";
                    tda.ID = id;
                    TData.Add(tda);
                }
            }
           
            else
            {

                for (int i = 0; i < 1; i++)
                {
                    tda = new RateListItem();
                    //tda.SUOMlst = BindUOM();
                    tda.DUOMlst = BindRUOM(id);
                    tda.ID = id;
                    tda.ProdRate= rate;
                    tda.Isvalid = "Y";
                    TData.Add(tda);
                }
            }
            ic.RateListItemlst = TData;
            return View(ic);




        }
        public IActionResult AddCF(string id)
        {
            Rate ic = new Rate();

            DataTable dt = new DataTable();
            DataTable dtt = new DataTable();

            dt = RateService.GetRateView(id);
            if (dt.Rows.Count > 0)
            {
                ic.Product = dt.Rows[0]["PRODUCT_NAME"].ToString();
                ic.ProName = dt.Rows[0]["PROD_NAME"].ToString();
                ic.Varint = dt.Rows[0]["PRODUCT_VARIANT"].ToString();

            }

            List<RateListItem> TData = new List<RateListItem>();
            RateListItem tda = new RateListItem();

            dtt = RateService.GetRateViewTable(id);
            if (dtt.Rows.Count > 0)
            {
                for (int i = 0; i < dtt.Rows.Count; i++)
                {
                    tda = new RateListItem();
                    tda.SrcUom = dtt.Rows[i]["UOM_CODE"].ToString();
                    tda.SrcUomID = dtt.Rows[i]["SRC_UOM"].ToString();
                    tda.DUOMlst = BindRUOM(id);
                    tda.DestUom = dtt.Rows[i]["DEST_UOM"].ToString();
                    tda.CF = dtt.Rows[i]["CF"].ToString();
                    tda.ProdRate = dtt.Rows[i]["RATE"].ToString();
                    tda.Percentage = dtt.Rows[i]["PERCENTAGE"].ToString();
                    tda.SalesRate = dtt.Rows[i]["SALES_RATE"].ToString();
                    tda.Isvalid = "Y";
                    tda.ID = id;
                    TData.Add(tda);
                }
            }
            else
            {

                for (int i = 0; i < 1; i++)
                {
                    tda = new RateListItem();
                    //tda.SUOMlst = BindUOM();
                    tda.DUOMlst = BindUOM();
                    tda.ID = id;
                    tda.Isvalid = "Y";
                    TData.Add(tda);
                }
            }
            ic.RateListItemlst = TData;
            return View(ic);




        }
        [HttpPost]
        public ActionResult RateView(Rate cy, string id)
        {

            try
            {
                cy.ID = id;
                string Strout = RateService.RateCRUD(cy,id);
                if (string.IsNullOrEmpty(Strout))
                {
                    if (cy.ID == null)
                    {
                        TempData["notice"] = "Rate Inserted Successfully...!";
                    }
                    else
                    {
                        TempData["notice"] = "Rate Updated Successfully...!";
                    }
                    return RedirectToAction("ListRate");
                }

                else
                {
                    ViewBag.PageTitle = "Edit Rate";
                    TempData["notice"] = Strout;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return View(cy);
        }

        [HttpPost]
        public ActionResult AddCF(Rate cy, string id)
        {

            try
            {
                cy.ID = id;
                string Strout = RateService.RateCRUD(cy, id);
                if (string.IsNullOrEmpty(Strout))
                {
                    if (cy.ID == null)
                    {
                        TempData["notice"] = "CF Inserted Successfully...!";
                    }
                    else
                    {
                        TempData["notice"] = "CF Updated Successfully...!";
                    }
                    return RedirectToAction("ListRate");
                }

                else
                {
                    ViewBag.PageTitle = "Edit CF";
                    TempData["notice"] = Strout;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return View(cy);
        }

        public JsonResult GetUOMGrpJSON()
        {
            RateListItem model = new RateListItem();
            model.UOMlst = BindUOM();
            return Json(BindUOM());
        }
        public JsonResult GetRUOMGrpJSON(string proid)
        {
            RateListItem model = new RateListItem();
            model.UOMlst = BindRUOM(proid);
            return Json(BindRUOM(proid));
        }

        public List<SelectListItem> BindUOM()
        {
            try
            {
                DataTable dtDesg =RateService.GetUom() ;
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
        public List<SelectListItem> BindRUOM(string proid)
        {
            try
            {
                DataTable dtDesg = datatrans.GetData("SELECT U.ID,U.UOM_CODE FROM UOM_CONVERT UC LEFT JOIN UOM U ON U.ID=UC.DEST_UOM WHERE PRO_ID='" + proid + "'");
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

        public JsonResult GetProdRateJSON(string proid)
        {
            RateListItem model = new RateListItem();
            string rate = datatrans.GetDataString("SELECT TOP 1 UNIT_COST FROM INVENTORY_ITEM WHERE VARIANT='"+ proid + "' ORDER BY DOC_DATE DESC");
            var result = new { rate = rate };
            return Json(result);
        }

        public List<SelectListItem> BindProdRate()
        {
            try
            {
                DataTable dtDesg = RateService.GetProdRate();
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["RATE"].ToString(), Value = dtDesg.Rows[i]["ID"].ToString() });
                }
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IActionResult ListRate()
        {
            return View();
        }
        
        public ActionResult MyListRategrid(string strStatus)
        {
            List<RateGrid> Reg = new List<RateGrid>();
            DataTable dtUsers = new DataTable();
            strStatus = strStatus == "" ? "Y" : strStatus;
            dtUsers = RateService.GetAllRate(strStatus);
            for (int i = 0; i < dtUsers.Rows.Count; i++)
            {
                string CF = string.Empty;
                string Rate = string.Empty;

                if (dtUsers.Rows[i]["IS_ACTIVE"].ToString() == "Y")
                {
                    CF = "<a href=AddCF?id=" + dtUsers.Rows[i]["ID"].ToString() + "><img src='../Images/file.png' alt='CF' width='20' /></a>";
                    Rate = "<a href=RateView?id=" + dtUsers.Rows[i]["ID"].ToString() + "><img src='../Images/file.png' alt='Rate' width='20' /></a>";
                }
                else
                {
                    Rate = "";
                    CF = "";
                }

                Reg.Add(new RateGrid
                {
                    id = dtUsers.Rows[i]["ID"].ToString(),
                    product = dtUsers.Rows[i]["PRODUCT_NAME"].ToString(),
                    proname = dtUsers.Rows[i]["PROD_NAME"].ToString(),
                    varint = dtUsers.Rows[i]["PRODUCT_VARIANT"].ToString(),
                    cf = CF,
                    rate= Rate,

                });
            }

            return Json(new
            {
                Reg
            });

        }
        

        public ActionResult DeleteMR(string tag, string id)
        {

            string flag = RateService.StatusChange(tag, id);
            if (string.IsNullOrEmpty(flag))
            {

                return RedirectToAction("ListRate");
            }
            else
            {
                TempData["notice"] = flag;
                return RedirectToAction("ListRate");
            }
        }
        public ActionResult Remove(string tag, string id)
        {

            string flag = RateService.RemoveChange(tag, id);
            if (string.IsNullOrEmpty(flag))
            {

                return RedirectToAction("ListRate");
            }
            else
            {
                TempData["notice"] = flag;
                return RedirectToAction("ListRate");
            }
        }

    }      
}
