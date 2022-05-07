using System.ComponentModel.DataAnnotations;
using BilHealth.Utility.Enum;
using NodaTime;

namespace BilHealth.Model
{
    public class CaseSystemMessage
    {
        [Required] public Guid Id { get; private set; }
        [Required] public CaseSystemMessageType Type { get; set; }
        [Required] public Instant DateTime { get; set; }
        [Required] public string Content { get; set; } = null!; // TODO: Needs a proper way to store contents

        [Required] public Guid CaseId { get; set; }
        public Case Case { get; set; } = null!;
    }
}
