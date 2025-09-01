using System.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using RetailSales.Interface.Accounts;
using RetailSales.Interface.Purchase;
using RetailSales.Models;
using RetailSales.Models.Accounts;
using RetailSales.Models.Inventory;
using RetailSales.Services.Accounts;
using RetailSales.Services.Inventory;
using RetailSales.Services.Master;
using RetailSales.Services.Purchase;

namespace RetailSales.Controllers.Accounts
{
    public class PaymentRequestController : Controller
    {
        IPaymentRequestService PaymentRequestService;
        IConfiguration? _configuratio;
        private string? _connectionString;
        DataTransactions datatrans;

        public PaymentRequestController(IPaymentRequestService _PaymentRequestService, IConfiguration _configuratio)
        {
            _connectionString = _configuratio.GetConnectionString("MySqlConnection");
            datatrans = new DataTransactions(_connectionString);
            PaymentRequestService = _PaymentRequestService;
        }

        public IActionResult PaymentRequest(string id)
        {
            PaymentRequest ic = new PaymentRequest();
            ic.Suplst = BindSupplier();
            ic.ReqBy = HttpContext.Request.Cookies["EmpName"];

            DataTable dttt = new DataTable();
            List<Models.Accounts.GRNItem> TData = new List<Models.Accounts.GRNItem>();
            Models.Accounts.GRNItem tda = new Models.Accounts.GRNItem();

            dttt = PaymentRequestService.GetPaymentRequestItem(id);
            if(id == null)
            {
                if (dttt.Rows.Count > 0)
                {
                    for (int i = 0; i < dttt.Rows.Count; i++)
                    {
                        tda = new Models.Accounts.GRNItem();
                        tda.GRNNo = dttt.Rows[i]["GRN_NO"].ToString();
                        tda.GRNAmt = dttt.Rows[i]["NET"].ToString();
                        tda.ID = id;
                        tda.Isvalid = "Y";
                        TData.Add(tda);
                    }
                }
            }
            else
            {
                DataTable dt = new DataTable();
                dt = PaymentRequestService.GetEditPaymentRequest(id);
                if(dt.Rows.Count > 0)
                {
                    ic.Suplst = BindSupplier();
                    ic.SuppName = dt.Rows[0]["SUP_ID"].ToString();
                    ic.ReqBy = dt.Rows[0]["REQ_BY"].ToString();
                    ic.TotReqAmt = dt.Rows[0]["TOT_REQ_AMT"].ToString();
                    ic.ID = id;
                }
                DataTable dtt = new DataTable();
                dtt = PaymentRequestService.GetEditPaymentRequestTable(id);
                if(dtt.Rows.Count > 0)
                {
                    for(int i = 0; i < dtt.Rows.Count; i++)
                    {
                        tda = new Models.Accounts.GRNItem();
                        tda.GRNNo = dtt.Rows[i]["GRN_NO"].ToString();
                        tda.GRNAmt = dtt.Rows[i]["GRN_AMT"].ToString();
                        tda.ReqAmt = dtt.Rows[i]["REQ_AMT"].ToString();
                        tda.PendAmt = dtt.Rows[i]["PEND_AMT"].ToString();
                        tda.Isvalid = "Y";
                        tda.ID = id;
                        TData.Add(tda);
                    }
                }
            }

            ic.GRNlst = TData;
            return View(ic);
        }
        [HttpPost]
        public ActionResult PaymentRequest(PaymentRequest cy, string id)
        {

            try
            {
                cy.ID = id;
                string Strout = PaymentRequestService.PaymentRequestCRUD(cy);
                if (string.IsNullOrEmpty(Strout))
                {
                    if (cy.ID == null)
                    {
                        TempData["notice"] = "PaymentRequest Inserted Successfully...!";
                    }
                    else
                    {
                        TempData["notice"] = "PaymentRequest Updated Successfully...!";
                    }
                    return RedirectToAction("ListPaymentRequest");
                }

                else
                {
                    ViewBag.PageTitle = "Edit PaymentRequest";
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
        public ActionResult ViewPaymentRequest(string id)
        {
            PaymentRequest ic = new PaymentRequest();
            DataTable dt = new DataTable();

            dt = PaymentRequestService.GetPaymentRequest(id);
            if (dt.Rows.Count > 0)
            {
                ic.SuppName = dt.Rows[0]["SUPPLIER_NAME"].ToString();
                ic.ReqBy = dt.Rows[0]["REQ_BY"].ToString();
                ic.TotReqAmt = dt.Rows[0]["TOT_REQ_AMT"].ToString();
            }
            DataTable dtt = new DataTable();

            List<Models.Accounts.GRNItem> TData = new List<Models.Accounts.GRNItem>();
            Models.Accounts.GRNItem tda = new Models.Accounts.GRNItem();

            dtt = PaymentRequestService.GetPaymentRequestTable(id);
            if (dtt.Rows.Count > 0)
            {
                for (int i = 0; i < dtt.Rows.Count; i++)
                {
                    tda = new Models.Accounts.GRNItem();
                    tda.GRNNo = dtt.Rows[i]["GRN_NO"].ToString();
                    tda.GRNAmt = dtt.Rows[i]["GRN_AMT"].ToString();
                    tda.ReqAmt = dtt.Rows[i]["REQ_AMT"].ToString();
                    tda.PendAmt = dtt.Rows[i]["PEND_AMT"].ToString();
                    tda.ID = id;
                    TData.Add(tda);
                }
            }
            ic.GRNlst = TData;
            return View(ic);
        }
        public IActionResult ListPaymentRequest()
        {
            return View();
        }
        public ActionResult MyListPaymentRequestgrid(string strStatus)
        {
            List<ListPaymentRequestgrid> Reg = new List<ListPaymentRequestgrid>();
            DataTable dtUsers = new DataTable();
            strStatus = strStatus == "" ? "Y" : strStatus;
            dtUsers = PaymentRequestService.GetAllListPurchaseorder(strStatus);
            for (int i = 0; i < dtUsers.Rows.Count; i++)
            {
                string Edit = string.Empty;
                string Delete = string.Empty;
                string View = string.Empty;

                if (dtUsers.Rows[i]["IS_ACTIVE"].ToString() == "Y")
                {
                    //View = "<a href=ViewPaymentRequest?id=" + dtUsers.Rows[i]["PAYREQBASICID"].ToString() + "' target='_blank'><img src='../Images/file.png' alt='View Details' width='20' /></a>";
                    View = "<a href='ViewPaymentRequest?id=" + dtUsers.Rows[i]["PAYREQBASICID"].ToString() + "' target='_blank'><img src='../Images/file.png' alt='View Details' width='20' /></a>";
                    Edit = "<a href=PaymentRequest?id=" + dtUsers.Rows[i]["PAYREQBASICID"].ToString() + "><img src='../Images/edit.png' alt='Edit'  /></a>";
                    Delete = "DeleteMR?tag=Del&id=" + dtUsers.Rows[i]["PAYREQBASICID"].ToString() + "";
                }
                else
                {
                    View = "";
                    Edit = "";
                    Delete = "DeleteMR?tag=Active&id=" + dtUsers.Rows[i]["PAYREQBASICID"].ToString() + "";
                }
                Reg.Add(new ListPaymentRequestgrid
                {
                    id = dtUsers.Rows[i]["PAYREQBASICID"].ToString(),
                    suppname = dtUsers.Rows[i]["SUPPLIER_NAME"].ToString(),
                    reqby = dtUsers.Rows[i]["REQ_BY"].ToString(),
                    totreqamt = dtUsers.Rows[i]["TOT_REQ_AMT"].ToString(),
                    view = View,
                    edit = Edit,
                    delete = Delete,
                    
                });
            }

            return Json(new
            {
                Reg
            });

        }
        public List<SelectListItem> BindSupplier()
        {
            try
            {
                DataTable dtDesg = datatrans.GetData("select ID,SUPPLIER_NAME from SUPPLIER where SUPPLIER.IS_ACTIVE='Y'");
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["SUPPLIER_NAME"].ToString(), Value = dtDesg.Rows[i]["ID"].ToString() });
                }
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ActionResult GetSupplierDetails(string suppid)
        {
            try
            {
                PaymentRequest ca = new PaymentRequest();
                List<Models.Accounts.GRNItem> TData = new List<Models.Accounts.GRNItem>();
                Models.Accounts.GRNItem dta = new Models.Accounts.GRNItem();
                //string grnno = "";
                //string grnamt = "";
                DataTable dtPR = new DataTable();

                dtPR = PaymentRequestService.GetGRNDetails(suppid);

                if (dtPR.Rows.Count > 0)
                {
                    for (int i = 0; i < dtPR.Rows.Count; i++)
                    {
                        dta = new Models.Accounts.GRNItem();
                        dta.GRNNo = dtPR.Rows[i]["GRN_NO"].ToString();
                        dta.GRNAmt = dtPR.Rows[i]["NET"].ToString();
                        dta.Isvalid = "Y";
                        TData.Add(dta);
                    }
                }
                ca.GRNlst = TData;
                return Json(ca.GRNlst);
                //var result = new { grnno = grnno, grnamt = grnamt };
                //return Json(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ActionResult DeleteMR(string tag, string id)
        {
            string flag = "";
            if (tag == "Del")
            {
                flag = PaymentRequestService.StatusChange(tag, id);
            }
            else
            {
                flag = PaymentRequestService.RemoveChange(tag, id);
            }
            if (string.IsNullOrEmpty(flag))
            {

                return RedirectToAction("ListPaymentRequest");
            }
            else
            {
                TempData["notice"] = flag;
                return RedirectToAction("ListPaymentRequest");
            }
        }
    }

}
