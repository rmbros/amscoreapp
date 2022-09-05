
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
        public int Appointments { get; set; }
        public int Visited { get; set; }
        public double Speed
        {
            get => TotalCalls == 0 ? 0 : Math.Round(TotalTime / (TotalCalls * 1.0), 2);
        }
    }
}