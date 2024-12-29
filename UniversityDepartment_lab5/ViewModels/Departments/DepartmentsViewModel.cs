using UniversityDepartment_lab5.Models;

namespace UniversityDepartment_lab5.ViewModels.Departments
{
    public class DepartmentViewModel
    {
        public IEnumerable<Department> Departments { get; set; } = new List<Department>();
        public string? Name { get; set; }
        public string? Faculty { get; set; }
        public bool? IsGraduating { get; set; }
        public PageViewModel PageViewModel { get; set; } = null!;
        public DepartmentSortViewModel SortViewModel { get; set; } = null!;
    }
}
