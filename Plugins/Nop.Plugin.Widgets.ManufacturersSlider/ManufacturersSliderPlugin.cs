using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nop.Plugin.Widgets.ManufacturersSlider.Components;
using Nop.Services.Cms;
using Nop.Services.Plugins;
using Nop.Web.Framework.Infrastructure;


namespace Nop.Plugin.Widgets.ManufacturersSlider;
public class ManufacturersSliderPlugin : BasePlugin, IWidgetPlugin
{
    public bool HideInWidgetList => false;

    //public string SystemName => "Widgets.ManufacturersSlider"; // Removed 'override' as SystemName is not virtual in BasePlugin

    public Task<IList<string>> GetWidgetZonesAsync()
    {
        return Task.FromResult<IList<string>>(new List<string> { "home_page_before_news" });
    }

    public Type GetWidgetViewComponent(string widgetZone)
    {
        return typeof(ManufacturersSliderViewComponent);
    }
}



