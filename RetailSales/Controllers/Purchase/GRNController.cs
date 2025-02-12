using Microsoft.AspNetCore.Mvc;
using RetailSales.Interface.Purchase;
using RetailSales.Models;
using RetailSales.Models;
using RetailSales.Services.Purchase;
using System.Data;

namespace RetailSales.Controllers.Purchase
{
    public class GRNController : Controller
    {
        IGRNService GRNService;
        IConfiguration? _configuratio;
        private string? _connectionString;
        DataTransactions datatrans;

        public GRNController(IGRNService _GRNService, IConfiguration _configuratio)
        {
            _connectionString = _configuratio.GetConnectionString("MySqlConnection");
            datatrans = new DataTransactions(_connectionString);
            GRNService = _GRNService;
        }
        public IActionResult GRN()
        {
            Purchaseorder ic = new Purchaseorder();

            return View(ic);
        }
        public IActionResult ListGRN()
        {
            return View();
        }
        public ActionResult MyListGRNgrid(string strStatus)
        {
            List<ListGRNgrid> Reg = new List<ListGRNgrid>();
            DataTable dtUsers = new DataTable();
            strStatus = strStatus == "" ? "Y" : strStatus;
            dtUsers = GRNService.GetAllListGRN(strStatus);
            for (int i = 0; i < dtUsers.Rows.Count; i++)
            {


                //string EditRow = string.Empty;
                //string View = string.Empty;
                string DeleteRow = string.Empty;

                if (dtUsers.Rows[i]["IS_ACTIVE"].ToString() == "Y")
                {
                    
                    //EditRow = "<a href=Purchaseorder?id=" + dtUsers.Rows[i]["GRN_BASIC_ID"].ToString() + "><img src='../Images/edit.png' alt='Edit 'width='20'  /></a>";
                    //View = "<a href=ViewPurchaseOrder?id=" + dtUsers.Rows[i]["GRN_BASIC_ID"].ToString() + " class='fancyboxs' data-fancybox-type='iframe'><img src='../Images/file.png' alt='View Details' width='20' /></a>";
                    DeleteRow = "<a href=Remove?tag=Del&id=" + dtUsers.Rows[i]["GRN_BASIC_ID"].ToString() + "><img src='../Images/Inactive.png' alt='Reactive' width='20' /></a>";

                }
                else
                {
                    //EditRow = "";
                    //View = "<a href=ViewPurchaseOrder?id=" + dtUsers.Rows[i]["GRN_BASIC_ID"].ToString() + " class='fancyboxs' data-fancybox-type='iframe'><img src='../Images/file.png' alt='View Details' width='20' /></a>";
                    DeleteRow = "<a href=Remove?tag=Del&id=" + dtUsers.Rows[i]["GRN_BASIC_ID"].ToString() + "><img src='../Images/Inactive.png' alt='Reactive' width='20' /></a>";
                }

                Reg.Add(new ListGRNgrid
                {
                    id = dtUsers.Rows[i]["GRN_BASIC_ID"].ToString(),
                    grn = dtUsers.Rows[i]["GRN_NO"].ToString(),
                    grndate = dtUsers.Rows[i]["GRN_DATE"].ToString(),
                    sup = dtUsers.Rows[i]["SUPPLIER_NAME"].ToString(),
                    net = dtUsers.Rows[i]["NET"].ToString(),
                    //editrow = EditRow,
                    //view = View,
                    delrow = DeleteRow,

                });
            }

            return Json(new
            {
                Reg
            });

        }
    }
}
