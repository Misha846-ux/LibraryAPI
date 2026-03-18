namespace Books.Api.Exeptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException(string message) : base(message)
        {
        }
    }

    public class ValidationAppException : Exception
    {
        public Dictionary<string, string[]> Errors { get; }

        public ValidationAppException(Dictionary<string, string[]> errors)
            : base("Validation failed")
        {
            Errors = errors;
        }
    }
}
