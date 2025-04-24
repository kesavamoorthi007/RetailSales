
using DocumentFormat.OpenXml.Bibliography;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using RetailSales.Interface;
using RetailSales.Models;

using RetailSales.Services;
using System.Data;
using City = RetailSales.Models.City;

namespace RetailSales.Controllers
{

    // implementing interface
    public class CityController : Controller
    {
        ICityService CityServices;
        public CityController(ICityService _CityServices)
        {
            CityServices = _CityServices;
        }

        // inserting to database
        public IActionResult City(string id)
        {
            City ic = new City();

            // country binding
            ic.colist = BindCountry();
            // state binding
            ic.stlist = BindState("");

            if (id == null)
            {

            }
            else
            {
                DataTable dt = new DataTable();
                dt = CityServices.GetEditCityDetail(id);
                if (dt.Rows.Count > 0)
                {
                    ic.ID = dt.Rows[0]["ID"].ToString();
                    ic.colist = BindCountry();
                    ic.CountryId = dt.Rows[0]["COUNTRY_ID"].ToString();
                    ic.stlist = BindState(ic.CountryId);
                    ic.StateId = dt.Rows[0]["STATE_ID"].ToString();
                    ic.CityName = dt.Rows[0]["CITY_NAME"].ToString();
                    
                }
            }
            return View(ic);
        }
        [HttpPost]

        // action after i/p submitted
        public ActionResult City(City Ic, string id)
        {
            try
            {
                Ic.ID = id;
                string Strout = CityServices.CityCRUD(Ic);
                if (string.IsNullOrEmpty(Strout))
                {
                    if (Ic.ID == null)
                    {
                        TempData["notice"] = "City Inserted Successfully...!";
                    }
                    else
                    {
                        TempData["notice"] = "City Updated Successfully...!";
                    }
                    return RedirectToAction("ListCity");
                }

                else
                {
                    ViewBag.PageTitle = "Edit City";
                    TempData["notice"] = Strout;
                    //return View();
                }

                // }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return View(Ic);
        }

        // Binding Country
        public List<SelectListItem> BindCountry()
        {
            try
            {
                DataTable dtDesg = CityServices.GetCountry();
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["COUNTRY_NAME"].ToString(), Value = dtDesg.Rows[i]["ID"].ToString() });
                }
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // Binding State
        public List<SelectListItem> BindState(string stateid)
        {
            try
            {
                DataTable dtDesg1 = CityServices.GetState(stateid);
                List<SelectListItem> lstdesg1 = new List<SelectListItem>();
                for (int i = 0; i < dtDesg1.Rows.Count; i++)
                {
                    lstdesg1.Add(new SelectListItem() { Text = dtDesg1.Rows[i]["STATE_NAME"].ToString(), Value = dtDesg1.Rows[i]["ID"].ToString() });
                }
                return lstdesg1;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public JsonResult GetstateJSON(string stateid)
        {
            //EnqItem model = new EnqItem();
            //  model.ItemGrouplst = BindItemGrplst(value);
            return Json(BindState(stateid));
        }

        // retrieves to listcity view page
        public IActionResult ListCity() 
        {
            return View();
        }

        public ActionResult MyListCitygrid(string strStatus)
        {
            List<Citygrid> Reg = new List<Citygrid>();
            DataTable dtUsers = new DataTable();
            strStatus = strStatus == "" ? "Y" : strStatus;
            dtUsers = CityServices.GetAllCityGRID(strStatus);
            for (int i = 0; i < dtUsers.Rows.Count; i++)
            {

                string DeleteRow = string.Empty;
                string EditRow = string.Empty;

                if (dtUsers.Rows[i]["IS_ACTIVE"].ToString() == "Y")
                {
                    EditRow = "<a href=City?id=" + dtUsers.Rows[i]["ID"].ToString() + "><img src='../Images/edit.png' alt='Edit'  /></a>";
                    DeleteRow = "<a href=DeleteMR?id=" + dtUsers.Rows[i]["ID"].ToString() + "><img src='../Images/Inactive.png' alt='Deactivate'  /></a>";
                }
                else
                {
                    EditRow = "";
                    DeleteRow = "<a href=Remove?tag=Del&id=" + dtUsers.Rows[i]["ID"].ToString() + "><img src='../Images/reactive.png' alt='Reactive' width='28' /></a>";
                }
                Reg.Add(new Citygrid
                {
                    id = dtUsers.Rows[i]["ID"].ToString(),
                    ciname = dtUsers.Rows[i]["CITY_NAME"].ToString(),
                    statid = dtUsers.Rows[i]["STATE_NAME"].ToString(),
                    counid = dtUsers.Rows[i]["COUNTRY_NAME"].ToString(),
                    
                    editrow = EditRow,
                    delrow = DeleteRow,

                });
            }

            return Json(new
            {
                Reg
            });

        }

        // delete action
        public ActionResult DeleteMR(string tag, string id)
        {

            string flag = CityServices.StatusChange(tag, id);
            if (string.IsNullOrEmpty(flag))
            {

                return RedirectToAction("ListCity");
            }
            else
            {
                TempData["notice"] = flag;
                return RedirectToAction("ListCity");
            }
        }

        // disabled page
        public ActionResult Remove(string tag, string id)
        {

            string flag = CityServices.RemoveChange(tag, id);
            if (string.IsNullOrEmpty(flag))
            {

                return RedirectToAction("ListCity");
            }
            else
            {
                TempData["notice"] = flag;
                return RedirectToAction("ListCity");
            }
        }
    }
}
