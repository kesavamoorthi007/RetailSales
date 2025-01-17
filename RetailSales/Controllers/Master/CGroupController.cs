using Microsoft.AspNetCore.Mvc;
using RetailSales.Interface.Master;
using RetailSales.Models;
using RetailSales.Services.Master;
using System.Data;

namespace RetailSales.Controllers.Master
{
    public class CGroupController : Controller
    {
        ICGroupService CGroupService;
        public CGroupController(ICGroupService _CGroupService)
        {
            CGroupService = _CGroupService;
        }
        public IActionResult CGroup(string id)
        {
            CGroup cg = new CGroup();

            if (id == null)
            {

            }
            else
            {
                DataTable dt = new DataTable();
                dt = CGroupService.GetEditCGroupDetail(id);
                if (dt.Rows.Count > 0)
                {
                    cg.ID = dt.Rows[0]["ID"].ToString();
                    cg.CustomerGroup = dt.Rows[0]["CUSTOMER_GROUP"].ToString();
                    cg.Description = dt.Rows[0]["DESCRIPTION"].ToString();

                }

            }
            return View(cg);
        }
        [HttpPost]
        public ActionResult CGroup(CGroup Ic, string id)
        {

            try
            {
                Ic.ID = id;
                string Strout = CGroupService.CGroupCRUD(Ic);
                if (string.IsNullOrEmpty(Strout))
                {
                    if (Ic.ID == null)
                    {
                        TempData["notice"] = "Group Inserted Successfully...!";
                    }
                    else
                    {
                        TempData["notice"] = "Group Updated Successfully...!";
                    }
                    return RedirectToAction("ListCGroup");
                }

                else
                {
                    ViewBag.PageTitle = "Edit CGroup";
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
        public IActionResult ListCGroup()
        {
            return View();
        }
        public ActionResult MyListCGroupgrid(string strStatus)
        {
            List<CGroupgrid> Reg = new List<CGroupgrid>();
            DataTable dtUsers = new DataTable();
            strStatus = strStatus == "" ? "Y" : strStatus;
            dtUsers = CGroupService.GetAllCGroupGRID(strStatus);
            for (int i = 0; i < dtUsers.Rows.Count; i++)
            {

                string DeleteRow = string.Empty;
                string EditRow = string.Empty;

                if (dtUsers.Rows[i]["STATUS"].ToString() == "Y")
                {
                    EditRow = "<a href=CGroup?id=" + dtUsers.Rows[i]["ID"].ToString() + "><img src='../Images/edit.png' alt='Edit'  /></a>";
                    DeleteRow = "<a href=DeleteMR?id=" + dtUsers.Rows[i]["ID"].ToString() + "><img src='../Images/Inactive.png' alt='Deactivate'  /></a>";
                }
                else
                {
                    EditRow = "";
                    DeleteRow = "<a href=Remove?tag=Del&id=" + dtUsers.Rows[i]["ID"].ToString() + "><img src='../Images/reactive.png' alt='Reactive' width='28' /></a>";
                }
                Reg.Add(new CGroupgrid
                {
                    id = dtUsers.Rows[i]["ID"].ToString(),
                    group = dtUsers.Rows[i]["CUSTOMER_GROUP"].ToString(),
                    des = dtUsers.Rows[i]["DESCRIPTION"].ToString(),
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

            string flag = CGroupService.StatusChange(tag, id);
            if (string.IsNullOrEmpty(flag))
            {

                return RedirectToAction("ListCGroup");
            }
            else
            {
                TempData["notice"] = flag;
                return RedirectToAction("ListCGroup");
            }
        }
        public ActionResult Remove(string tag, string id)
        {

            string flag = CGroupService.RemoveChange(tag, id);
            if (string.IsNullOrEmpty(flag))
            {

                return RedirectToAction("ListCGroup");
            }
            else
            {
                TempData["notice"] = flag;
                return RedirectToAction("ListCGroup");
            }
        }
    }
}
