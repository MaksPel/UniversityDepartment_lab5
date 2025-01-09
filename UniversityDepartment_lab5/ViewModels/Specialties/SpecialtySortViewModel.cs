namespace UniversityDepartment_lab5.ViewModels.Specialties
{
    public enum SortState
    {
        No,
        NameAsc,
        NameDesc,
        DepartmentAsc,
        DepartmentDesc
    }
    public class SpecialtySortViewModel
    {
        public SortState NameSort { get; set; }
        public SortState DepartmentSort { get; set; }
        public SortState CurrentState { get; set; }

        public SpecialtySortViewModel(SortState sortOrder)
        {
            NameSort = sortOrder == SortState.NameAsc ? SortState.NameDesc : SortState.NameAsc;
            DepartmentSort = sortOrder == SortState.DepartmentAsc ? SortState.DepartmentDesc : SortState.DepartmentAsc;
            CurrentState = sortOrder;
        }
        
    }
}
