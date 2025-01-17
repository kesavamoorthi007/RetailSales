using Microsoft.AspNetCore.Mvc;

namespace RetailSales.Controllers.Master
{
    public class ProductController : Controller
    {
        public IActionResult Product()
        {
            return View();
        }

        public IActionResult ListProduct()
        {
            return View();
        }

        public ActionResult MyListProductgrid()
        {
            return View();
        }
    }
}
