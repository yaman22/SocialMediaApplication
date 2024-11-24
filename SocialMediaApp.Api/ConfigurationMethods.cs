using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using SocialMediaApp.Api.Controllers.Base.Attributes;
using SocialMediaApp.Api.Controllers.Base.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace SocialMediaApp.Api;

public static class ConfigurationMethods
{
    /// <summary>
    /// Apply Options on Identity
    /// </summary>
    /// <param name="options"></param>
    public static void IdentityOptions(IdentityOptions options)
    {
        options.Password.RequireDigit = false;
        options.Password.RequireLowercase = false;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequireUppercase = false;
        options.Password.RequiredLength = 8;
        options.Password.RequiredUniqueChars = 1;
    }
    
    /// <summary>
    /// Swagger Options
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="options"></param>
    public static void SwaggerOptions(this WebApplicationBuilder builder, SwaggerGenOptions options)
    {

        options.SwaggerDoc("v1", new OpenApiInfo
        {
            Title = $"SocialMediaApplication {builder.Environment.EnvironmentName} API",
            Version = "v1",
            Description = $"Launched at {DateTime.UtcNow:f}"
        });
        options.CustomSchemaIds(t => $"{t.FullName!.Replace("+", ".")}");
        options.EnableAnnotations();
        options.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme
        {
            In = ParameterLocation.Header,
            Description = "Please enter a valid token",
            Name = "Authorization",
            Type = SecuritySchemeType.Http,
            Scheme = JwtBearerDefaults.AuthenticationScheme
        });
        options.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = JwtBearerDefaults.AuthenticationScheme,
                    },
                    Name = JwtBearerDefaults.AuthenticationScheme
                },
                Array.Empty<string>()
            },
        });
    }
    
    public static (IServiceCollection services, IWebHostEnvironment webHostEnvironment) AddApiGroupNames<TApiGroupNames>(this (
			IServiceCollection services,
			IWebHostEnvironment webHostEnvironment) apiFactoryParams,
		bool mustRoutContainsName = false)
		where TApiGroupNames : Enum
	{
		apiFactoryParams.services.Configure<SwaggerGenOptions>(o =>
		{
			typeof(TApiGroupNames).GetFields().Skip(1).ToList().ForEach(f =>
			{
				var info = f.GetCustomAttributes(typeof(GroupInfoAttribute), false)
					.OfType<GroupInfoAttribute>()
					.FirstOrDefault(new GroupInfoAttribute(string.Empty, string.Empty, string.Empty));
				o.SwaggerDoc(f.Name, new OpenApiInfo
				{
					Title = $"{AppDomain.CurrentDomain.FriendlyName} {info.Title} - In {apiFactoryParams.webHostEnvironment.EnvironmentName}",
					Version = info.Version,
					Description = $"Published at {DateTime.Now}",
				});
			});
			o.DocInclusionPredicate((docName, apiDescription) =>
			{
				if (!apiDescription.TryGetMethodInfo(out _)) return false;
				if (string.Equals(docName, "All", StringComparison.OrdinalIgnoreCase)) return true;
				var nationalist = apiDescription.ActionDescriptor.EndpointMetadata.FirstOrDefault(x => x is IApiGroup<TApiGroupNames>);
				if (string.Equals(docName, "NoGroup", StringComparison.OrdinalIgnoreCase)) return nationalist == null;
				if (nationalist != null)
				{
					var actionFilter = (IApiGroup<TApiGroupNames>)nationalist;
					return actionFilter.GroupNames.Any(x => string.Equals(x.ToString().Trim(), docName, StringComparison.OrdinalIgnoreCase)
															&& (!mustRoutContainsName || (apiDescription.RelativePath?.Contains(x.ToString().Trim()) ?? true)));
				}

				return false;
			});
		});
		return apiFactoryParams;
	}

	public static IApplicationBuilder UseSwagger<TApiGroupNames>(this IApplicationBuilder app, bool useApiGroupName = true)
		where TApiGroupNames : Enum
	{
		app.UseSwagger();
		app.UseSwaggerUI(c =>
		{
			if (useApiGroupName)
			{
				typeof(TApiGroupNames).GetFields().Skip(1).ToList().ForEach(f =>
				{
					var info = f.GetCustomAttributes(typeof(GroupInfoAttribute), false)
						.OfType<GroupInfoAttribute>()
						.FirstOrDefault(new GroupInfoAttribute(string.Empty, string.Empty, string.Empty));
					c.SwaggerEndpoint($"/swagger/{f.Name}/swagger.json", info.Title);
				});
			}
			else
			{
				c.SwaggerEndpoint("/swagger/v1/swagger.json", string.Empty);
			}

			c.DocExpansion(DocExpansion.None);
		});
		return app;
	}
	
	/// <summary>
	/// 
	/// </summary>
	/// <param name="options"></param>
	public static void JsonOptions(JsonOptions options)
	{
		options.JsonSerializerOptions.WriteIndented = true;
		options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
		// options.JsonSerializerOptions.Converters.Add(new RequestConverter());
	}
	
	/// <summary>
	/// JsonFile Options
	/// </summary>
	/// <param name="configuration"></param>
	/// <param name="environment"></param>
	/// <returns></returns>
	public static IConfigurationBuilder AddJsonFiles(this ConfigurationManager configuration, IWebHostEnvironment environment)
	{
		return configuration
			.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
			.AddJsonFile($"appsettings.{environment.EnvironmentName}.json", reloadOnChange: true, optional: true)
			.AddEnvironmentVariables();
	}

}