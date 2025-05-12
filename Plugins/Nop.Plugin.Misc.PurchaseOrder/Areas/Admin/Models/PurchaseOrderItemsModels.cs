using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nop.Web.Framework.Models;

namespace Nop.Plugin.Misc.PurchaseOrder.Areas.Admin.Models;
public record PurchaseOrderItemsModel: BaseNopModel
{
        public string PictureUrl { get; set; }
        public string ProductName { get; set; }
        public decimal Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public string UnitPriceFormatted { get; set; }
        public decimal TotalCost => UnitPrice * Quantity;
        public string TotalCostFormatted { get; set; }
}
