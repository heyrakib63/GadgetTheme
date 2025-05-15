using Nop.Services.Events;
using Nop.Web.Framework.Events;
using Nop.Web.Framework.Models;
using Nop.Web.Models.Catalog;

namespace Nop.Plugin.Widget.ProductDescriptionExt.Services.EventConsumers;
public class ModelEventCunsumer : IConsumer<ModelPreparedEvent<BaseNopModel>>
{
    private readonly IProductDescriptionService _productDescriptionService;
    public ModelEventCunsumer(
        IProductDescriptionService productDescriptionService
        )
    {
        _productDescriptionService = productDescriptionService;
    }

    public async Task HandleEventAsync(ModelPreparedEvent<BaseNopModel> eventMessage)
    {
        var modelType = eventMessage.Model;
        if(modelType is not null && modelType is ProductDetailsModel productDetailsModel)
        {
            var model = productDetailsModel;
            var productDescription = await _productDescriptionService.GetProductDescriptionByProductIdAsync(model.Breadcrumb.ProductId);

            if (productDescription!=null)
                model.CustomProperties[ProductDescriptionExtDefaults.ExtraDescription] = productDescription.Description;
        }
    }
}
