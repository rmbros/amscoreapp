namespace AmsApp.Dto
{
    public class SearchDto
    {
        public string? Branch { get; set; }
        public string? PatientId { get; set; }
        public string? BioMetricId{ get; set; }
        public string? Mobile { get; set; }
        public string? FromDate { get; set; }
        public string? ToDate { get; set; }
    }

    public class OBReportSearchDto
    {
        public string TeamLead { get; set; } = null!;
        public string Executive { get; set; } = null!;
        public DateTime? FromDate { get; set; } = null!;
        public DateTime? ToDate { get; set; } = null!;
    }
}
