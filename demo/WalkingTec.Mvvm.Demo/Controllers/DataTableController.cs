using Microsoft.AspNetCore.Mvc;

using WalkingTec.Mvvm.Core;
using WalkingTec.Mvvm.Core.Extensions;
using WalkingTec.Mvvm.Demo.ViewModels.DataTableVMs;
using WalkingTec.Mvvm.Mvc;

namespace WalkingTec.Mvvm.Demo.Controllers
{
    [ActionDescription("使用存储过程")]
    public class DataTableController : BaseController
    {
        [ActionDescription("搜索")]
        public IActionResult Index()
        {
            var vm = CreateVM<ActionLogListVM>();
            return PartialView(vm);
        }

        [ActionDescription("搜索")]
        [HttpPost]
        public string Search(ActionLogListVM vm)
        {
            return vm.GetJson(false);
        }

    }
}
