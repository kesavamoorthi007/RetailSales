using Microsoft.AspNetCore.Mvc;
using RetailSales.Interface;
using RetailSales.Models;
using System.Data;

namespace RetailSales.Controllers
{
    public class StockinhandController : Controller
    {
        IStockinhandService Stockinhands;

        public StockinhandController(IStockinhandService _StockinhandService)
        {
            Stockinhands = _StockinhandService;
        }
        public IActionResult Stockinhand(string id)
        {
            Stockinhand ic = new Stockinhand();

            List<Stockinhand> TData = new List<Stockinhand>();
            Stockinhand tda = new Stockinhand();
            if (id == null)
            {
                for (int i = 0; i < 1; i++)
                {
                    tda = new Stockinhand();
                    //tda.ItemGrouplst = BindItemGrplst();
                    //tda.Isvalid = "Y";
                    TData.Add(tda);
                }
            }
            else
            {


            }
            //ic.Stockinhand = TData;
            return View(ic);
        }
        public IActionResult ListStockinhand()
        {
            return View();
        }
        public ActionResult MyListStockinhandgrid(string strStatus)
        {
            List<Stockinhandgrid> Reg = new List<Stockinhandgrid>();
            DataTable dtUsers = new DataTable();
            strStatus = strStatus == "" ? "Y" : strStatus;
            dtUsers = Stockinhands.GetAllListStockinhand(strStatus);
            for (int i = 0; i < dtUsers.Rows.Count; i++)
            {

                string DeleteRow = string.Empty;
                string EditRow = string.Empty;
                string GoToSales = string.Empty;

                if (dtUsers.Rows[i]["IS_ACTIVE"].ToString() == "Y")
                {

                        EditRow = "<a><img src='../Images/edit.png' alt='Edit' /></a>";

                    DeleteRow = "<a><img src='../Images/Inactive.png' alt='Reactive' width='20' /></a>";

                }
                else
                {
                    DeleteRow = "<a><img src='../Images/Inactive.png' alt='Reactive' width='20' /></a>";
                    EditRow = "";
                }
                Reg.Add(new Stockinhandgrid
                {
                    id = dtUsers.Rows[i]["ID"].ToString(),
                    productname = dtUsers.Rows[i]["PRODUCT_NAME"].ToString(),
                    variant = dtUsers.Rows[i]["VARIANT"].ToString(),
                    balancequantity = dtUsers.Rows[i]["BALANCE_QUANTITY"].ToString(),
                    uom = dtUsers.Rows[i]["UOM"].ToString(),
                    unitcost = dtUsers.Rows[i]["UNIT_COST"].ToString(),
                    totalvalue = dtUsers.Rows[i]["TOTAL_VALUE"].ToString(),
                    editrow = EditRow,
                    //move = GoToSales,
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
