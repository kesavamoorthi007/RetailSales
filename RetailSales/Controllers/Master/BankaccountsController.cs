using Microsoft.AspNetCore.Mvc;
using RetailSales.Interface.Master;
using RetailSales.Models;
using RetailSales.Models.Master;

namespace RetailSales.Controllers.Master
{
    public class BankaccountsController : Controller
    {
        IBankaccountsService BankaccountsService;

        public BankaccountsController(IBankaccountsService _BankaccountsService)
        {
            BankaccountsService = _BankaccountsService;
        }
        public IActionResult Bankaccounts(string id)
        {
            Bankaccounts ic = new Bankaccounts();

            if (id == null)
            {

            }
            else
            {


            }
            return View(ic);
        }

    }
}

