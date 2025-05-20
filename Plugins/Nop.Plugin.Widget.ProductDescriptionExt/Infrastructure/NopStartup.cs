using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nop.Core.Infrastructure;
using Nop.Plugin.Widget.ProductDescriptionExt.Services;

namespace Nop.Plugin.Widget.ProductDescriptionExt.Infrastructure;
public class NopStartup : INopStartup
{
    public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<RazorViewEngineOptions>(options =>
        {
            options.ViewLocationExpanders.Add(new ViewLocationExpander());
        });
        services.AddScoped<IProductDescriptionService, ProductDescriptionService>();
    }
    public void Configure(IApplicationBuilder application)
    {
    }
    public int Order => 3050;
}