using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Nop.Plugin.Misc.PurchaseOrder.Areas.Admin.Models;
public class CreatePurchaseOrderModel
{
    public CreatePurchaseOrderModel()
    {
        SupplierProductSearchModel = new SupplierProductSearchModel();
    }
    [Display(Name = "Select a Supplier")]
    public int SupplierId { get; set; }
    public IList<SelectListItem> AvailableSuppliers { get; set; } = new List<SelectListItem>();

    public DateTime OrderDate { get; set; }
    public Guid PurchaseOrderNo { get; set; }
    public IList<PurchaseOrderItemModel> SelectedProducts { get; set; } = new List<PurchaseOrderItemModel>();
    public decimal TotalCost { get; set; }
    public string OrderTotalFormatted { get; set; }
    public SupplierProductSearchModel SupplierProductSearchModel { get; set; }
}
public class PurchaseOrderItemModel
{
    public string PurchaseOrderNo { get; set; }
    public int ProductId { get; set; }
    public string ProductName { get; set; }
    public decimal UnitPrice { get; set; }
    public int Quantity { get; set; }
    public decimal TotalCost => UnitPrice * Quantity;
}
