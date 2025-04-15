using Nop.Core;
using Nop.Core.Infrastructure;
using Nop.Services.Plugins;
using System.Threading.Tasks;

namespace Nop.Plugin.Misc.SupplierManagement
{
    public class SupplierManagementPlugin : BasePlugin
    {
        private readonly IWebHelper _webHelper;

        public SupplierManagementPlugin(IWebHelper webHelper)
        {
            _webHelper = webHelper;
        }

        public override string GetConfigurationPageUrl()
        {
            return $"{_webHelper.GetStoreLocation()}Admin/Supplier/List";
        }

        public override Task InstallAsync()
        {
            // We'll add installation logic here later
            return base.InstallAsync();
        }

        public override Task UninstallAsync()
        {
            // We'll add uninstallation logic here later
            return base.UninstallAsync();
        }
    }
}