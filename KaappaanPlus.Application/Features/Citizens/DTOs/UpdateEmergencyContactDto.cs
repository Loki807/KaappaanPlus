namespace KaappaanPlus.Application.Features.Citizens.DTOs
{
    public class UpdateEmergencyContactDto
    {
        public Guid CitizenId { get; set; }
        public string EmergencyContact { get; set; } = "";
    }
}
