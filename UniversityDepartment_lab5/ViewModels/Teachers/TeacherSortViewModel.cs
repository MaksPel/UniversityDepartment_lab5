namespace UniversityDepartment_lab5.ViewModels.Teachers
{
    public enum SortState
    {
        No,
        NameAsc,
        NameDesc,
        SurnameAsc,
        SurnameDesc,
        MidnameAsc,
        MidnameDesc,
        PositionAsc,
        PositionDesc,
        AgeAsc,
        AgeDesc
    }
    public class TeacherSortViewModel
    {
        public SortState NameSort { get; set; }
        public SortState SurnameSort { get; set; }
        public SortState MidnameSort { get; set; }
        public SortState PositionSort { get; set; }
        public SortState AgeSort { get; set; }
        public SortState CurrentState { get; set; }

        public TeacherSortViewModel(SortState sortOrder)
        {
            NameSort = sortOrder == SortState.NameAsc ? SortState.NameDesc : SortState.NameAsc;
            SurnameSort = sortOrder == SortState.SurnameAsc ? SortState.SurnameDesc : SortState.SurnameAsc;
            MidnameSort = sortOrder == SortState.MidnameAsc ? SortState.MidnameDesc : SortState.MidnameAsc;
            PositionSort = sortOrder == SortState.PositionAsc ? SortState.PositionDesc : SortState.PositionAsc;
            AgeSort = sortOrder == SortState.AgeAsc ? SortState.AgeDesc : SortState.AgeAsc;
            CurrentState = sortOrder;
        }
        
    }
}
