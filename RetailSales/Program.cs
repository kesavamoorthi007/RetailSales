using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.AspNetCore.Http.Features;
using RetailSales.Interface;
using RetailSales.Services;
using RetailSales.Interface.Master;
using RetailSales.Services.Master;
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


        // adding interface and services containers
        builder.Services.TryAddSingleton<ICityService, CityServices>();

        builder.Services.TryAddSingleton<IStateService, StateService>();

		builder.Services.TryAddSingleton<ICCategoryService, CCategoryService>();
		builder.Services.TryAddSingleton<ICGroupService, CGroupService>();



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
