using Nop.Services.Catalog;
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
            var extraDescription = await _productDescriptionService.GetExtraDescriptionByProductIdAsync(model.Breadcrumb.ProductId);

            if (!string.IsNullOrEmpty(extraDescription))
                model.CustomProperties["ExtraDescription"] = extraDescription;
        }
    }
}
