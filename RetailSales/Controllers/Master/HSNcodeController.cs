using System.Collections.Generic;
using System.Data;
using RetailSales.Interface;
using RetailSales.Interface.Master;
using RetailSales.Models;
using RetailSales.Services;
using RetailSales.Services.Master;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace RetailSales.Controllers
{
    public class HSNcodeController : Controller
    {
        IHSNcodeService HSNcodeService;
        public HSNcodeController(IHSNcodeService _HSNcodeService)
        {
            HSNcodeService = _HSNcodeService;
        }
        public IActionResult HSNcode(string id)
        {
            HSNcode st = new HSNcode();
            st.createby = Request.Cookies["UserId"];
            //st.CGstlst = BindCGst();
            //st.SGstlst = BindSGst();
            //st.IGstlst = BindIGst();


            List<HSNItem> TData = new List<HSNItem>();
            HSNItem tda = new HSNItem();
            if (id == null)
            {
                for (int i = 0; i < 1; i++)
                {
                    tda = new HSNItem();

                    
                    tda.tarifflst = Bindtarifflst();
                    tda.Isvalid = "Y";
                    TData.Add(tda);
                }

            }

            else
            {
                DataTable dt = new DataTable();
              
                dt = HSNcodeService.GetHSNcode(id);

                if (dt.Rows.Count > 0)
                {
                    st.HCode = dt.Rows[0]["HSCODE"].ToString();
                    st.Dec = dt.Rows[0]["HSDESC"].ToString();
                    st.Per = dt.Rows[0]["GSTP"].ToString();

                }
               
                DataTable dt2 = new DataTable();

                dt2 = HSNcodeService.GettariffItem(id);
                if (dt2.Rows.Count > 0)
                {
                    for (int i = 0; i < dt2.Rows.Count; i++)
                    {
                        tda = new HSNItem();
                        //double toaamt = 0;
                        tda.tarifflst = Bindtarifflst();
                        tda.tariff = dt2.Rows[i]["TARIFFID"].ToString();
                        
                        tda.Isvalid = "Y";
                        TData.Add(tda);
                    }
                }
            }
            st.hsnlst = TData;
            return View(st);
        }
        [HttpPost]
        public ActionResult HSNcode(HSNcode ss, string id)
        {
            ViewBag.PageTitle = "HSNcode";
            try
            {
                ss.ID = id;
                string Strout = HSNcodeService.HSNcodeCRUD(ss);
                if (string.IsNullOrEmpty(Strout))
                {
                    if (ss.ID == null)
                    {
                        TempData["notice"] = " HSNcode Inserted Successfully...!";
                    }
                    else
                    {
                        TempData["notice"] = " HSNcode Updated Successfully...!";
                    }
                    return RedirectToAction("ListHSNcode");
                }

                else
                {
                    ViewBag.PageTitle = "Edit HSNcode";
                    TempData["notice"] = Strout;
                    return RedirectToAction("HSNcode");

                }

                // }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return View(ss);
        }

        public JsonResult GettariffJSON()
        {
            //DeductionItem model = new DeductionItem();
            //  model.ItemGrouplst = BindItemGrplst(value);
            return Json(Bindtarifflst());
        }

        public List<SelectListItem> Bindtarifflst()
        {
            try
            {
                DataTable dtDesg = HSNcodeService.Gettariff();
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["TAX_NAME"].ToString(), Value = dtDesg.Rows[i]["ID"].ToString() });
                }
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        

        //public List<SelectListItem> BindCGst()
        //{
        //    try
        //    {
        //        DataTable dtDesg = HSNcodeService.GetCGst();
        //        List<SelectListItem> lstdesg = new List<SelectListItem>();
        //        for (int i = 0; i < dtDesg.Rows.Count; i++)
        //        {
        //            lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["PERCENTAGE"].ToString(), Value = dtDesg.Rows[i]["PERCENTAGE"].ToString() });
        //        }
        //        return lstdesg;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
        //public List<SelectListItem> BindSGst()
        //{
        //    try
        //    {
        //        DataTable dtDesg = HSNcodeService.GetSGst();
        //        List<SelectListItem> lstdesg = new List<SelectListItem>();
        //        for (int i = 0; i < dtDesg.Rows.Count; i++)
        //        {
        //            lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["PERCENTAGE"].ToString(), Value = dtDesg.Rows[i]["PERCENTAGE"].ToString() });
        //        }
        //        return lstdesg;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
        //public List<SelectListItem> BindIGst()
        //{
        //    try
        //    {
        //        DataTable dtDesg = HSNcodeService.GetIGst();
        //        List<SelectListItem> lstdesg = new List<SelectListItem>();
        //        for (int i = 0; i < dtDesg.Rows.Count; i++)
        //        {
        //            lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["PERCENTAGE"].ToString(), Value = dtDesg.Rows[i]["PERCENTAGE"].ToString() });
        //        }
        //        return lstdesg;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
        public IActionResult ListHSNcode(/*string status*/)
        {
            return View();
        }

        public ActionResult DeleteMR(string tag, string id)
        {
            string flag = "";
            if (tag == "Del")
            {
                flag = HSNcodeService.StatusChange(tag, id);
            }
            else
            {
                flag = HSNcodeService.RemoveChange(tag, id);
            }
            if (string.IsNullOrEmpty(flag))
            {

                return RedirectToAction("ListHSNcode");
            }
            else
            {
                TempData["notice"] = flag;
                return RedirectToAction("ListHSNcode");
            }
        }
        public ActionResult Myhsncodegrid(string strStatus)
        {
            List<HsnList> Reg = new List<HsnList>();
            DataTable dtUsers = new DataTable();
            strStatus = strStatus == "" ? "Y" : strStatus;

            dtUsers = HSNcodeService.GetAllhsncode(strStatus);
            for (int i = 0; i < dtUsers.Rows.Count; i++)
            {

                string DeleteRow = string.Empty;
                string EditRow = string.Empty;

                if (dtUsers.Rows[i]["IS_ACTIVE"].ToString() == "Y")
                {

                    EditRow = "<a href=HSNcode?id=" + dtUsers.Rows[i]["HSNMASTID"].ToString() + "><img src='../Images/edit.png' alt='Edit' /></a>";
                    DeleteRow = "DeleteMR?tag=Del&id=" + dtUsers.Rows[i]["HSNMASTID"].ToString() + "";
                }
                else
                {

                    EditRow = "";
                    DeleteRow = "DeleteMR?tag=Active&id=" + dtUsers.Rows[i]["HSNMASTID"].ToString() + "";
                }

               

                Reg.Add(new HsnList
                {
                    id = Convert.ToInt64(dtUsers.Rows[i]["HSNMASTID"].ToString()),
                    hcode = dtUsers.Rows[i]["HSCODE"].ToString(),
                    dec = dtUsers.Rows[i]["HSDESC"].ToString(),
                    per = dtUsers.Rows[i]["GSTP"].ToString(),
                    editrow = EditRow,
                    delrow = DeleteRow,


                });
            }

            return Json(new
            {
                Reg
            });

        }
        

       
    }
}
