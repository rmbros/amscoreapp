using System;
using System.Collections.Generic;

namespace AmsApp.Models
{
    public partial class CCDisposition
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public int CanAssign { get; set; }
        public string? Description { get; set; }
        public byte Status { get; set; }
        public int? Validity { get; set; }
        public DateTime CreatedOn { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public int? ModifiedBy { get; set; }
    }
}
