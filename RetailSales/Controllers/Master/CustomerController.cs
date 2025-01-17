using Microsoft.AspNetCore.Mvc;
using RetailSales.Interface.Master;
using RetailSales.Models;

namespace RetailSales.Controllers.Master
{
    public class CustomerController : Controller
    {
        ICustomerService CustomerService;

        public CustomerController(ICustomerService _CustomerService)
        {
            CustomerService = _CustomerService;
        }
        public IActionResult Customer(string id)
        {
            Customer ic = new Customer();

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
