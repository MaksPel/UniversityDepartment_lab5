namespace UniversityDepartment_lab5.ViewModels.Courses
{
    public enum SortState
    {
        No,
        SpecialtyAsc,
        SpecialtyDesc,
        CourseNumberAsc,
        CourseNumberDesc,
        SemesterAsc,
        SemesterDesc,
    }
    public class CoursesSortViewModel
    {
        public SortState CourseNumberSort { get; set; }
        public SortState SemesterSort { get; set; }
        public SortState SpecialtySort { get; set; }
        public SortState CurrentState { get; set; }

        public CoursesSortViewModel(SortState sortOrder)
        {
            CourseNumberSort = sortOrder == SortState.CourseNumberAsc ? SortState.CourseNumberDesc : SortState.CourseNumberAsc;
            SemesterSort = sortOrder == SortState.SemesterAsc ? SortState.SemesterDesc : SortState.SemesterAsc;
            SpecialtySort = sortOrder == SortState.SpecialtyAsc ? SortState.SpecialtyDesc : SortState.SpecialtyAsc;
            CurrentState = sortOrder;
        }
    }
}
