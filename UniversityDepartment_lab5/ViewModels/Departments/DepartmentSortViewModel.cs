namespace UniversityDepartment_lab5.ViewModels.Departments
{
    public enum SortState
    {
        No,
        NameAsc,
        NameDesc,
        FacultyAsc,
        FacultyDesc
    }
    public class DepartmentSortViewModel
    {
        public SortState NameSort { get; set; }
        public SortState FacultySort { get; set; }
        public SortState CurrentState { get; set; }

        public DepartmentSortViewModel(SortState sortOrder)
        {
            NameSort = sortOrder == SortState.NameAsc ? SortState.NameDesc : SortState.NameAsc;
            FacultySort = sortOrder == SortState.FacultyAsc ? SortState.FacultyDesc : SortState.FacultyAsc;
            CurrentState = sortOrder;
        }
        
    }
}
