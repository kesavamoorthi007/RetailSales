using Microsoft.AspNetCore.Mvc;
using RetailSales.Interface.Master;

namespace RetailSales.Controllers.Master
{
    public class CountryController : Controller
    {
        ICountryService CountryService;
        public CountryController(ICountryService _CountryService)
        {
            CountryService = _CountryService;
        }
        //public IActionResult Country(string id)
        //{
        //    Country ic = new Country();
        //    if (id == null)
        //    {


        //    }
        //    else
        //    {
                
        //        //ic = CountryService.GetCountryById(id);

        //    }


        //    return View(ic);
        //}
        
    }
}
