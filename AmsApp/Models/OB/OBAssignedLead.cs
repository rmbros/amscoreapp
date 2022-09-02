using System;
using System.Collections.Generic;

namespace AmsApp.Models
{
    public partial class OBAssignedLead
    {
        public int Id { get; set; }
        public int LeadId { get; set; }
        public string Mobile { get; set; } = null!;
        public string? Name { get; set; }
        public int? Age { get; set; }
        public int? Gender { get; set; }
        public int? MainDisease { get; set; }
        public int? SubDisease { get; set; }
        public int? ClinicBranch { get; set; }
        public DateTime? AppointmentDate { get; set; }
        public string? Address { get; set; }
        public int? AgentId { get; set; }
        public int? TeamLeadId { get; set; }
        public DateTime? AllocatedDateTime { get; set; }
        public int? Disposition { get; set; }
        public DateTime? NextCallDate { get; set; }
        public byte Status { get; set; }
        public DateTime CreatedOn { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public int? ModifiedBy { get; set; }
    }
}
