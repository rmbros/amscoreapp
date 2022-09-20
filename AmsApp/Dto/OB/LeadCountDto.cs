
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
        public int Interested { get; set; }
        public int Callback { get; set; }
        public double Speed
        {
            get => TotalCalls == 0 ? 0 : Math.Round(TotalTime / (TotalCalls * 1.0), 2);
        }
        public int TimeWasted
        {
            get {
                DateTime startTime = DateTime.Now.Date.AddHours(9).AddMinutes(30);                
                DateTime dayend = DateTime.Now.Date.AddHours(18).AddMinutes(30);
                DateTime endTime = dayend.Subtract(DateTime.Now).TotalSeconds < 0 ? dayend : DateTime.Now;
                TimeSpan span = endTime.Subtract(startTime);
                if (endTime.Hour >= 14)
                {
                    return Convert.ToInt32(span.TotalMinutes) - TotalTime - 30; //30min lunch break
                }
                else
                {
                    return Convert.ToInt32(span.TotalMinutes) - TotalTime;
                }
            }
        }
    }

    public class OBVisitListDto
    {
        public int Id { get; set; }
        public string Mobile { get; set; } = null!;
        public string? Name { get; set; }
        public string? Agent { get; set; }
        public string? Status { get; set; }
        public string? AppointmentDate { get; set; }
        public string? Actions { get; set; }
    }
}