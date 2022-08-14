namespace AmsApp.Models
{
    public class OBCallHistory
    {
        public int Id { get; set; }
        public int AgentId { get; set; }
        public int LeadId { get; set; }
        public DateTime? CallDate { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public int? Duration { get; set; }
        public int? Disposition { get; set; }
        public DateTime? NextCallDate { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
