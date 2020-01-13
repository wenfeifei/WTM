using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WalkingTec.Mvvm.Core;
using WalkingTec.Mvvm.Core.Extensions;
using WalkingTec.Mvvm.Demo.Models;
using WalkingTec.Mvvm.Demo.ViewModels.MajorVMs;

namespace WalkingTec.Mvvm.Demo.ViewModels.StudentVMs
{
    public class StudentVM : BaseCRUDVM<Student>
    {
        public MajorListVM MajorList { get; set; }
        public List<Guid> SelectedMajorIds { get; set; }

        public StudentVM()
        {
            MajorList = new MajorListVM();
            MajorList.CopyContext(this);
            SetInclude(x => x.StudentMajor);
        }



        protected override void InitVM()
        {
            SelectedMajorIds = Entity.StudentMajor.Select(x => x.MajorId).ToList();
        }

        public override void DoAdd()
        {
            Entity.StudentMajor = new List<StudentMajor>();
            if (SelectedMajorIds != null)
            {
                foreach (var majorid in SelectedMajorIds)
                {
                    Entity.StudentMajor.Add(new StudentMajor { MajorId = majorid });
                }
            }
            base.DoAdd();
        }

        public override void DoEdit(bool updateAllFields = false)
        {
            Entity.StudentMajor = new List<StudentMajor>();
            SelectedMajorIds?.ForEach(x => Entity.StudentMajor.Add(new StudentMajor { MajorId = x }));
            base.DoEdit(updateAllFields);
        }

    }
}
