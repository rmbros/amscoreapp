
namespace AmsApp.Dto
{
    public class LeadCountDto
    {
        public int TotalLeads { get; set; }
        public int OnHoldCount { get; set; }
        public int NextCallDateCount { get; set; }
        public int NewLeads { get; set; }
        public int TotalCalls { get; set; }
        public int TotalTime { get; set; }
        public decimal Speed { get; set; }
    }
}