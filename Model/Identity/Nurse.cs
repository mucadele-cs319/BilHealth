namespace BilHealth.Model
{
    public class Nurse : DomainUser
    {
        public List<TriageRequest>? TriageRequests { get; set; }
    }
}
