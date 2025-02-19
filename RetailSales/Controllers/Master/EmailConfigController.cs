using System.Data;
using Microsoft.AspNetCore.Mvc;
using RetailSales.Interface.Master;
using RetailSales.Models.Master;
using RetailSales.Services.Master;

namespace RetailSales.Controllers.Master
{
    public class EmailConfigController : Controller
    {

        IEmailConfigService EmailConfigService;
        public EmailConfigController(IEmailConfigService _EmailConfigService)
        {
            EmailConfigService = _EmailConfigService;
        }

        public IActionResult EmailConfig(string id)
        {
            EmailConfig ic = new EmailConfig();
            
            if (id == null)
            {

            }
            else
            {

                DataTable dt = new DataTable();
                dt = EmailConfigService.GetEditEmailConfig(id);
                if (dt.Rows.Count > 0)
                {
                    ic.ID = dt.Rows[0]["ID"].ToString();
                    ic.Smtphost = dt.Rows[0]["SMTP_HOST"].ToString();
                    ic.Portno = dt.Rows[0]["PORT_NO"].ToString();
                    ic.Emailid = dt.Rows[0]["EMAIL_ID"].ToString();
                }
            }
            return View(ic);
        }

        [HttpPost]
        public ActionResult EmailConfig(EmailConfig cy, string id)
        {

            try
            {
                cy.ID = id;
                string Strout = EmailConfigService.EmailConfigCRUD(cy);
                if (string.IsNullOrEmpty(Strout))
                {
                    if (cy.ID == null)
                    {
                        TempData["notice"] = "EmailConfig Inserted Successfully...!";
                    }
                    else
                    {
                        TempData["notice"] = "EmailConfig Updated Successfully...!";
                    }
                    return RedirectToAction("ListEmailConfig");
                }

                else
                {
                    ViewBag.PageTitle = "Edit EmailConfig";
                    TempData["notice"] = Strout;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return View(cy);
        }

        public IActionResult ListEmailConfig()
        {
            return View();
        }

        public ActionResult MyListEmailConfiggrid(string strStatus)
        {
            List<ListEmailConfig> Reg = new List<ListEmailConfig>();
            DataTable dtUsers = new DataTable();
            strStatus = strStatus == "" ? "Y" : strStatus;
            dtUsers = EmailConfigService.GetAllEmailConfigGRID(strStatus);
            for (int i = 0; i < dtUsers.Rows.Count; i++)
            {

                string Delete = string.Empty;
                string Edit = string.Empty;

                if (dtUsers.Rows[i]["IS_ACTIVE"].ToString() == "Y")
                {

                    Edit = "<a href=EmailConfig?id=" + dtUsers.Rows[i]["ID"].ToString() + "><img src='../Images/edit.png' alt='Edit'  /></a>";
                    Delete = "<a href=DeleteMR?id=" + dtUsers.Rows[i]["ID"].ToString() + "><img src='../Images/Inactive.png' alt='Deactivate'  /></a>";
                }
                else
                {

                    Edit = "";
                    Delete = "<a href=Remove?tag=Del&id=" + dtUsers.Rows[i]["ID"].ToString() + "><img src='../Images/reactive.png' alt='Reactive' width='28' /></a>";
                }
                Reg.Add(new ListEmailConfig
                {
                    id = dtUsers.Rows[i]["ID"].ToString(),
                    smtphost = dtUsers.Rows[i]["SMTP_HOST"].ToString(),
                    portno = dtUsers.Rows[i]["PORT_NO"].ToString(),
                    emailid = dtUsers.Rows[i]["EMAIL_ID"].ToString(),
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

            string flag = EmailConfigService.StatusChange(tag, id);
            if (string.IsNullOrEmpty(flag))
            {

                return RedirectToAction("ListEmailConfig");
            }
            else
            {
                TempData["notice"] = flag;
                return RedirectToAction("ListEmailConfig");
            }
        }
        public ActionResult Remove(string tag, string id)
        {

            string flag = EmailConfigService.RemoveChange(tag, id);
            if (string.IsNullOrEmpty(flag))
            {

                return RedirectToAction("ListEmailConfig");
            }
            else
            {
                TempData["notice"] = flag;
                return RedirectToAction("ListEmailConfig");
            }
        }
    }
}
