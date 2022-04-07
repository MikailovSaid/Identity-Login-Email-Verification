using EntityFramework.Data;
using EntityFramework.Services;
using EntityFramework.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EntityFramework.ViewComponents
{
    public class HeaderViewComponent : ViewComponent
    {
        private readonly LayoutService _layoutSettings;
        public HeaderViewComponent(LayoutService layoutSettings)
        {
            _layoutSettings = layoutSettings;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            int productCount = 0;
            if(Request.Cookies["basket"] != null)
            {
                List<BasketVM> basket = JsonConvert.DeserializeObject<List<BasketVM>>(Request.Cookies["basket"]);
                productCount = basket.Sum(m => m.Count);
            }
            else
            {
                productCount = 0;
            }

            ViewBag.productCount = productCount;

            Dictionary<string, string> settings = _layoutSettings.GetSettings();

            return await Task.FromResult(View(settings));
        }
    }
}
