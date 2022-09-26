using System;
using System.Collections.Generic;

namespace AmsApp.Models
{
    public partial class VwVisit
    {
        public int Id { get; set; }
        public string Mobile { get; set; } = null!;
        public string? Name { get; set; }
        public int? AgentId { get; set; }
        public string? PatientId { get; set; }
        public DateTime? AppointmentDate { get; set; }
        public int? Age { get; set; }
        public int? Gender { get; set; }
        public int? MainDisease { get; set; }
        public int? SubDisease { get; set; }
        public int? ClinicBranch { get; set; }
        public string AppType { get; set; } = null!;
        public int? TLId { get; set; }
    }
}
