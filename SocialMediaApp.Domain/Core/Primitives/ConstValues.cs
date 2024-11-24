namespace SocialMediaApp.Domain.Core.Primitives;

public static class ConstValues
{
    public const string JwtOptions = nameof(JwtOptions);
    public const string DefaultConnection = nameof(DefaultConnection);
    
    public static readonly string wwwroot = "wwwroot";
    
    public static readonly string Uploads = nameof(Uploads);
    public static readonly string Seed = nameof(Seed);
    
    public static readonly string coloredLaptop = "coloredLaptop.jpg";
    public static readonly string F22Rapture = "F22Rapture.jpg";
    public static readonly string GTR = "GTR.jpeg";
    public static readonly string laptop = "laptop.jpg";
    public static readonly string maleUser = "maleUser.png";
    public static readonly string femaleUser = "femaleUser.png";
    
    
    
    
    
    public static string SocialMediaAppInstanceName(string environmentName) => $"{environmentName}SocialMediaApp";
    public static string JsonFileName(string? environment = "") => $"appsettings{environment}.json";
}