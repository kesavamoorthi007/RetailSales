﻿using DocumentFormat.OpenXml.InkML;
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
        public IActionResult Rate(string id)
        {
            Rate ic = new Rate();
            ic.DocDate = DateTime.Now.ToString("dd-MMM-yyyy");

            List<RateList> TData = new List<RateList>();
            RateList tda = new RateList();
            if (id == null)
            {
                DataTable dtt = new DataTable();
                dtt = RateService.GetproductDetail(id);

                if (dtt.Rows.Count > 0)
                {
                    for (int i = 0; i < dtt.Rows.Count; i++)
                    {
                        tda = new RateList();
                        tda.Item = dtt.Rows[i]["PRODUCT_NAME"].ToString();
                        tda.Varient = dtt.Rows[i]["PRODUCT_VARIANT"].ToString();
                        tda.Unit = dtt.Rows[i]["UOM"].ToString();                        
                        tda.Isvalid = "Y";
                        TData.Add(tda);
                    }

                }  
            }
            else
            {
                DataTable dt = new DataTable();
                dt = RateService.GetEditRate(id);
                if (dt.Rows.Count > 0)
                {
                    //ic.DocNo = dt.Rows[0]["DOC_NO"].ToString();
                    ic.DocDate = dt.Rows[0]["DOC_DATE"].ToString(); 
                    ic.ValidFrom = dt.Rows[0]["VALID_FROM"].ToString();
                    ic.ValidTo = dt.Rows[0]["VALID_TO"].ToString();
                    ic.ID = id;
                }
                DataTable dtt = new DataTable();
                dtt = RateService.GetEditRateDetail(id);
                if (dtt.Rows.Count > 0)
                {
                    for (int i = 0; i < dtt.Rows.Count; i++)
                    {
                        tda = new RateList();
                        tda.Item = dtt.Rows[i]["ITEM_NAME"].ToString();
                        tda.Varient = dtt.Rows[i]["VARIANT"].ToString();
                        tda.Unit = dtt.Rows[i]["UNIT"].ToString();
                        tda.Rate1 = dtt.Rows[i]["RATE"].ToString();
                        tda.ID = id;
                        tda.Isvalid = "Y";
                        TData.Add(tda);
                    }                      
                }
            }
            ic.RateListIdem = TData;
            return View(ic);
        }

        [HttpPost]
        public ActionResult Rate(Rate cy, string id)
        {

            try
            {
                cy.ID = id;
                string Strout = RateService.RateCRUD(cy);
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

                string Delete = string.Empty;
                string Edit = string.Empty;
                //string GoToSales = string.Empty;
                string Revision = string.Empty;

                if (dtUsers.Rows[i]["IS_ACTIVE"].ToString() == "Y")
                {
                    Revision = "<a href=RateView?id=" + dtUsers.Rows[i]["RATE_BASIC_ID"].ToString() + " class='fancyboxs' data-fancybox-type='iframe'><img src='../Images/file.png' alt='View Details' width='20' /></a>";
                    Edit = "<a href=Rate?id=" + dtUsers.Rows[i]["RATE_BASIC_ID"].ToString() + "><img src='../Images/edit.png' alt='Edit'  /></a>";
                    Delete = "<a href=DeleteMR?id=" + dtUsers.Rows[i]["RATE_BASIC_ID"].ToString() + "><img src='../Images/Inactive.png' alt='Deactivate'  /></a>";
                }
                else
                {

                    Edit = "";
                    Delete = "<a href=Remove?tag=Del&id=" + dtUsers.Rows[i]["RATE_BASIC_ID"].ToString() + "><img src='../Images/reactive.png' alt='Reactive' width='28' /></a>";
                }

                Reg.Add(new RateGrid
                {
                    id = dtUsers.Rows[i]["RATE_BASIC_ID"].ToString(),
                    //docno = dtUsers.Rows[i]["DOC_NO"].ToString(),
                    docdate = dtUsers.Rows[i]["DOC_DATE"].ToString(),
                    validfrom = dtUsers.Rows[i]["VALID_FROM"].ToString(),
                    validto = dtUsers.Rows[i]["VALID_TO"].ToString(),
                    editrow = Edit,                    
                    delrow = Delete,
                    revision = Revision,

                });
            }

            return Json(new
            {
                Reg
            });

        }
        public IActionResult RateView(string id)
        {
            Rate ic = new Rate();

            DataTable dt = new DataTable();
            DataTable dtt = new DataTable();

            dt = RateService.GetEditRate(id);
            if (dt.Rows.Count > 0)
            {
                ic.DocDate = dt.Rows[0]["DOC_DATE"].ToString();
                ic.ValidFrom = dt.Rows[0]["VALID_FROM"].ToString();
                ic.ValidTo = dt.Rows[0]["VALID_TO"].ToString();                                                  
                ic.ID = id;

            }
                 
            List<RateList> TData = new List<RateList>();
            RateList tda = new RateList();




            dtt = RateService.GetEditRateDetail(id);
            if (dtt.Rows.Count > 0)
            {
                for (int i = 0; i < dtt.Rows.Count; i++)
                {
                    tda = new RateList();
                    tda.Item = dtt.Rows[i]["ITEM_NAME"].ToString();
                    tda.Varient = dtt.Rows[i]["VARIANT"].ToString();
                    tda.Unit = dtt.Rows[i]["UNIT"].ToString();
                    tda.Rate1 = dtt.Rows[i]["RATE"].ToString();                  
                    tda.ID = id;
                    TData.Add(tda);
                }
            }
            ic.RateListIdem = TData;
            return View(ic);
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
