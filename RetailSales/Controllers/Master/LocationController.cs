using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using RetailSales.Interface.Master;
using RetailSales.Models;
using RetailSales.Models.Master;
using RetailSales.Services.Master;
using System.Data;


namespace RetailSales.Controllers.Master
{
    public class LocationController : Controller
    {
        ILocationService LocationService;

        public LocationController(ILocationService _LocationService)
        {
            LocationService = _LocationService;
        }
        public IActionResult Location(string id)
        {
            Location ic = new Location();
            if (id == null)
            {

            }
            else
            {
                DataTable dt = new DataTable();
                dt = LocationService.GetEditLocation(id);
                if (dt.Rows.Count > 0)
                {

                    ic.ID = dt.Rows[0]["ID"].ToString();
                    ic.Locationname = dt.Rows[0]["LOCATION_NAME"].ToString();
                    ic.Address = dt.Rows[0]["LOC_ADDRESS"].ToString();
                    ic.Bin = dt.Rows[0]["BIN"].ToString();

                }


            }
            return View(ic);
        }
        [HttpPost]
        public ActionResult Location(Location cy, string id)
        {

            try
            {
                cy.ID = id;
                string Strout = LocationService.LocationCRUD(cy);
                if (string.IsNullOrEmpty(Strout))
                {
                    if (cy.ID == null)
                    {
                        TempData["notice"] = "Location Inserted Successfully...!";
                    }
                    else
                    {
                        TempData["notice"] = "Location Updated Successfully...!";
                    }
                    return RedirectToAction("ListLocation");
                }

                else
                {
                    ViewBag.PageTitle = "Edit Location";
                    TempData["notice"] = Strout;
                    //return View();
                }

                // }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return View(cy);
        }
        public IActionResult ListLocation()
        {
            return View();
        }

        public ActionResult MyListLocationgrid(string strStatus)
        {
            List<Locationgrid> Reg = new List<Locationgrid>();
            DataTable dtUsers = new DataTable();
            strStatus = strStatus == "" ? "Y" : strStatus;
            dtUsers = LocationService.GetAllLocationGRID(strStatus);
            for (int i = 0; i < dtUsers.Rows.Count; i++)
            {

                string DeleteRow = string.Empty;
                string EditRow = string.Empty;
                string a= dtUsers.Rows[i]["IS_ACTIVE"].ToString();
                if (a == "Y")
                {
                    EditRow = "<a href=Location?id=" + dtUsers.Rows[i]["ID"].ToString() + "><img src='../Images/edit.png' alt='Edit'  /></a>";
                    DeleteRow = "<a href=DeleteMR?id=" + dtUsers.Rows[i]["ID"].ToString() + "><img src='../Images/Inactive.png' alt='Deactivate'  /></a>";
                }
                else
                {
                    EditRow = "";
                    DeleteRow = "<a href=Remove?tag=Del&id=" + dtUsers.Rows[i]["ID"].ToString() + "><img src='../Images/reactive.png' alt='Reactive' width='28' /></a>";
                }
                Reg.Add(new Locationgrid
                {
                    id = dtUsers.Rows[i]["ID"].ToString(),
                    lname = dtUsers.Rows[i]["LOCATION_NAME"].ToString(),
                    address = dtUsers.Rows[i]["LOC_ADDRESS"].ToString(),
                    bin = dtUsers.Rows[i]["BIN"].ToString(),
                    editrow = EditRow,
                    delrow = DeleteRow,

                });
            }

            return Json(new
            {
                Reg
            });

        }
        public ActionResult DeleteMR(string tag, string id)
        {

            string flag = LocationService.StatusChange(tag, id);
            if (string.IsNullOrEmpty(flag))
            {

                return RedirectToAction("ListLocation");
            }
            else
            {
                TempData["notice"] = flag;
                return RedirectToAction("ListLocation");
            }
        }
        public ActionResult Remove(string tag, string id)
        {

            string flag = LocationService.RemoveChange(tag, id);
            if (string.IsNullOrEmpty(flag))
            {

                return RedirectToAction("ListLocation");
            }
            else
            {
                TempData["notice"] = flag;
                return RedirectToAction("ListLocation");
            }
        }

    }
}
