using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using WalkingTec.Mvvm.Core;
using WalkingTec.Mvvm.Core.Extensions;
using WalkingTec.Mvvm.Mvc;
using WalkingTec.Mvvm.Mvc.Admin.ViewModels.FrameworkRoleVMs;

namespace WalkingTec.Mvvm.Admin.Api
{

    [ActionDescription("RoleManagement")]
    [ApiController]
    [Route("api/_FrameworkRole")]
	public class _FrameworkRoleController : BaseApiController
    {
        [ActionDescription("Search")]
        [HttpPost("Search")]
		public string Search(FrameworkRoleSearcher searcher)
        {
            var vm = CreateVM<FrameworkRoleListVM>();
            vm.Searcher = searcher;
            return vm.GetJson();
        }

        [ActionDescription("Get")]
        [HttpGet("{id}")]
        public FrameworkRoleVM Get(Guid id)
        {
            var vm = CreateVM<FrameworkRoleVM>(id);
            return vm;
        }

        [ActionDescription("GetPageActions")]
        [HttpGet("GetPageActions/{id}")]
        public FrameworkRoleMDVM2 GetPageActions(Guid id)
        {
            var vm = CreateVM<FrameworkRoleMDVM2>(id);
            return vm;
        }


        [ActionDescription("PageFunction")]
        [HttpPut("EditPrivilege")]
        public IActionResult EditPrivilege(FrameworkRoleMDVM2 vm)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.GetErrorJson());
            }
            else
            {
                vm.DoChange();
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState.GetErrorJson());
                }
                else
                {
                    return Ok(vm.Entity);
                }
            }
        }

        [ActionDescription("Create")]
        [HttpPost("Add")]
        public IActionResult Add(FrameworkRoleVM vm)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.GetErrorJson());
            }
            else
            {
                vm.DoAdd();
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState.GetErrorJson());
                }
                else
                {
                    return Ok(vm.Entity);
                }
            }

        }

        [ActionDescription("Edit")]
        [HttpPut("Edit")]
        public IActionResult Edit(FrameworkRoleVM vm)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.GetErrorJson());
            }
            else
            {
                vm.DoEdit(false);
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState.GetErrorJson());
                }
                else
                {
                    return Ok(vm.Entity);
                }
            }
        }

		[HttpPost("BatchDelete")]
        [ActionDescription("Delete")]
        public IActionResult BatchDelete(string[] ids)
        {
            var vm = CreateVM<FrameworkRoleBatchVM>();
            if (ids != null && ids.Count() > 0)
            {
                vm.Ids = ids;
            }
            else
            {
                return Ok();
            }
            if (!ModelState.IsValid || !vm.DoBatchDelete())
            {
                return BadRequest(ModelState.GetErrorJson());
            }
            else
            {
                return Ok(ids.Count());
            }
        }


        [ActionDescription("Export")]
        [HttpPost("ExportExcel")]
        public IActionResult ExportExcel(FrameworkRoleSearcher searcher)
        {
            var vm = CreateVM<FrameworkRoleListVM>();
            vm.Searcher = searcher;
            vm.SearcherMode = ListVMSearchModeEnum.Export;
            var data = vm.GenerateExcel();
            return File(data, "application/vnd.ms-excel", $"Export_FrameworkRole_{DateTime.Now.ToString("yyyy-MM-dd")}.xls");
        }

        [ActionDescription("ExportByIds")]
        [HttpPost("ExportExcelByIds")]
        public IActionResult ExportExcelByIds(string[] ids)
        {
            var vm = CreateVM<FrameworkRoleListVM>();
            if (ids != null && ids.Count() > 0)
            {
                vm.Ids = new List<string>(ids);
                vm.SearcherMode = ListVMSearchModeEnum.CheckExport;
            }
            var data = vm.GenerateExcel();
            return File(data, "application/vnd.ms-excel", $"Export_FrameworkRole_{DateTime.Now.ToString("yyyy-MM-dd")}.xls");
        }

        [ActionDescription("DownloadTemplate")]
        [HttpGet("GetExcelTemplate")]
        public IActionResult GetExcelTemplate()
        {
            var vm = CreateVM<FrameworkRoleImportVM>();
            var qs = new Dictionary<string, string>();
            foreach (var item in Request.Query.Keys)
            {
                qs.Add(item, Request.Query[item]);
            }
            vm.SetParms(qs);
            var data = vm.GenerateTemplate(out string fileName);
            return File(data, "application/vnd.ms-excel", fileName);
        }

        [ActionDescription("Import")]
        [HttpPost("Import")]
        public ActionResult Import(FrameworkRoleImportVM vm)
        {

            if (vm.ErrorListVM.EntityList.Count > 0 || !vm.BatchSaveData())
            {
                return BadRequest(vm.GetErrorJson());
            }
            else
            {
                return Ok(vm.EntityList.Count);
            }
        }


    }
}
