using Microsoft.AspNetCore.Mvc;
using RetailSales.Interface.Master;
using RetailSales.Models.Master;
using RetailSales.Services.Master;
using System.Data;


namespace RetailSales.Controllers.Master
{
    public class TaxMasterController : Controller
    {
        ITaxMasterService TaxMasterService;
        public TaxMasterController(ITaxMasterService _TaxMasterService)
        {
            TaxMasterService = _TaxMasterService;
        }

        public IActionResult TaxMaster(string id)
        {
            TaxMaster ic = new TaxMaster();

            if (id == null)
            {

            }
            else
            {
                DataTable dt = new DataTable();
                dt = TaxMasterService.GetEditTaxMaster(id);
                if (dt.Rows.Count > 0)
                {
                    ic.ID = dt.Rows[0]["ID"].ToString();
                    ic.TaxName = dt.Rows[0]["TAX_NAME"].ToString();
                    ic.Percentage = dt.Rows[0]["PERCENTAGE"].ToString();
                    ic.Taxdescription = dt.Rows[0]["TAX_DESC"].ToString();
                }
            }

            return View(ic);
        }
        [HttpPost]
        public ActionResult TaxMaster(TaxMaster cy, string id)
        {
            ViewBag.PageTitle = "TaxMaster";
            try
            {
                cy.ID = id;
                string Strout = TaxMasterService.TaxMasterCRUD(cy);
                if (string.IsNullOrEmpty(Strout))
                {
                    if (cy.ID == null)
                    {
                        TempData["notice"] = "Tax Inserted Successfully...!";
                    }
                    else
                    {
                        TempData["notice"] = "Tax Updated Successfully...!";
                    }
                    return RedirectToAction("ListTaxMaster");
                }

                else
                {
                    ViewBag.PageTitle = "Edit Tax";
                    TempData["notice"] = Strout;
                    return RedirectToAction("TaxMaster");
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return View(cy);
        }

        public IActionResult ListTaxMaster()
        {
            return View();
        }

        public ActionResult MyListTaxMaster(string strStatus)
        {
            List<ListTaxMasterGrid> Reg = new List<ListTaxMasterGrid>();
            DataTable dtUsers = new DataTable();
            strStatus = strStatus == "" ? "Y" : strStatus;
            dtUsers = TaxMasterService.GetAllTaxMaster(strStatus);
            for (int i = 0; i < dtUsers.Rows.Count; i++)
            {

                string Delete = string.Empty;
                string Edit = string.Empty;

                if (dtUsers.Rows[i]["IS_ACTIVE"].ToString() == "Y")
                {

                    Edit = "<a href=TaxMaster?id=" + dtUsers.Rows[i]["ID"].ToString() + "><img src='../Images/edit.png' alt='Edit'  /></a>";
                    Delete = "DeleteMR?tag=Del&id=" + dtUsers.Rows[i]["ID"].ToString() + "";
                }
                else
                {

                    Edit = "";
                    Delete = "DeleteMR?tag=Active&id=" + dtUsers.Rows[i]["ID"].ToString() + "";
                }
                Reg.Add(new ListTaxMasterGrid
                {
                    id = dtUsers.Rows[i]["ID"].ToString(),
                    taxname = dtUsers.Rows[i]["TAX_NAME"].ToString(),
                    percentage = dtUsers.Rows[i]["PERCENTAGE"].ToString(),
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
                flag = TaxMasterService.StatusChange(tag, id);
            }
            else
            {
                flag = TaxMasterService.RemoveChange(tag, id);
            }
            if (string.IsNullOrEmpty(flag))
            {

                return RedirectToAction("ListTaxMaster");
            }
            else
            {
                TempData["notice"] = flag;
                return RedirectToAction("ListTaxMaster");
            }
        }

    }
}
