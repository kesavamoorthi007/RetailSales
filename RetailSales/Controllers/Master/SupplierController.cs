using Microsoft.AspNetCore.Mvc;
using RetailSales.Interface.Master;
using RetailSales.Models;
using RetailSales.Models.Master;

namespace RetailSales.Controllers.Master
{
    public class SupplierController : Controller
    {
        ISupplierService SupplierService;
        public SupplierController(ISupplierService _SupplierService)
        {
            SupplierService = _SupplierService;
        }
        public IActionResult Supplier(String id)
        {
            Supplier ic = new Supplier();
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
