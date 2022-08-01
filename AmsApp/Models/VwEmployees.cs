namespace AmsApp.Models
{
    public partial class VwEmployees
    {
        public int EmployeeId { get; set; }
        public string Name { get; set; }
        public string? Surname { get; set; }
        public string? EmployeeCode { get; set; }
        public string? BioMetricId { get; set; }
        public string? EmpNameWithId { get; set; }
        public DateTime? Dob { get; set; }
        public int? Gender { get; set; }
        public string? Mobile1 { get; set; }
        public string? Mobile2 { get; set; }
        public string? WhatsApp { get; set; }
        public byte EmpStatus { get; set; }
        public string? PhotoFile { get; set; }
        public DateTime? Doj { get; set; }
        public int? Designation { get; set; }
        public string? DesignationTitle { get; set; }
        public int? DesgOrder { get; set; }
        public int? Branch { get; set; }
        public string? BranchName { get; set; }
        public int? Department { get; set; }
        public string? DepartmentName { get; set; }
        public int? SubDepartment { get; set; }
        public string? SubDepartmentName { get; set; }
        public string? FromTime { get; set; }
        public string? ToTime { get; set; }
        public int? UserId { get; set; }
        public string? UserName { get; set; }
    }
}
