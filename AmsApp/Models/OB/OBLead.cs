using System.ComponentModel.DataAnnotations.Schema;

namespace AmsApp.Models
{
    public class OBLead
    {
        public int Id { get; set; }
        public string Mobile { get; set; } = null!;
        public string? Name { get; set; }
        public int? Age { get; set; }
        public int? Gender { get; set; }
        public string? Email { get; set; }
        public string? Address { get; set; }
        public string? City { get; set; }
        public string? Dist { get; set; }
        public string? State { get; set; }
        public string? Country { get; set; }
        public int? Pin { get; set; }
        public string? AltMobile { get; set; }
        public int? MainDisease { get; set; }
        public int? SubDisease { get; set; }
        public string? Notes { get; set; }
        public int? ClinicBranch { get; set; }
        public int? AllocatedAgentId { get; set; }
        public DateTime? AllocatedDateTime { get; set; }
        public bool OnHold { get; set; }
        public DateTime? LastCallOn { get; set; }
        public int? LastCalledBy { get; set; }
        public int? Disposition { get; set; }
        public DateTime? NextCallDate { get; set; }
        public int? PatientId { get; set; }
        public int? OBLeadUploadHistoryId { get; set; }
        public int? Source { get; set; }
        public int? SubSource { get; set; }
        public byte Status { get; set; }
        public DateTime CreatedOn { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public int? ModifiedBy { get; set; }
        public string MaskedMobile
        {
            get => string.IsNullOrEmpty(Mobile)? string.Empty : $"xxxxxx{Mobile.Substring(6,4)}";
        }

        [NotMapped]
        public DateTime StartTime { get; set; } = DateTime.Now;
        [NotMapped]
        public DateTime EndTime { get; set; }
        [NotMapped]
        public string NextDate { get; set; }
        [NotMapped]
        public string NextTime { get; set; }
    }
}
