using System.ComponentModel.DataAnnotations;
using BilHealth.Utility.Enum;

namespace BilHealth.Model
{
    public class CaseSystemMessage
    {
        [Required] public Guid Id { get; private set; }
        [Required] public CaseSystemMessageType Type { get; set; } = null!;
        [Required] public DateTime DateTime { get; set; }
        [Required] public string Content { get; set; } = null!; // TODO: Needs a proper way to store contents
    }
}
