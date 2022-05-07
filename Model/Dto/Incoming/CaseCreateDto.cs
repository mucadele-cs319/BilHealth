using System.ComponentModel.DataAnnotations;
using BilHealth.Utility.Enum;

namespace BilHealth.Model.Dto.Incoming
{
    public class CaseCreateDto
    {
        [Required] public Guid PatientUserId { get; set; }
        [Required] public string Title { get; set; } = null!;
        [Required] public CaseType Type { get; set; }
    }
}
