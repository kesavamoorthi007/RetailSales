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

namespace RetailSales.Controllers.Master
{
    public class RateController : Controller
    {
        IRateService RateService;
        public RateController(IRateService _RateService)
        {
            RateService = _RateService;
        }
        //public IActionResult Rate(string id)
        //{
        //    Rate ic = new Rate();
        //    ic.DocDate = DateTime.Now.ToString("dd-MMM-yyyy");

        //    List<RateList> TData = new List<RateList>();
        //    RateList tda = new RateList();
        //    if (id == null)
        //    {
        //        DataTable dtt = new DataTable();
        //        dtt = RateService.GetproductDetail(id);

        //        if (dtt.Rows.Count > 0)
        //        {
        //            for (int i = 0; i < dtt.Rows.Count; i++)
        //            {
        //                tda = new RateList();
        //                tda.Item = dtt.Rows[i]["PRODUCT_NAME"].ToString();
        //                tda.Varient = dtt.Rows[i]["PRODUCT_VARIANT"].ToString();
        //                tda.Unit = dtt.Rows[i]["UOM"].ToString();                        
        //                tda.Isvalid = "Y";
        //                TData.Add(tda);
        //            }

        //        }  
        //    }
        //    else
        //    {
        //        DataTable dt = new DataTable();
        //        dt = RateService.GetEditRate(id);
        //        if (dt.Rows.Count > 0)
        //        {
        //            //ic.DocNo = dt.Rows[0]["DOC_NO"].ToString();
        //            ic.DocDate = dt.Rows[0]["DOC_DATE"].ToString(); 
        //            ic.ValidFrom = dt.Rows[0]["VALID_FROM"].ToString();
        //            ic.ValidTo = dt.Rows[0]["VALID_TO"].ToString();
        //            ic.ID = id;
        //        }
        //        DataTable dtt = new DataTable();
        //        dtt = RateService.GetEditRateDetail(id);
        //        if (dtt.Rows.Count > 0)
        //        {
        //            for (int i = 0; i < dtt.Rows.Count; i++)
        //            {
        //                tda = new RateList();
        //                tda.Item = dtt.Rows[i]["ITEM_NAME"].ToString();
        //                tda.Varient = dtt.Rows[i]["VARIANT"].ToString();
        //                tda.Unit = dtt.Rows[i]["UNIT"].ToString();
        //                tda.Rate1 = dtt.Rows[i]["RATE"].ToString();
        //                tda.ID = id;
        //                tda.Isvalid = "Y";
        //                TData.Add(tda);
        //            }                      
        //        }
        //    }
        //    ic.RateListIdem = TData;
        //    return View(ic);
        //}

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

            dtt = RateService.GetRateViewTable(id);
            if (dtt.Rows.Count > 0)
            {
                for (int i = 0; i < dtt.Rows.Count; i++)
                {
                    tda = new RateListItem();

                    tda.SrcUom = dtt.Rows[0]["UOM_CODE"].ToString();
                    tda.DestUom = dtt.Rows[0]["DEST_UOM"].ToString();
                    tda.CF = dtt.Rows[0]["CF"].ToString();
                    tda.ProdRate = dtt.Rows[0]["RATE"].ToString();
                    tda.Percentage = dtt.Rows[0]["PERCENTAGE"].ToString();
                    tda.SalesRate = dtt.Rows[0]["SALES_RATE"].ToString();
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

        public List<SelectListItem> BindUOM()
        {
            try
            {
                DataTable dtDesg = RateService.GetUom();
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

        //anoter     
        //public JsonResult GetItemGrpJSON()
        //{

        //    RateList model = new RateList();
        //    model.Itemlst = BindItem(); 
        //    return Json(BindItem()); 
        //}

        //public JsonResult GetVarientJSON(string id)
        //{
        //    return Json(BindVariant(id));
        //}


        public IActionResult ListRate()
        {
            return View();
        }
        

//        public List<SelectListItem> BindItem()
//        {
//            try
//            {
//                DataTable dtDesg = RateService.GetItem();
//                List<SelectListItem> lstdesg = new List<SelectListItem>();
//                for (int i = 0; i < dtDesg.Rows.Count; i++)
//                {
//                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["PRODUCT_NAME"].ToString(), Value = dtDesg.Rows[i]["ID"].ToString() });
//                }
//                return lstdesg;
//            }
//            catch (Exception ex)
//            {
//                throw ex;
//            }
//        }

//        private List<SelectListItem> BindVariant(string id)
//        {
//            try
//            {
//                DataTable dtDesg = RateService.GetVariant(id)
//;
//                List<SelectListItem> lstdesg = new List<SelectListItem>();
//                for (int i = 0; i < dtDesg.Rows.Count; i++)
//                {
//                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["PRODUCT_VARIANT"].ToString(), Value = dtDesg.Rows[i]["ID"].ToString() });
//                }
//                return lstdesg;
//            }
//            catch (Exception ex)

//            {
//                throw ex;
//            }
//        }
        //public ActionResult GetItemDetails(string ItemId)
        //{
        //    try
        //    {
        //        DataTable dt = new DataTable();
        //        //string item = "";
        //        string unit = "";
        //        //string rate = "";
        //        dt = RateService.GetItemDetails(ItemId);

        //        if (dt.Rows.Count > 0)
        //        {
                    
        //            unit = dt.Rows[0]["UOM"].ToString();
        //            //rate = dt.Rows[0]["RATE"].ToString();


        //        }
        //        //item = item,, rate = rate 
        //        var result = new {unit = unit};
        //        return Json(result);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        public ActionResult MyListRategrid(string strStatus)
        {
            List<RateGrid> Reg = new List<RateGrid>();
            DataTable dtUsers = new DataTable();
            strStatus = strStatus == "" ? "Y" : strStatus;
            dtUsers = RateService.GetAllRate(strStatus);
            for (int i = 0; i < dtUsers.Rows.Count; i++)
            {

                
                string CF = string.Empty;

                if (dtUsers.Rows[i]["IS_ACTIVE"].ToString() == "Y")
                {
                    CF = "<a href=RateView?id=" + dtUsers.Rows[i]["ID"].ToString() + " class='fancyboxs' data-fancybox-type='iframe'><img src='../Images/file.png' alt='View Details' width='20' /></a>";
                }
                else
                {

                    CF = "";
                }

                Reg.Add(new RateGrid
                {
                    id = dtUsers.Rows[i]["ID"].ToString(),
                    product = dtUsers.Rows[i]["PRODUCT_NAME"].ToString(),
                    proname = dtUsers.Rows[i]["PROD_NAME"].ToString(),
                    varint = dtUsers.Rows[i]["PRODUCT_VARIANT"].ToString(),
                    cf = CF,


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
