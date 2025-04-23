using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nop.Core.Infrastructure;
using Nop.Plugin.GadgetTheme.SupplierManagement.Areas.Admin.Factories;
using Nop.Plugin.GadgetTheme.SupplierManagement.Factories;
using Nop.Plugin.GadgetTheme.SupplierManagement.Services;

namespace Nop.Plugin.GadgetTheme.SupplierManagement.Infrastructure;
public class NopStartup : INopStartup
{
    public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<RazorViewEngineOptions>(options =>
        {
            options.ViewLocationExpanders.Add(new ViewLocationExpander());
        });
        services.AddScoped<ISupplierServices, SupplierService>();
        services.AddScoped<IProductSupplierMappingService, ProductSupplierMappingService>();
        services.AddScoped<ISupplierModelFactory, SupplierModelFactory>();
    }
    public void Configure(IApplicationBuilder application)
    {
    }
    public int Order => 3000;
}