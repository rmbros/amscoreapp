using System;
using System.Collections.Generic;

namespace AmsApp.Models
{
    public partial class AppAppointment
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public int? Age { get; set; }
        public int? Gender { get; set; }
        public int? MainDisease { get; set; }
        public int? SubDisease { get; set; }
        public int? ClinicBranch { get; set; }
        public DateTime? AppointmentDate { get; set; }
        public string Mobile { get; set; } = null!;
        public string? Location { get; set; }
        public int? AgentId { get; set; }
        public string? PatientId { get; set; }
        public byte Status { get; set; }
        public DateTime CreatedOn { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public int? ModifiedBy { get; set; }
    }
}
