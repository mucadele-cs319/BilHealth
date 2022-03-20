using BilHealth.Utility.Enum;

namespace BilHealth.Model
{
    public class Nurse : User
    {
        public List<TriageRequest>? TriageRequests { get; set; }
    }
}
