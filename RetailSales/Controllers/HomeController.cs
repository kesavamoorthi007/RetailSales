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
            // Top 10 Products Query
            var dt = datatrans.GetData(@"
        SELECT TOP 10 
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


            // Sales Summary Query
            var dt1 = datatrans.GetData(@"
        SELECT 
            H.INV_DATE,
            COUNT(DISTINCT H.SAL_INV_BASICID) AS Total_Sales,
            SUM(H.NET) AS Total_Amount
        FROM [RetailSales].[dbo].[SALES_INV] H
        WHERE H.IS_ACTIVE = 'Y'
        GROUP BY H.INV_DATE
        ORDER BY H.INV_DATE");

            List<Home> chartData1 = dt1.AsEnumerable().Select(r => new Home
            {
                INV_DATE = Convert.ToDateTime(r["INV_DATE"]),
                Total_Sales = Convert.ToInt32(r["Total_Sales"]),
                Total_Amount = Convert.ToDecimal(r["Total_Amount"])
            }).ToList();

            ViewBag.SalesChart = chartData1;

            var dt2 = datatrans.GetData(@"
    SELECT 
        FORMAT(H.INV_DATE, 'yyyy-MM') AS MonthYear,
        COUNT(DISTINCT H.SAL_INV_BASICID) AS Total_Sales,
        SUM(H.NET) AS Total_Amount
    FROM [RetailSales].[dbo].[SALES_INV] H
    WHERE H.IS_ACTIVE = 'Y'
    GROUP BY FORMAT(H.INV_DATE, 'yyyy-MM')
    ORDER BY MonthYear");

            List<Home> monthlyChartData = dt2.AsEnumerable().Select(r => new Home
            {
                MonthYear = r["MonthYear"].ToString(),
                Total_Sales = Convert.ToInt32(r["Total_Sales"]),
                Total_Amount = Convert.ToDecimal(r["Total_Amount"])
            }).ToList();

            ViewBag.MonthlyChart = monthlyChartData;


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
