using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

using WalkingTec.Mvvm.Core;
using WalkingTec.Mvvm.Core.Auth;
using WalkingTec.Mvvm.Demo.ViewModels.HomeVMs;
using WalkingTec.Mvvm.Mvc;

namespace WalkingTec.Mvvm.Demo.Controllers
{
    public class HomeController : BaseController
    {
        [AllRights]
        public IActionResult Index()
        {
            ViewData["title"] = "WTM";
            return View();
        }

        [AllowAnonymous]
        public IActionResult PIndex()
        {
            return View();
        }

        [AllRights]
        [ActionDescription("首页")]
        public IActionResult FrontPage()
        {
            var areas = GlobaInfo.AllModule.Select(x => x.Area).Distinct();
            var legend = new List<string>();
            var series = new List<object>();
            foreach (var area in areas)
            {
                var legendName = area?.AreaName ?? "默认";
                var controllers = GlobaInfo.AllModule.Where(x => x.Area == area);
                legend.Add(legendName);
                series.Add(new
                {
                    name = legendName,
                    type = "bar",
                    data = new int[] {
                        controllers.Count(),
                        controllers.SelectMany(x => x.Actions).Count()
                    },
                });
            }

            var otherLegend = new List<string>() { "相关信息" };
            var otherSeries = new List<object>()
            {
                new {
                    name = "相关信息",
                    type = "bar",
                    data = new int[] {
                        GlobaInfo.AllModels.Count(),
                        GlobaInfo.AllAssembly.Count(),
                        ConfigInfo.DataPrivilegeSettings.Count(),
                        ConfigInfo.ConnectionStrings.Count(),
                        ConfigInfo.AppSettings.Count()
                    },
                }
            };

            ViewData["controller.legend"] = legend;
            ViewData["controller.series"] = series;
            ViewData["other.legend"] = otherLegend;
            ViewData["other.series"] = otherSeries;

            return PartialView();
        }

        [AllRights]
        [ActionDescription("Layout")]
        public IActionResult Layout()
        {
            ViewData["debug"] = ConfigInfo.IsQuickDebug;
            return PartialView();
        }

        [AllRights]
        public IActionResult UserInfo()
        {
            if (HttpContext.Request.Cookies.TryGetValue(CookieAuthenticationDefaults.CookiePrefix + AuthConstants.CookieAuthName, out string cookieValue))
            {
                var protectedData = Base64UrlTextEncoder.Decode(cookieValue);
                var dataProtectionProvider = HttpContext.RequestServices.GetRequiredService<IDataProtectionProvider>();
                var _dataProtector = dataProtectionProvider
                                        .CreateProtector(
                                            "Microsoft.AspNetCore.Authentication.Cookies.CookieAuthenticationMiddleware",
                                            CookieAuthenticationDefaults.AuthenticationScheme,
                                            "v2");
                var unprotectedData = _dataProtector.Unprotect(protectedData);

                string cookieData = Encoding.UTF8.GetString(unprotectedData);
                return Json(cookieData);
            }
            else
                return Json("无数据");
        }

    }
}
