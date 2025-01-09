using UniversityDepartment_lab5.Models;

namespace UniversityDepartment_lab5.ViewModels.Specialties
{
    public class SpecialtiesViewModel
    {
        public IEnumerable<Specialty> Specialties { get; set; } = new List<Specialty>();
        public string? Name { get; set; }
        public string? Department { get; set; }
        public PageViewModel PageViewModel { get; set; } = null!;
        public SpecialtySortViewModel SortViewModel { get; set; } = null!;
    }
}
