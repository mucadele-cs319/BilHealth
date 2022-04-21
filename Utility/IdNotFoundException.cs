namespace BilHealth.Utility
{
    public class IdNotFoundException : KeyNotFoundException
    {
        public IdNotFoundException() : base()
        {
        }

        public IdNotFoundException(string message) : base(message)
        {
        }

        public IdNotFoundException(string message, Exception inner) : base(message, inner)
        {
        }

        public IdNotFoundException(Type type, Guid id) : base($"No '{type.Name}' found with ID '{id}'")
        {
        }
    }
}
