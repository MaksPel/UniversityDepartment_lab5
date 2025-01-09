using UniversityDepartment_lab5.Models;

namespace UniversityDepartment_lab5.ViewModels.Subjects
{
    public class SubjectsViewModel
    {
        public IEnumerable<Subject> Subjects { get; set; } = new List<Subject>();
        public string? Name { get; set; }
        public string? ReportingType { get; set; }
        public int LectureHours { get; set; }
        public int PracticalHours { get; set; }
        public int LabHours { get; set; }
        public PageViewModel PageViewModel { get; set; } = null!;
        public SubjectSortViewModel SortViewModel { get; set; } = null!;
    }
}
