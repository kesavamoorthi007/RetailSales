using Microsoft.AspNetCore.Mvc;
using RetailSales.Interface;
using RetailSales.Interface.Purchase;
using RetailSales.Models;
using RetailSales.Services.Purchase;
using System.Data;
using System.Diagnostics;

namespace RetailSales.Controllers
{
    public class HomeController : Controller
    {
        IHomeService HomeService;
        IConfiguration? _configuratio;
        private string? _connectionString;
        DataTransactions datatrans;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger,IHomeService _HomeService, IConfiguration _configuratio)
        {
            _logger = logger;
            _connectionString = _configuratio.GetConnectionString("MySqlConnection");
            datatrans = new DataTransactions(_connectionString);
            HomeService = _HomeService;
        }
        public IActionResult Index()
        {
            var dt = datatrans.GetData(@"SELECT TOP 10 
                                    P.PRODUCT_NAME AS ProductName,
                                    V.PRODUCT_VARIANT AS Variant,
                                    SUM(CAST(D.QTY AS DECIMAL(18,2))) AS TotalQtySold
                                FROM [RetailSales].[dbo].[SAL_INV_DEATILS] D
                                INNER JOIN [RetailSales].[dbo].[SALES_INV] B ON D.SAL_INV_BASICID = B.SAL_INV_BASICID
                                LEFT JOIN [RetailSales].[dbo].[PRODUCT] P ON P.ID = D.ITEM
                                LEFT JOIN [RetailSales].[dbo].[PRO_DETAIL] V ON V.ID = D.VARIENT
                                WHERE B.IS_ACTIVE = 'Y'
                                GROUP BY P.PRODUCT_NAME, V.PRODUCT_VARIANT
                                ORDER BY TotalQtySold DESC");

            List<Home> chartData = dt.AsEnumerable().Select(r => new Home
            {
                ProductName = r["ProductName"].ToString(),
                Variant = r["Variant"].ToString(),
                TotalQtySold = Convert.ToInt32(r["TotalQtySold"])
            }).ToList();

            ViewBag.ProductChart = chartData;

            return View();
        }

        public IActionResult Privacy()
        {
            return View();

        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
