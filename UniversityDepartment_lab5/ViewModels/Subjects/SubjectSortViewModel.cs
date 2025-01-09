namespace UniversityDepartment_lab5.ViewModels.Subjects
{
    public enum SortState
    {
        No,
        NameAsc,
        NameDesc,
        ReportingTypeAsc,
        ReportingTypeDesc,
        LectureHoursAsc,
        LectureHoursDesc,
        PracticalHoursAsc,
        PracticalHoursDesc,
        LabHoursAsc,
        LabHoursDesc
    }
    public class SubjectSortViewModel
    {
        public SortState NameSort { get; set; }
        public SortState ReportingTypeSort { get; set; }
        public SortState LectureHoursSort { get; set; }
        public SortState PracticalHoursSort { get; set; }
        public SortState LabHoursSort { get; set; }
        public SortState CurrentState { get; set; }

        public SubjectSortViewModel(SortState sortOrder)
        {
            NameSort = sortOrder == SortState.NameAsc ? SortState.NameDesc : SortState.NameAsc;
            ReportingTypeSort = sortOrder == SortState.ReportingTypeAsc ? SortState.ReportingTypeDesc : SortState.ReportingTypeAsc;
            LectureHoursSort = sortOrder == SortState.LectureHoursAsc ? SortState.LectureHoursDesc : SortState.LectureHoursAsc;
            PracticalHoursSort = sortOrder == SortState.PracticalHoursAsc ? SortState.PracticalHoursDesc : SortState.PracticalHoursAsc;
            LabHoursSort = sortOrder == SortState.LabHoursAsc ? SortState.LabHoursDesc : SortState.LabHoursAsc;
            CurrentState = sortOrder;
        }
        
    }
}
