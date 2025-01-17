using Microsoft.AspNetCore.Mvc;
using RetailSales.Interface.Master;
using RetailSales.Models;

namespace RetailSales.Controllers.Master
{
    public class ProductdetailController : Controller
    {
        IProductdetailService  ProductdetailService;

        public ProductdetailController(IProductdetailService _ProductdetailService)
        {
            ProductdetailService = _ProductdetailService;
        }
        public IActionResult Productdetail(string id)
        {
            Productdetail ic = new Productdetail();

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

