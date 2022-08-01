using System;
using System.Collections.Generic;

namespace AmsApp.Models
{
    public partial class User
    {
        public int Id { get; set; }
        public string UserName { get; set; } = null!;
        public string Password { get; set; } = null!;
        public DateTime? PasswordModifiedOn { get; set; }
        public string? Password2 { get; set; }
        public DateTime? Password2ModifiedOn { get; set; }
        public string AdminPassword { get; set; } = null!;
        public int EmployeeId { get; set; }
        public int Role { get; set; }
        public DateTime? LastLoggedIn { get; set; }
        public string? LoggedInIp { get; set; }
        public byte Status { get; set; }
        public DateTime CreatedOn { get; set; }
        public int CreatedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
        public int ModifiedBy { get; set; }
        public int? IsLoggedin { get; set; }
    }
}
