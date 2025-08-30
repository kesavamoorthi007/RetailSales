using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using RetailSales.Interface.Accounts;
using RetailSales.Interface.Master;
using System.Data;
using RetailSales.Models.Master;
using RetailSales.Models;
using RetailSales.Services.Master;


namespace RetailSales.Controllers.Master
{
    public class UOMController : Controller
    {
        
        IUOMService UOMService;
        public UOMController(IUOMService _UOMService)
        {
            UOMService = _UOMService;
        }

        public IActionResult UOM(string id)
        {
            UOM ic = new UOM();

            if (id == null)
            {

            }
            else
            {
                DataTable dt = new DataTable();
                dt = UOMService.GetEditUOM(id);
                if (dt.Rows.Count > 0)
                {
                    ic.ID = dt.Rows[0]["ID"].ToString();
                    ic.UOMCODE = dt.Rows[0]["UOM_CODE"].ToString();
                    ic.Description = dt.Rows[0]["UOM_DESCRIPTION"].ToString();
                    ic.Factor = dt.Rows[0]["CONVERSION_FACTOR"].ToString();
                }
            }
            return View(ic);
           
        }
        [HttpPost]

        public ActionResult UOM(UOM cy, string id)
        {
            ViewBag.PageTitle = "UOM";
            try
            {
                cy.ID = id;
                string Strout = UOMService.UOMCRUD(cy);
                if (string.IsNullOrEmpty(Strout))
                {
                    if (cy.ID == null)
                    {
                        TempData["notice"] = "UOM Inserted Successfully...!";
                    }
                    else
                    {
                        TempData["notice"] = "UOM Updated Successfully...!";
                    }
                    return RedirectToAction("ListUOM");
                }

                else
                {
                    ViewBag.PageTitle = "Edit UOM";
                    TempData["notice"] = Strout;
                    return RedirectToAction("UOM");
                }
                
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return View(cy);
        }

        public IActionResult ListUOM()
        {
            return View();
        }

        public ActionResult MyListUOMgrid(string strStatus)
        {
            List<ListUOMGrid> Reg = new List<ListUOMGrid>();
            DataTable dtUsers = new DataTable();
            strStatus = strStatus == "" ? "Y" : strStatus;
            dtUsers = UOMService.GetAllUOMGRID(strStatus);
            for (int i = 0; i < dtUsers.Rows.Count; i++)
            {

                string Delete = string.Empty;
                string Edit = string.Empty;

                if (dtUsers.Rows[i]["IS_ACTIVE"].ToString() == "Y")
                {

                    Edit = "<a href=UOM?id=" + dtUsers.Rows[i]["ID"].ToString() + "><img src='../Images/edit.png' alt='Edit'  /></a>";
                    Delete = "DeleteMR?tag=Del&id=" + dtUsers.Rows[i]["ID"].ToString() + "";
                }
                else
                {

                    Edit = "";
                    Delete = "DeleteMR?tag=Active&id=" + dtUsers.Rows[i]["ID"].ToString() + "";
                }
                Reg.Add(new ListUOMGrid
                {
                    id = dtUsers.Rows[i]["ID"].ToString(),
                    uomcode = dtUsers.Rows[i]["UOM_CODE"].ToString(),
                    description = dtUsers.Rows[i]["UOM_DESCRIPTION"].ToString(),
                    factor = dtUsers.Rows[i]["CONVERSION_FACTOR"].ToString(),
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
            if (tag == "Del")
            {
                flag = UOMService.StatusChange(tag, id);
            }
            else
            {
                flag = UOMService.RemoveChange(tag, id);
            }
            if (string.IsNullOrEmpty(flag))
            {

                return RedirectToAction("ListUOM");
            }
            else
            {
                TempData["notice"] = flag;
                return RedirectToAction("ListUOM");
            }
        }

    }
}
