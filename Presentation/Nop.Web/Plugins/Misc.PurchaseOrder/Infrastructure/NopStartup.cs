using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nop.Core.Infrastructure;

namespace Nop.Plugin.Misc.PurchaseOrder.Infrastructure;
public class NopStartup : INopStartup
{
    public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<RazorViewEngineOptions>(options =>
        {
            options.ViewLocationExpanders.Add(new ViewLocationExpander());
        });
        //services.AddScoped<ISupplierServices, SupplierService>();
        //services.AddScoped<IProductSupplierMappingService, ProductSupplierMappingService>();
        //services.AddScoped<ISupplierModelFactory, SupplierModelFactory>();
    }
    public void Configure(IApplicationBuilder application)
    {
    }
    public int Order => 3000;
}
