using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.AspNetCore.Http.Features;
using RetailSales.Interface;
using RetailSales.Services;
using RetailSales.Interface.Master;
using RetailSales.Services.Master;

using RetailSales.Services.Sales;
using RetailSales.Interface.Sales;

 
using RetailSales.Interface.Purchase;
using RetailSales.Services.Purchase;
using RetailSales.Interface.Accounts;
using RetailSales.Services.Accounts;

 
using RetailSales.Services.Inventory;
using RetailSales.Interface.Inventory;
 


using RetailSales;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddControllersWithViews();

        builder.Services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
		builder.Services.TryAddSingleton<ILoginService, LoginService>();
		builder.Services.TryAddSingleton<ICountryService, CountryService>();
        builder.Services.TryAddSingleton<IEmployeeService, EmployeeService>();
        builder.Services.TryAddSingleton<ICustomerService, CustomerService>();
        builder.Services.TryAddSingleton<ILocationService, LocationService>();
        builder.Services.TryAddSingleton<IProductdetailService, ProductdetailService>();
        builder.Services.TryAddSingleton<IPurchaseorderService, PurchaseorderService>();
        builder.Services.TryAddSingleton<ISalesReturnService, SalesReturnService>();
        builder.Services.TryAddSingleton<ISequenceService, SequenceService>();
        builder.Services.TryAddSingleton<IStateService, StateService>();
		builder.Services.TryAddSingleton<ICCategoryService, CCategoryService>();
		builder.Services.TryAddSingleton<ICGroupService, CGroupService>();
        builder.Services.TryAddSingleton<IProductService, ProductService>();
        builder.Services.TryAddSingleton<ICompanyService, CompanyService>();

		builder.Services.TryAddSingleton<ISalesInvoiceService, SalesInvoiceService>();
		builder.Services.TryAddSingleton<IStockinhandService, StockinhandService>();
		builder.Services.TryAddSingleton<IStockTransferService, StockTransferService>();
		builder.Services.TryAddSingleton<IBankaccountsService, BankaccountsService>();
		builder.Services.TryAddSingleton<IProductService, ProductService>();
		builder.Services.TryAddSingleton<IDebitNoteService, DebitNoteService>();
		builder.Services.TryAddSingleton<ICreditNoteService, CreditNoteService>();
		builder.Services.TryAddSingleton<IContraVoucherService, ContraVoucherService>();
		builder.Services.TryAddSingleton<IJournalVoucherService, JournalVoucherService>();
        builder.Services.TryAddSingleton<ILedgersServices, LedgersService>();
        builder.Services.TryAddSingleton<IHSNcodeService, HSNcodeService>();

        // adding ProductName interface and services containers
        builder.Services.TryAddSingleton<IProductNameService, ProductNameService>();
        // adding City interface and services containers
        builder.Services.TryAddSingleton<ICityService, CityServices>();
        // adding Supplier interface and services containers
        builder.Services.TryAddSingleton<ISupplierService, SupplierService>();
        // adding UOM interface and services containers
        builder.Services.TryAddSingleton<IUOMService, UOMService>();
        // adding BIN interface and services containers
        builder.Services.TryAddSingleton<IBINService, BINService>();
        // adding Account Group interface and services containers
        builder.Services.TryAddSingleton<IAccountGroupService, AccountGroupService>();
        // adding Stock Adjustment interface and services containers
        builder.Services.TryAddSingleton<IStockAdjustmentService, StockAdjustmentService>();
        // adding Email Config interface and services containers
        builder.Services.TryAddSingleton<IEmailConfigService, EmailConfigService>();
        // adding Payment Request interface and services containers
        builder.Services.TryAddSingleton<IPaymentRequestService, PaymentRequestService>();

        builder.Services.TryAddSingleton<ITaxMasterService, TaxMasterService>();
        builder.Services.TryAddSingleton<IDirectPurchaseService, DirectPurchaseService>();
        builder.Services.TryAddSingleton<IRateService, RateService>();
        builder.Services.TryAddSingleton<IGRNService, GRNService>();

		builder.Services.TryAddSingleton<IAccConfig, AccConfigService>();

		builder.Services.AddSession();


        builder.Services.AddControllers();
        builder.Services.Configure<FormOptions>(x =>
        {
            x.ValueCountLimit = int.MaxValue;
        });
        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Home/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }


        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();
        app.UseStaticFiles();
        app.UseAuthorization();
        app.UseSession();

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Account}/{action=Login}/{fid?}");



        app.Run();
    }


    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();

        }
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllerRoute(
                name: "defalut",
                pattern: "{controller=Report}/(action=Print}/{id?})");
        });

    }
    public void ConfigureServices(IServiceCollection service)
    {
        service.AddMvc();
        service.AddOptions();
       
         
    }
    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
        .ConfigureWebHostDefaults(webBuilder =>
        {
            webBuilder.UseContentRoot(Directory.GetCurrentDirectory());
            webBuilder.UseWebRoot("wwwroot");
            webBuilder.UseStartup<IStartup>();

        });

    [Obsolete]
    public void Configure(IApplicationBuilder app, Microsoft.AspNetCore.Hosting.IHostingEnvironment env)
    {
        // Other configurations...

        app.UseStaticFiles(); // Enable static file serving, e.g., for wwwroot folder

        // More configurations...
    }

}
