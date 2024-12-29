using UniversityDepartment_lab5.Models;

namespace UniversityDepartment_lab5.ViewModels.Faculties
{
    public class FacultiesViewModel
    {
        public IEnumerable<Faculty> Faculties { get; set; } = new List<Faculty>();
        public string? Name { get; set; }
        public PageViewModel PageViewModel { get; set; } = null!;
        public FacultySortViewModel SortViewModel { get; set; } = null!;
    }
}
