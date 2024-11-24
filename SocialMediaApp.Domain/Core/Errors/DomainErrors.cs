using System.Net;

namespace SocialMediaApp.Domain.Core.Errors;

public static class DomainErrors
{
    public static class User
    {
        public static Error NotFound() => new Error("User not found", HttpStatusCode.BadRequest);
        public static Error EmailPasswordMismatch() => new Error("Email and Password are mismatch", HttpStatusCode.BadRequest);
        public static Error EmailAlreadyExist(string email) => new Error($"Email: {email} already exist", HttpStatusCode.BadRequest);
    }
    
    public static class Comment
    {
        public static Error CommentMustHaveTextOrFile() => new Error("Comment can't be empty, it must have a content of text, file or both.", HttpStatusCode.BadRequest);
    }
    
    public static class Post
    {
        public static Error CannotCreatePostWithoutContentOrFiles() => new Error("You can't create empty post, it must have a content of text, file or both.", HttpStatusCode.BadRequest);

    }
}