using EntityFramework.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EntityFramework.ViewComponents
{
    public class FooterViewComponent : ViewComponent
    {
        private readonly LayoutService _layoutSettings;
        public FooterViewComponent(LayoutService layoutSettings)
        {
            _layoutSettings = layoutSettings;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            Dictionary<string, string> settings = _layoutSettings.GetSettings();

            return await Task.FromResult(View(settings));
        }
    }
}
