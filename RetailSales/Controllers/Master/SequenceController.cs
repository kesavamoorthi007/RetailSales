using Microsoft.AspNetCore.Mvc;
using RetailSales.Interface;
using RetailSales.Models;
using RetailSales.Services;
using System.Data;


namespace RetailSales.Controllers
{
    public class SequenceController : Controller
    {
        ISequenceService SequenceService;

        public SequenceController(ISequenceService _SequenceService)
        {
            SequenceService = _SequenceService;
        }
        public IActionResult Sequence(string id)
        {
            Sequence ic = new Sequence();
            ic.Suffix = "24-25";
            if (id == null)
            {

            }
            else
            {
                DataTable dt = new DataTable();
                dt = SequenceService.GetEditSequence(id);
                if (dt.Rows.Count > 0)
                {

                    ic.ID = dt.Rows[0]["ID"].ToString();
                    ic.Transection = dt.Rows[0]["TRANSECTION_TYPE"].ToString();
                    ic.Prefix = dt.Rows[0]["PREFIX"].ToString();
                    ic.Suffix = dt.Rows[0]["SUFFIX"].ToString();
                    ic.Lnumber = dt.Rows[0]["LAST_NUMBER"].ToString();
                    ic.Number = dt.Rows[0]["NUMBER_LENGTH"].ToString();
                   

                }


            }
            return View(ic);
        }
        [HttpPost]
        public ActionResult Sequence(Sequence cy, string id)
        {
            ViewBag.PageTitle = "Sequence";
            try
            {
                cy.ID = id;
                string Strout = SequenceService.SequenceCRUD(cy);
                if (string.IsNullOrEmpty(Strout))
                {
                    if (cy.ID == null)
                    {
                        TempData["notice"] = "Sequence Inserted Successfully...!";
                    }
                    else
                    {
                        TempData["notice"] = "Sequence Updated Successfully...!";
                    }
                    return RedirectToAction("ListSequence");
                }

                else
                {
                    ViewBag.PageTitle = "Edit Sequence";
                    TempData["notice"] = Strout;
                    return RedirectToAction("Sequence");
                }

                // }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return View(cy);
        }
        public IActionResult ListSequence()
        {
            return View();
        }
        public ActionResult MyListSequencegrid(string strStatus)
        {
            List<Sequencegrid> Reg = new List<Sequencegrid>();
            DataTable dtUsers = new DataTable();
            strStatus = strStatus == "" ? "Y" : strStatus;
            dtUsers = SequenceService.GetAllSequenceGRID(strStatus);
            for (int i = 0; i < dtUsers.Rows.Count; i++)
            {

                string DeleteRow = string.Empty;
                string EditRow = string.Empty;
                string a = dtUsers.Rows[i]["IS_ACTIVE"].ToString();
                if (a == "Y")
                {
                    EditRow = "<a href=Sequence?id=" + dtUsers.Rows[i]["ID"].ToString() + "><img src='../Images/edit.png' alt='Edit'  /></a>";
                    DeleteRow = "<a href=DeleteMR?id=" + dtUsers.Rows[i]["ID"].ToString() + "><img src='../Images/Inactive.png' alt='Deactivate'  /></a>";
                }
                else
                {
                    EditRow = "";
                    DeleteRow = "<a href=Remove?tag=Del&id=" + dtUsers.Rows[i]["ID"].ToString() + "><img src='../Images/reactive.png' alt='Reactive' width='28' /></a>";
                }
                Reg.Add(new Sequencegrid
                {
                    id = dtUsers.Rows[i]["ID"].ToString(),
                    transection = dtUsers.Rows[i]["TRANSECTION_TYPE"].ToString(),
                    prefix = dtUsers.Rows[i]["PREFIX"].ToString(),
                    suffix = dtUsers.Rows[i]["SUFFIX"].ToString(),
                    lnumber = dtUsers.Rows[i]["LAST_NUMBER"].ToString(),
                    number = dtUsers.Rows[i]["NUMBER_LENGTH"].ToString(),
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

            string flag = SequenceService.StatusChange(tag, id);
            if (string.IsNullOrEmpty(flag))
            {

                return RedirectToAction("ListSequence");
            }
            else
            {
                TempData["notice"] = flag;
                return RedirectToAction("ListSequence");
            }
        }
        public ActionResult Remove(string tag, string id)
        {

            string flag = SequenceService.RemoveChange(tag, id);
            if (string.IsNullOrEmpty(flag))
            {

                return RedirectToAction("ListSequence");
            }
            else
            {
                TempData["notice"] = flag;
                return RedirectToAction("ListSequence");
            }
        }

    }
}
