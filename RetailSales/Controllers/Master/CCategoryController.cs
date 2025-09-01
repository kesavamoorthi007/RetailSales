using Microsoft.AspNetCore.Mvc;
using RetailSales.Interface.Master;
using RetailSales.Models;
using RetailSales.Services.Master;
using System.Data;

namespace RetailSales.Controllers.Master
{
    public class CCategoryController : Controller
    {
        ICCategoryService CCategoryService;
        IConfiguration? _configuratio;
        private string? _connectionString;
        DataTransactions datatrans;
        public CCategoryController(ICCategoryService _CCategoryService, IConfiguration _configuratio)
        {
            CCategoryService = _CCategoryService;
            _connectionString = _configuratio.GetConnectionString("MySqlConnection");

            datatrans = new DataTransactions(_connectionString);

        }
        public IActionResult CCategory(string id)
        {
            CCategory cc = new CCategory();

            if (id == null)
            {

            }
            else
            {
                DataTable dt = new DataTable();
                dt = CCategoryService.GetEditCCategoryDetail(id);
                if (dt.Rows.Count > 0)
                {
                    cc.ID = dt.Rows[0]["ID"].ToString();
                    cc.Category = dt.Rows[0]["CUSTOMER_CATEGORY"].ToString();
                    cc.Description = dt.Rows[0]["DESCRIPTION"].ToString();

                }

            }
            return View(cc);
        }
        [HttpPost]
        public ActionResult CCategory(CCategory cy, string id)
        {
            ViewBag.PageTitle = "CCategory";
            try
            {
                cy.ID = id;
                string Strout = CCategoryService.CCategoryCRUD(cy);
                if (string.IsNullOrEmpty(Strout))
                {
                    if (cy.ID == null)
                    {
                        TempData["notice"] = "Category Inserted Successfully...!";
                    }
                    else
                    {
                        TempData["notice"] = "Category Updated Successfully...!";
                    }
                    return RedirectToAction("ListCCategory");
                }

                else
                {
                    ViewBag.PageTitle = "Edit CCategory";
                    TempData["notice"] = Strout;
                    return RedirectToAction("CCategory");
                }

                // }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return View(cy);
        }
        public IActionResult ListCCategory()
        {
            return View();
        }
        public ActionResult MyListCCategorygrid(string strStatus)
        {
            List<CCategorygrid> Reg = new List<CCategorygrid>();
            DataTable dtUsers = new DataTable();
            strStatus = strStatus == "" ? "Y" : strStatus;
            dtUsers = CCategoryService.GetAllCCategoryGRID(strStatus);
            for (int i = 0; i < dtUsers.Rows.Count; i++)
            {

                string DeleteRow = string.Empty;
                string EditRow = string.Empty;

                if (dtUsers.Rows[i]["STATUS"].ToString() == "Y")
                {
                    EditRow = "<a href=CCategory?id=" + dtUsers.Rows[i]["ID"].ToString() + "><img src='../Images/edit.png' alt='Edit' /></a>";
                    DeleteRow = "DeleteMR?tag=Del&id=" + dtUsers.Rows[i]["ID"].ToString() + "";
                }
                else
                {
                    EditRow = "";
                    DeleteRow = "DeleteMR?tag=Active&id=" + dtUsers.Rows[i]["ID"].ToString() + "";
                }
                Reg.Add(new CCategorygrid
                {
                    id = dtUsers.Rows[i]["ID"].ToString(),
                    cate = dtUsers.Rows[i]["CUSTOMER_CATEGORY"].ToString(),
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
            string flag = "";
            if (tag == "Del")
            {
                DataTable te = datatrans.GetData("SELECT ITEM FROM DPBASIC B,DPDETAIL D WHERE B.DPBASICID=D.DPBASICID AND D.ITEM='" + id + "' UNION SELECT ITEM FROM GRN_BASIC B,GRN_DETAIL D WHERE B.GRN_BASIC_ID=D.GRN_BASIC_ID AND D.ITEM='" + id + "'");
                if (te.Rows.Count > 0)
                {
                    TempData["notice"] = "This Product Category Can not be Delete";

                }
                else
                {
                    flag = CCategoryService.StatusChange(tag, id);
                }
            }
            else
            {
                flag = CCategoryService.RemoveChange(tag, id);
            }
            if (string.IsNullOrEmpty(flag))
            {

                return RedirectToAction("ListCCategory");
            }
            else
            {
                TempData["notice"] = flag;
                return RedirectToAction("ListCCategory");
            }
        }
        //public ActionResult Remove(string tag, string id)
        //{

        //    string flag = CCategoryService.RemoveChange(tag, id);
        //    if (string.IsNullOrEmpty(flag))
        //    {

        //        return RedirectToAction("ListCCategory");
        //    }
        //    else
        //    {
        //        TempData["notice"] = flag;
        //        return RedirectToAction("ListCCategory");
        //    }
        //}
    }
}
