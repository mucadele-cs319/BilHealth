using System.ComponentModel.DataAnnotations;

namespace BilHealth.Model.Dto.Incoming
{
    public class CaseMessageUpdateDto
    {
        [Required] public string Content { get; set; } = null!;
    }
}
