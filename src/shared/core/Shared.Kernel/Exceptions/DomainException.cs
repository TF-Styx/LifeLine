namespace Shared.Kernel.Exceptions
{
    public class DomainException : Exception
    {
        public DomainException(string messages) : base(messages) { }

        public DomainException(string messages, Exception inner) : base(messages, inner) { }
    }
}
