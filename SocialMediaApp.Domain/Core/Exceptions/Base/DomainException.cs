using System.Net;
using SocialMediaApp.Domain.Core.Errors;

namespace SocialMediaApp.Domain.Core.Exceptions.Base;

public class DomainException : Exception
{
    public DomainException(string message, HttpStatusCode statusCode) : base(message) => StatusCode = statusCode;
    public HttpStatusCode StatusCode { get; set; }
    
    public static implicit operator Error(DomainException domainException) => new Error(domainException.Message, domainException.StatusCode);
}