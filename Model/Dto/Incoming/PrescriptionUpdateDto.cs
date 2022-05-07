using System.ComponentModel.DataAnnotations;

namespace BilHealth.Model.Dto.Incoming
{
    public class PrescriptionUpdateDto
    {
        [Required] public string Item { get; set; } = null!;
    }
}
