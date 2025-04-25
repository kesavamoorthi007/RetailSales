using System.Data;
using DocumentFormat.OpenXml.Bibliography;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using RetailSales.Interface.Master;
using RetailSales.Models.Master;


namespace RetailSales.Controllers.Master
{
    public class StateController : Controller
    {
        IStateService StateService;
        public StateController(IStateService _StateService)
        {
            StateService = _StateService;
        }
        public IActionResult State(string id)
        {
            State ic = new State();
            ic.Cuntylst = BindCountry();

            if (id == null)
            {

            }
            else
            {
                DataTable dt = new DataTable();
                dt = StateService.GetEditStateDetail(id);
                if (dt.Rows.Count > 0)
                {
                    ic.ID = dt.Rows[0]["ID"].ToString();
                    ic.StatCode = dt.Rows[0]["STATE_CODE"].ToString();
                    ic.StatName = dt.Rows[0]["STATE_NAME"].ToString();
                    ic.ConName = dt.Rows[0]["COUNTRY_ID"].ToString();

                }

            }
            return View(ic);
        }
        [HttpPost]
        public ActionResult State(State Ic, string id)
        {
            ViewBag.PageTitle = "State";
            try
            {
                Ic.ID = id;
                string Strout = StateService.StateCRUD(Ic);
                if (string.IsNullOrEmpty(Strout))
                {
                    if (Ic.ID == null)
                    {
                        TempData["notice"] = "State Inserted Successfully...!";
                    }
                    else
                    {
                        TempData["notice"] = "State Updated Successfully...!";
                    }
                    return RedirectToAction("ListState");
                }

                else
                {
                    ViewBag.PageTitle = "Edit State";
                    TempData["notice"] = Strout;
                    return RedirectToAction("State");
                }

                // }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return View(Ic);
        }
        public List<SelectListItem> BindCountry()
        {
            try
            {
                DataTable dtDesg = StateService.GetCountry();
                List<SelectListItem> lstdesg = new();
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
        public IActionResult ListState()
        {
            return View();
        }

        public ActionResult MyListStategrid(string strStatus)
        {
            List<Stategrid> Reg = new List<Stategrid>();
            DataTable dtUsers = new DataTable();
            strStatus = strStatus == "" ? "Y" : strStatus;
            dtUsers = StateService.GetAllStateGRID(strStatus);
            for (int i = 0; i < dtUsers.Rows.Count; i++)
            {

                string DeleteRow = string.Empty;
                string EditRow = string.Empty;

                if (dtUsers.Rows[i]["IS_ACTIVE"].ToString() == "Y")
                {
                    EditRow = "<a href=State?id=" + dtUsers.Rows[i]["ID"].ToString() + "><img src='../Images/edit.png' alt='Edit'  /></a>";
                    DeleteRow = "<a href=DeleteMR?id=" + dtUsers.Rows[i]["ID"].ToString() + "><img src='../Images/Inactive.png' alt='Deactivate'  /></a>";

                }
                else
                {
                    EditRow = "";
                    DeleteRow = "<a href=Remove?tag=Del&id=" + dtUsers.Rows[i]["ID"].ToString() + "><img src='../Images/reactive.png' alt='Reactive' width='28' /></a>";

                }
                Reg.Add(new Stategrid
                {
                    id = dtUsers.Rows[i]["ID"].ToString(),
                    statcode = dtUsers.Rows[i]["STATE_CODE"].ToString(),
                    statname = dtUsers.Rows[i]["STATE_NAME"].ToString(),
                    conname = dtUsers.Rows[i]["COUNTRY_NAME"].ToString(),
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

            string flag = StateService.StatusChange(tag, id);
            if (string.IsNullOrEmpty(flag))
            {

                return RedirectToAction("ListState");
            }
            else
            {
                TempData["notice"] = flag;
                return RedirectToAction("ListState");
            }
        }
        public ActionResult Remove(string tag, string id)
        {

            string flag = StateService.RemoveChange(tag, id);
            if (string.IsNullOrEmpty(flag))
            {

                return RedirectToAction("ListState");
            }
            else
            {
                TempData["notice"] = flag;
                return RedirectToAction("ListState");
            }
        }

    }
}
