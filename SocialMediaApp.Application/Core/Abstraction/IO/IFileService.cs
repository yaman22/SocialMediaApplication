using Microsoft.AspNetCore.Http;

namespace SocialMediaApp.Application.Core.Abstraction.IO;

public interface IFileService
{
    Task<string>? UploadAsync(IFormFile? file, string? path = null);
    Task<List<string>>? UploadAsync(List<IFormFile>? files);
    Task<string?> Modify(string? prop,IFormFile? file);
    void Delete(string? path);
    void Delete(List<string>? paths);
}