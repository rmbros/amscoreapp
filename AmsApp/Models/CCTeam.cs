using System;
using System.Collections.Generic;

namespace AmsApp.Models
{
    public partial class CCTeam
    {
        public int Id { get; set; }
        public int CCTL { get; set; }
        public int CCE { get; set; }
        public string? Status { get; set; } = null!;
    }
}
