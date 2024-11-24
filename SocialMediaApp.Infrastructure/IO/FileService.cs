using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using SocialMediaApp.Application.Core.Abstraction.IO;
using SocialMediaApp.Domain.Core.Primitives;

namespace SocialMediaApp.Infrastructure.IO;

public class FileService(IWebHostEnvironment webHostEnvironment) : IFileService
{
	private readonly string _wwwrootPath = webHostEnvironment.WebRootPath;

	public async Task<string>? UploadAsync(IFormFile? file, string? path = null)
	{
		if (file is null)
			return null;

		if (string.IsNullOrEmpty(_wwwrootPath)) 
			throw new NullReferenceException($"Path of wwwroot is null {_wwwrootPath}");

		var uploadFolder = string.IsNullOrEmpty(path) ? _wwwrootPath : Path.Combine(_wwwrootPath, path);
		if (!Directory.Exists(uploadFolder)) Directory.CreateDirectory(uploadFolder);

		var uniqueFileName = $"{Guid.NewGuid()}_{file!.FileName}";

		var subDir = _subDir(ConstValues.Uploads);
		var filePath = Path.Combine(uploadFolder,subDir , uniqueFileName);

		await using var fileStream = new FileStream(filePath, FileMode.Create);
		await file.CopyToAsync(fileStream);
		return Path.Combine(subDir, uniqueFileName);
	}

	public async Task<List<string>>? UploadAsync(List<IFormFile>? files)
	{
		if (files is null)
		{
			return null;
		}

		List<string?> paths = new();
		foreach (var file in files)
		{
			paths.Add(await UploadAsync(file)!);
		}
		return paths!;
	}

	public async Task<string?> Modify(string? prop, IFormFile? file)
	{
		if (file is null)
			return prop;

		if (prop is not null)
		{
			Delete(prop);
		}

		return await UploadAsync(file);

	}

	public void Delete(string? path)
	{
		if (string.IsNullOrEmpty(path)) return;
		
		
		path = Path.Combine(_wwwrootPath, path);

		if (!File.Exists(path)) throw new FileNotFoundException($"File With path: {path} is not found");

		File.Delete(path);
	}

	public void Delete(List<string>? paths)
	{
		if (paths is null || !paths.Any()) return;

		foreach (var path in paths)
		{
			Delete(path);
		}
	}
	
	private string _subDir(string baseDir)
	{
		var dir = Path.Combine(baseDir, $"{DateTime.Now.Year}-{DateTime.Now.Month}");
		var fullDir = Path.Combine(_wwwrootPath, dir);
		if (!Directory.Exists(fullDir))
		{
			Directory.CreateDirectory(fullDir);
		}
		return dir;
	}
}