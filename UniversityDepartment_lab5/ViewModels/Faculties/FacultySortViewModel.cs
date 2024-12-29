namespace UniversityDepartment_lab5.ViewModels.Faculties
{
    public enum SortState
    {
        No,
        NameAsc,
        NameDesc
    }
    public class FacultySortViewModel
    {
        public SortState NameSort { get; set; }
        public SortState CurrentState { get; set; }

        public FacultySortViewModel(SortState sortOrder)
        {
            NameSort = sortOrder == SortState.NameAsc ? SortState.NameDesc : SortState.NameAsc;
            CurrentState = sortOrder;
        }   
    }
}
