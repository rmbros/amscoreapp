using AmsApp.Models;

namespace AmsApp.Dto
{
    public class CallDto
    {
        public int Id { get; set; }
        public string Mobile { get; set; } = null!;
        public string? Name { get; set; }
        public int? Disposition { get; set; }
        public DateTime? NextCallDate { get; set; }
        public string MaskedMobile
        {
            get => string.IsNullOrEmpty(Mobile) ? string.Empty : $"xxxxxx{Mobile.Substring(6, 4)}";
        }
    }
}