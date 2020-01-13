using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WalkingTec.Mvvm.Core;
using WalkingTec.Mvvm.Core.Extensions;
using WalkingTec.Mvvm.Mvc;

namespace WalkingTec.Mvvm.Doc.Controllers
{
    [AllowAnonymous]
    public class HomeController : BaseController
    {
        public async Task<ActionResult> Index()
        {
            ViewData["title"] = "WTM";
            var user = new LoginUserInfo { ITCode = "admin", Id = Guid.Parse("F75317BB-2C36-451F-ADA7-29DD42DAA492") };
            LoginUserInfo = user;
            AuthenticationProperties properties = null;
                properties = new AuthenticationProperties
                {
                    IsPersistent = true,
                    ExpiresUtc = DateTimeOffset.UtcNow.Add(TimeSpan.FromDays(30))
                };

            var principal = user.CreatePrincipal();
            // 在上面注册AddAuthentication时，指定了默认的Scheme，在这里便可以不再指定Scheme。
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, properties);
            return View();
        }

        public IActionResult PIndex()
        {
            return View();
        }

        public IActionResult FrontPage()
        {
            return Redirect("/QuickStart/Intro");
        }

        [ActionDescription("捐赠名单")]
        public IActionResult DonateList()
        {
            return PartialView();
        }

        [ActionDescription("Layout")]
        public IActionResult Layout()
        {
            return PartialView();
        }
    }
}
