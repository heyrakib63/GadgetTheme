using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nop.Core.Infrastructure;
using Nop.Plugin.Misc.ProductDescriptionExt.Domain;
using Nop.Plugin.Misc.ProductDescriptionExt.Services;

namespace Nop.Plugin.GadgetTheme.SupplierManagement.Infrastructure;
public class NopStartup : INopStartup
{
    public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        //services.Configure<RazorViewEngineOptions>(options =>
        //{
        //    options.ViewLocationExpanders.Add(new ViewLocationExpander());
        //});
        services.AddScoped<IProductDescriptionService, ProductDescriptionService>();
        //services.AddScoped<IProductSupplierMappingService, ProductSupplierMappingService>();
        //services.AddScoped<ISupplierModelFactory, SupplierModelFactory>();
    }
    public void Configure(IApplicationBuilder application)
    {
    }
    public int Order => 3000;
}