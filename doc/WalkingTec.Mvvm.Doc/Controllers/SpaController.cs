﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WalkingTec.Mvvm.Core;
using WalkingTec.Mvvm.Mvc;

namespace WalkingTec.Mvvm.Doc.Controllers
{
    [AllowAnonymous]
    [ActionDescription("前后端分离（React）")]
    public class SpaController : BaseController
    {
        [ActionDescription("介绍")]
        public IActionResult Intro()
        {
            return PartialView();
        }

        [ActionDescription("全局配置")]
        public IActionResult Global()
        {
            return PartialView();
        }

        [ActionDescription("文件结构")]
        public IActionResult Dir()
        {
            return PartialView();
        }
    }
}
