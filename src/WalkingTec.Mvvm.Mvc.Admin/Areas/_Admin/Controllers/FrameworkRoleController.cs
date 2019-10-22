using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using WalkingTec.Mvvm.Core;
using WalkingTec.Mvvm.Core.Extensions;
using WalkingTec.Mvvm.Mvc.Admin.ViewModels.FrameworkRoleVMs;

namespace WalkingTec.Mvvm.Mvc.Admin.Controllers
{
    [Area("_Admin")]
    [ActionDescription("RoleManagement")]
    public class FrameworkRoleController : BaseController
    {
        #region 查询
        [ActionDescription("Search")]
        public ActionResult Index()
        {
            var vm = CreateVM<FrameworkRoleListVM>();
            return PartialView(vm);
        }
        [ActionDescription("Search")]
        [HttpPost]
        public string Search(FrameworkRoleListVM vm)
        {
            return vm.GetJson(false);
        }
        #endregion

        #region 新建
        [ActionDescription("Create")]
        public ActionResult Create()
        {
            var vm = CreateVM<FrameworkRoleVM>();
            return PartialView(vm);
        }

        [HttpPost]
        [ActionDescription("Create")]
        public ActionResult Create(FrameworkRoleVM vm)
        {
            if (!ModelState.IsValid)
            {
                return PartialView(vm);
            }
            else
            {
                vm.DoAdd();
                return FFResult().CloseDialog().RefreshGrid();
            }
        }
        #endregion

        #region 修改
        [ActionDescription("Edit")]
        public ActionResult Edit(Guid id)
        {
            var vm = CreateVM<FrameworkRoleVM>(id);
            return PartialView(vm);
        }

        [HttpPost]
        [ActionDescription("Edit")]
        public ActionResult Edit(FrameworkRoleVM vm)
        {
            if (!ModelState.IsValid)
            {
                return PartialView(vm);
            }
            else
            {
                vm.DoEdit();
                return FFResult().CloseDialog().RefreshGrid();
            }
        }
        #endregion

        #region 删除
        [ActionDescription("Delete")]
        public ActionResult Delete(Guid id)
        {
            var vm = CreateVM<FrameworkRoleVM>(id);
            return PartialView(vm);
        }

        [HttpPost]
        [ActionDescription("Delete")]
        public ActionResult Delete(Guid id, IFormCollection noUse)
        {
            var vm = CreateVM<FrameworkRoleVM>(id);
            vm.DoDelete();
            if (!ModelState.IsValid)
            {
                return PartialView(vm);
            }
            else
            {
                return FFResult().CloseDialog().RefreshGrid();
            }
        }
        #endregion

        #region 批量删除
        [HttpPost]
        [ActionDescription("BatchDelete")]
        public ActionResult BatchDelete(Guid[] ids)
        {
            var vm = CreateVM<FrameworkRoleBatchVM>(Ids: ids);
            return PartialView(vm);
        }

        [HttpPost]
        [ActionDescription("BatchDelete")]
        public ActionResult DoBatchDelete(FrameworkRoleBatchVM vm, IFormCollection noUse)
        {
            if (!ModelState.IsValid || !vm.DoBatchDelete())
            {
                return PartialView("BatchDelete", vm);
            }
            else
            {
                return FFResult().CloseDialog().RefreshGrid();
            }
        }
        #endregion

        #region 导入
        [ActionDescription("Import")]
        public ActionResult Import()
        {
            var vm = CreateVM<FrameworkRoleImportVM>();
            return PartialView(vm);
        }

        [HttpPost]
        [ActionDescription("Import")]
        public ActionResult Import(FrameworkRoleImportVM vm, IFormCollection nouse)
        {
            if (vm.ErrorListVM.EntityList.Count > 0 || !vm.BatchSaveData())
            {
                return PartialView(vm);
            }
            else
            {
                return FFResult().CloseDialog().RefreshGrid().Alert(Program._localizer["ImportSuccess", vm.EntityList.Count.ToString()]);
            }
        }
        #endregion

        #region 详细
        [ActionDescription("Details")]
        public PartialViewResult Details(Guid id)
        {
            var role = CreateVM<FrameworkRoleMDVM>(id);
            role.ListVM.SearcherMode = ListVMSearchModeEnum.Custom1;
            return PartialView(role);
        }
        #endregion

        #region 页面权限
        [ActionDescription("PageFunction")]
        public PartialViewResult PageFunction(Guid id)
        {
            var role = CreateVM<FrameworkRoleMDVM>(id);
            role.ListVM.SearcherMode = ListVMSearchModeEnum.Custom2;
            return PartialView(role);
        }

        [ActionDescription("PageFunction")]
        [HttpPost]
        public ActionResult PageFunction(FrameworkRoleMDVM vm, IFormCollection noUse)
        {
            vm.DoChange();
            return FFResult().CloseDialog().Alert(Program._localizer["OprationSuccess"]);
        }
        #endregion

        [ActionDescription("Export")]
        [HttpPost]
        public IActionResult ExportExcel(FrameworkRoleListVM vm)
        {
            vm.SearcherMode = vm.Ids != null && vm.Ids.Count > 0 ? ListVMSearchModeEnum.CheckExport : ListVMSearchModeEnum.Export;
            var data = vm.GenerateExcel();
            return File(data, "application/vnd.ms-excel", $"Export_FrameworkRole_{DateTime.Now.ToString("yyyy-MM-dd")}.xls");
        }
    }
}
