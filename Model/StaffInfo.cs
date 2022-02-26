using System.ComponentModel.DataAnnotations;

namespace BilHealth.Model
{
    public class StaffInfo
    {
        [Required] public Guid Id { get; private set; }
    }
}
