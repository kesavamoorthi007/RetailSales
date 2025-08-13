using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using RetailSales.Models.Master;
using System.Data;
using RetailSales.Models.Master;
using RetailSales.Interface.Master;
using RetailSales.Services.Master;
using RetailSales.Services.Accounts;

namespace RetailSales.Controllers.Master
{
    public class BINMasterController : Controller
    {
        IBINService BINService;
        public BINMasterController(IBINService _BINService)
        {
            BINService = _BINService;
        }

        public IActionResult BINMaster(string id)
        {
            BIN ic = new BIN();
            ic.locationlist = BindLocation();

            if (id == null)
            {

            }
            else
            {

                DataTable dt = new DataTable();
                dt = BINService.GetEditBIN(id);
                if (dt.Rows.Count > 0)
                {
                    ic.ID = dt.Rows[0]["ID"].ToString();
                    ic.BINID = dt.Rows[0]["BINID"].ToString();
                    ic.Location = dt.Rows[0]["LOCID"].ToString();
                    ic.Description = dt.Rows[0]["BINDESC"].ToString();
                }
            }
            return View(ic);
        }
        [HttpPost]

        public ActionResult BINMaster(BIN cy, string id)
        {
            ViewBag.PageTitle = "BIN";
            try
            {
                cy.ID = id;
                string Strout = BINService.BINCRUD(cy);
                if (string.IsNullOrEmpty(Strout))
                {
                    if (cy.ID == null)
                    {
                        TempData["notice"] = "BIN Inserted Successfully...!";
                    }
                    else
                    {
                        TempData["notice"] = "BIN Updated Successfully...!";
                    }
                    return RedirectToAction("ListBIN");
                }

                else
                {
                    ViewBag.PageTitle = "Edit BIN";
                    TempData["notice"] = Strout;
                   
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return RedirectToAction("ListBIN");
        }

        public IActionResult ListBIN()
        {
            return View();
        }

        public List<SelectListItem> BindLocation()
        {
            try
            {
                DataTable dtDesg = BINService.GetLocation();
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i<dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["LOCATION_NAME"].ToString(), Value = dtDesg.Rows[i]["ID"].ToString() });
                }
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ActionResult MyListBINgrid(string strStatus)
        {
            List<ListBIN> Reg = new List<ListBIN>();
            DataTable dtUsers = new DataTable();
            strStatus = strStatus == "" ? "Y" : strStatus;
            dtUsers = BINService.GetAllBINGRID(strStatus);
            for (int i = 0; i < dtUsers.Rows.Count; i++)
            {

                string Delete = string.Empty;
                string Edit = string.Empty;

                if (dtUsers.Rows[i]["IS_ACTIVE"].ToString() == "Y")
                {

                    Edit = "<a href=BINMaster?id=" + dtUsers.Rows[i]["ID"].ToString() + "><img src='../Images/edit.png' alt='Edit'  /></a>";
                    Delete = "<a href=DeleteMR?id=" + dtUsers.Rows[i]["ID"].ToString() + "><img src='../Images/Inactive.png' alt='Deactivate'  /></a>";
                }
                else
                {

                    Edit = "";
                    Delete = "<a href=Remove?tag=Del&id=" + dtUsers.Rows[i]["ID"].ToString() + "><img src='../Images/reactive.png' alt='Reactive' width='28' /></a>";
                }
                Reg.Add(new ListBIN
                {
                    id = dtUsers.Rows[i]["ID"].ToString(),
                    binid = dtUsers.Rows[i]["BINID"].ToString(),
                    description = dtUsers.Rows[i]["BINDESC"].ToString(),
                    location = dtUsers.Rows[i]["LOCATION_NAME"].ToString(),
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

            string flag = BINService.StatusChange(tag, id);
            if (string.IsNullOrEmpty(flag))
            {

                return RedirectToAction("ListBIN");
            }
            else
            {
                TempData["notice"] = flag;
                return RedirectToAction("ListBIN");
            }
        }
        public ActionResult Remove(string tag, string id)
        {

            string flag = BINService.RemoveChange(tag, id);
            if (string.IsNullOrEmpty(flag))
            {

                return RedirectToAction("ListBIN");
            }
            else
            {
                TempData["notice"] = flag;
                return RedirectToAction("ListBIN");
            }
        }

    }
}
