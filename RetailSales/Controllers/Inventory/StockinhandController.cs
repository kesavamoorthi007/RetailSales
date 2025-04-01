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
        public ActionResult MyListStockinhandgrid()
        {
            List<Stockinhandgrid> Reg = new List<Stockinhandgrid>();
            DataTable dtUsers = new DataTable();
            dtUsers = Stockinhands.GetAllListStockinhand();
            for (int i = 0; i < dtUsers.Rows.Count; i++)
            {

                Reg.Add(new Stockinhandgrid
                {
                    
                    item = dtUsers.Rows[i]["ITEM_ID"].ToString(),
                    product = dtUsers.Rows[i]["PRODUCT"].ToString(),
                    variant = dtUsers.Rows[i]["VARIANT"].ToString(),
                    uom = dtUsers.Rows[i]["UOM"].ToString(),
                    qty = dtUsers.Rows[i]["BALANCE_QTY"].ToString(),

                });
            }

            return Json(new
            {
                Reg
            });

        }
      
        
    }
}
