using UniversityDepartment_lab5.Models;

namespace UniversityDepartment_lab5.ViewModels.Teachers
{
    public class TeachersViewModel
    {
        public IEnumerable<Teacher> Teachers { get; set; } = new List<Teacher>();
        public string? Name { get; set; }
        public string? Surname { get; set; }
        public string? Midname { get; set; }
        public string? Position { get; set; }
        public int Age { get; set; }
        public PageViewModel PageViewModel { get; set; } = null!;
        public TeacherSortViewModel SortViewModel { get; set; } = null!;
    }
}
