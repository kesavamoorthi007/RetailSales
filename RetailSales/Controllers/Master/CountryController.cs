using Microsoft.AspNetCore.Mvc;
using RetailSales.Interface.Master;
using RetailSales.Models;
using System.Data;

namespace RetailSales.Controllers.Master
{
    public class CountryController : Controller
    {
        ICountryService CountryService;
        public CountryController(ICountryService _CountryService)
        {
            CountryService = _CountryService;
        }
        public IActionResult Country(string id)
        {
            Country ic = new Country();

            if (id == null)
            {

            }
            else
            {
                //ic = CountryService.GetCountryById(id);

            }
            return View(ic);
        }
        [HttpPost]
        public ActionResult Country(Country Ic, string id)
        {

            try
            {
                Ic.ID = id;
                string Strout = CountryService.CountryCRUD(Ic);
                if (string.IsNullOrEmpty(Strout))
                {
                    if (Ic.ID == null)
                    {
                        TempData["notice"] = "Country Inserted Successfully...!";
                    }
                    else
                    {
                        TempData["notice"] = "Country Updated Successfully...!";
                    }
                    return RedirectToAction("Country");
                }

                else
                {
                    ViewBag.PageTitle = "Edit Country";
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
        public IActionResult ListCountry()
        {
            return View();
        }
        public ActionResult MyListCountrygrid(string strStatus)
        {
            List<Countrygrid> Reg = new List<Countrygrid>();
            DataTable dtUsers = new DataTable();
            strStatus = strStatus == "" ? "Y" : strStatus;
            dtUsers = CountryService.GetAllCountryGRID(strStatus);
            for (int i = 0; i < dtUsers.Rows.Count; i++)
            {

                string DeleteRow = string.Empty;
                string EditRow = string.Empty;

                if (dtUsers.Rows[i]["IS_ACTIVE"].ToString() == "Y")
                {

                    EditRow = "<a href=Country?id=" + dtUsers.Rows[i]["ID"].ToString() + "><img src='../Images/edit.png' alt='Edit' /></a>";
                    DeleteRow = "DeleteMR?tag=Del&id=" + dtUsers.Rows[i]["ID"].ToString() + "";
                }
                else
                {
                    EditRow = "";
                    DeleteRow = "Remove?tag=Del&id=" + dtUsers.Rows[i]["ID"].ToString() + "";
                }


                Reg.Add(new Countrygrid
                {
                    id = dtUsers.Rows[i]["ID"].ToString(),
                    coname = dtUsers.Rows[i]["COUNTRY_NAME"].ToString(),
                    concode = dtUsers.Rows[i]["COUNTRY_CODE"].ToString(),
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
