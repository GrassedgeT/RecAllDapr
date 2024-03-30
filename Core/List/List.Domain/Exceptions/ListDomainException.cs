using Ddd.Domain;

namespace RecAll.Core.List.Domain;

public class ListDomainException : DomainException {
    public ListDomainException() { }

    public ListDomainException(string message) : base(message) { }

    public ListDomainException(string message, Exception innerException) : base(message, innerException) { }
}