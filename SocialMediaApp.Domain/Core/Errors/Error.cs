using System.Net;

namespace SocialMediaApp.Domain.Core.Errors;

public class Error : IEquatable<Error>
{
    private Error()
    {
    }

    public static Error None => new Error("No Errors", HttpStatusCode.Accepted);
    public static Error NullValue => new Error("Null Value", HttpStatusCode.BadRequest);
    public Error(string message, HttpStatusCode statusCode) => (Message, StatusCode) = (message, statusCode);
    public static Error Create(Exception exception) => new Error($"{exception.GetType().Name} - {exception.Message}", HttpStatusCode.InternalServerError);

    public string Message { get; }
    public HttpStatusCode StatusCode { get; }

    public bool Equals(Error? other) => other != null && StatusCode == other.StatusCode; 
    public override bool Equals(object? obj) => Equals(obj as Error);
    public override int GetHashCode() => HashCode.Combine(Message, (int)StatusCode);
    public static bool operator ==(Error? left, Error? right) => Equals(left, right);
    public static bool operator !=(Error? left, Error? right) => !Equals(left, right);
}