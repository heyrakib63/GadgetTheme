using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Nop.Services.Catalog;
using Nop.Services.Media;
using Nop.Web.Framework.Components;

namespace Nop.Plugin.Widgets.ManufacturersSlider.Components;
public class ManufacturersSliderViewComponent: NopViewComponent
{

    private readonly IManufacturerService _manufacturerService;
    private readonly IPictureService _pictureService;

    public ManufacturersSliderViewComponent(IManufacturerService manufacturerService, IPictureService pictureService)
    {
        _manufacturerService = manufacturerService;
        _pictureService = pictureService;
    }

    public async Task<IViewComponentResult> InvokeAsync(string widgetZone, object additionalData)
    {
        var manufacturers = await _manufacturerService.GetAllManufacturersAsync(showHidden: false);
        var images = new List<string>();

        foreach (var m in manufacturers)
        {
            var imageUrl = await _pictureService.GetPictureUrlAsync(m.PictureId);
            images.Add(imageUrl ?? "/images/default.png");
        }

        return View("~/Plugins/Widgets.ManufacturersSlider/Views/ManufacturersSlider.cshtml", images);
    }
}
