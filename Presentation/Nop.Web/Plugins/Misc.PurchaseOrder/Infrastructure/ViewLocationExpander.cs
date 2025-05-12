using Microsoft.AspNetCore.Mvc.Razor;
using Nop.Core.Infrastructure;
using Nop.Web.Framework.Themes;
namespace Nop.Plugin.GadgetTheme.SupplierManagement.Infrastructure;
public class ViewLocationExpander : IViewLocationExpander
{
    protected const string THEME_KEY = "nop.themename";
    public void PopulateValues(ViewLocationExpanderContext context)
    {
        context.Values[THEME_KEY] = EngineContext.Current.Resolve<IThemeContext>().GetWorkingThemeNameAsync().Result;
    }    
    public IEnumerable<string> ExpandViewLocations(ViewLocationExpanderContext context, IEnumerable<string> viewLocations)
    {
        if (context.AreaName == "admin")
        {
            viewLocations = new string[]
            {
                $"/Plugins/GadgetTheme.SupplierManagement/Areas/Admin/Views/{{0}}.cshtml",
                $"/Plugins/GadgetTheme.SupplierManagement/Areas/Admin/Views/{{1}}/{{0}}.cshtml",
                $"/Plugins/GadgetTheme.SupplierManagement/Areas/Admin/Views/Supplier/{{0}}.cshtml"
            }.Concat(viewLocations);
        }
        else
        {
            viewLocations = new string[]
            {
                $"/Plugins/GadgetTheme.SupplierManagement/Views/{{0}}.cshtml",
                $"/Plugins/GadgetTheme.SupplierManagement/Views/{{1}}/{{0}}.cshtml"
            }.Concat(viewLocations);
        }
        return viewLocations;
    }
}