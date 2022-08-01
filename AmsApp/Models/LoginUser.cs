using System;
using System.Collections.Generic;

namespace AmsApp.Models
{
    public partial class LoginUser
    {
        public int Id { get; set; }
        public string UserName { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string? Password2 { get; set; }
        public int EmployeeId { get; set; }
        public int Role { get; set; }
        public string? LoggedInIp { get; set; }
    }
}
