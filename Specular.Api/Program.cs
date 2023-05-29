using System.Reflection;
using System.Xml.Linq;
using System.Xml.XPath;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.OpenApi.Models;
using NgSoftware.Specular.Administrations.Api.Controllers;
using NgSoftware.Specular.Administrations.Api.Infrastructures;
using NgSoftware.Specular.Api.DI;
using NgSoftware.Specular.Api.Infrastructures;
using NgSoftware.Specular.Common.Mvc.Extensions;
using NgSoftware.Specular.Common.Mvc.Filters;
using NgSoftware.Specular.Context.Extensions;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSpecularLogger(builder.Configuration, "Specular.Api");
builder.Services.AddAuth(builder.Configuration);
builder.Services.AddSpecularContext();

#region Controllers

var accountAssembly = typeof(AccountController).GetTypeInfo().Assembly;
builder.Services.AddControllers(config =>
    {
        config.Conventions.Add(new ApiExplorerGroupConvention());
        config.Filters.Add<AdministrationExceptionFilter>();
    })
    .AddNewtonsoftJson()
    .AddApplicationPart(accountAssembly)
    .AddControllersAsServices();

#endregion
#region Documentations

builder.Services.AddApiVersioning(opt =>
{
    opt.DefaultApiVersion = new ApiVersion(1, 0);
    opt.AssumeDefaultVersionWhenUnspecified = true;
    opt.ReportApiVersions = true;
    opt.ApiVersionReader = ApiVersionReader.Combine(new UrlSegmentApiVersionReader(),
        new HeaderApiVersionReader("x-api-version"),
        new MediaTypeApiVersionReader("x-api-version"));
});
builder.Services.AddVersionedApiExplorer(setup =>
{
    setup.GroupNameFormat = "'v'VVV";
    setup.SubstituteApiVersionInUrl = true;
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme,
        new OpenApiSecurityScheme
        {
            Name = "Authorization",
            Type = SecuritySchemeType.Http,
            Scheme = JwtBearerDefaults.AuthenticationScheme,
            BearerFormat = "JWT",
            In = ParameterLocation.Header,
            Description =
                "Заголовок авторизации JWT с использованием схемы Bearer. \r\n\r\n Введите ваш токен в текстовое поле ниже.\r\n\r\nПример: \"eyJhbGciOiJI**********Eomy5nEqws\"",
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
                }
            },
            Array.Empty<string>()
        }
    });

    // Загружаем всю документацию и шаманим с enum, чтобы в схеме были видны summary
    // и тестировщики больше не ругались, что они не понимают что всё это значит
    var dir = new DirectoryInfo(AppContext.BaseDirectory);
    foreach (var fi in dir.EnumerateFiles("*.xml"))
    {
        var doc = XDocument.Load(fi.FullName);
        options.IncludeXmlComments(() => new XPathDocument(doc.CreateReader()), true);
        options.SchemaFilter<DescribeEnumMembers>(doc);
    }
    options.MapType<FileContentResult>(() => new OpenApiSchema { Type = "file" });
    options.OperationFilter<FormFileFilter>();
    options.CustomSchemaIds(x => x.FullName);
    options.EnableAnnotations();
});
builder.Services.AddSwaggerGenNewtonsoftSupport();
builder.Services.ConfigureOptions<ConfigureSwaggerOptions>();

#endregion
builder.Services.AddCors(options =>
{
    var section = builder.Configuration.GetSection("CorsPolicyOrigins");
    var origins = section.Get<string[]>() ?? Array.Empty<string>();
    options.AddPolicy(ApiConstants.DebugCorsPolicyName, config =>
    {
        config.WithOrigins(origins)
            .SetIsOriginAllowed(_ => true)
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials();
    });
});
builder.Services.AddDataProtection();
builder.Services.RegisterModule<ApiModule>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    var apiVersionDescriptionProvider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();
    app.UseDocumentation(apiVersionDescriptionProvider);
    app.UseCors(ApiConstants.DebugCorsPolicyName);
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseRouting();
app.UseAuthorization();
app.MapControllers();
app.Run();


/// <summary>
/// Маркерный класс для тестирования зависимостей
/// </summary>
public partial class Program { }
