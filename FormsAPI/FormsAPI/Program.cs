using Amazon.S3.Model.Internal.MarshallTransformations;
using Asp.Versioning;
using Asp.Versioning.ApiExplorer;
using FormsAPI.Controllers;
using FormsAPI.Extensions;
using FormsAPI.Middlewares;
using FormsAPI.ModelProfiles;
using FormsAPI.Services;
using FormsAPI.Services.Elastic;
using FormsAPI.Services.DropboxAPIService;
using FormsAPI.Services.SalesForce;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Microsoft.VisualBasic;
using Models.Enums;
using OnixLabs.Security.Cryptography;
using Org.BouncyCastle.Asn1.Cms.Ecc;
using Repositories;
using Repositories.Data;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Text.Json;
using System.Threading.Tasks;

public class Program
{
    private static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        ConfigureServices(builder);

        var app = builder.Build();

        app.UseRouting();
        app.UseExceptionHandler(_ => { });
        app.UseAuthorization();

        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            var provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();
            foreach(var desc in provider.ApiVersionDescriptions)
            {
                options.SwaggerEndpoint($"/swagger/{desc.GroupName}/swagger.json",
                    $"FormsAPI {desc.GroupName.ToUpper()}");
            }
            options.RoutePrefix = "swagger";
        });
        /* app.ApplyMigrations();*/
        

        app.MapControllers();

        app.Run();
    }

    private static void ConfigureServices(WebApplicationBuilder builder)
    {
        builder.Services.AddDbContext<FormsDbContext>(options =>
        {

            options.UseNpgsql(
                builder.Configuration.GetConnectionString("default"),
                o =>
                {
                    o.MapEnum<UserRole>("role");
                    o.MapEnum<UserState>("state");
                    o.MapEnum<FormAccessibility>("accessibility");

                });
            options.UseLazyLoadingProxies();
        });        

        builder.Services.AddControllers().AddApplicationPart(typeof(AccountController).Assembly).AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
        });

        builder.Services.AddEndpointsApiExplorer();

        builder.Services.AddApiVersioning(options =>
        {
            options.DefaultApiVersion = new Asp.Versioning.ApiVersion(1.0);
            options.AssumeDefaultVersionWhenUnspecified = true;
            options.ReportApiVersions = true;

        }).AddApiExplorer(options =>
        {
            options.GroupNameFormat = "'v'VVV";
            options.SubstituteApiVersionInUrl = true;
        });

        builder.Services.AddSwaggerGen(options =>
        {
            var provider = builder.Services.BuildServiceProvider()
                .GetRequiredService<IApiVersionDescriptionProvider>();

            foreach (var description in provider.ApiVersionDescriptions)
            {
                options.SwaggerDoc(
                    description.GroupName,
                    new OpenApiInfo
                    {
                        Title = $"FormsAPI {description.ApiVersion}",
                        Version = description.ApiVersion.ToString()
                    });
            }

            options.DocInclusionPredicate((documentVersion, apiDesc) =>
            {
                if (!apiDesc.TryGetMethodInfo(out var methodInfo))
                    return false;

                var versions = methodInfo.DeclaringType?
                    .GetCustomAttributes(true)
                    .OfType<ApiVersionAttribute>()
                    .SelectMany(attr => attr.Versions);

                var mappedVersions = methodInfo
                    .GetCustomAttributes(true)
                    .OfType<MapToApiVersionAttribute>()
                    .SelectMany(attr => attr.Versions);

                return versions?.Any(v =>
                    !mappedVersions.Any() || mappedVersions.Contains(v)) ?? false;
            });
        });

        builder.Services.AddExceptionHandler<ExceptionMiddleware>();
        builder.Services.AddMemoryCache();

        builder.Services.Configure<DropboxAPISettings>(builder.Configuration.GetSection("DropboxAPISettings"));
        builder.Services.Configure<SalesforceSettings>(builder.Configuration.GetSection("SalesforceSettings"));
        builder.Services.Configure<ElasticSettings>(builder.Configuration.GetSection("ElasticSettings"));

        builder.Services.AddScoped<EmailService>(); 
        builder.Services.AddSingleton<ImageService>(); 
        builder.Services.AddSingleton<EncryptionService>();
        builder.Services.AddSingleton<IDropboxAPIService, DropboxAPIService>();
        builder.Services.AddSingleton<IElasticService, ElasticsearchService>();
        builder.Services.AddSingleton<ISalesforceService, SalesForceService>();

        builder.Services.AddScoped<AccessFormUsersRepository>();
        builder.Services.AddScoped<CheckBoxesRepository>();
        builder.Services.AddScoped<CommentsRepository>();
        builder.Services.AddScoped<FormAnswersRepository>();
        builder.Services.AddScoped<FormQuestionOptionsRepository>();
        builder.Services.AddScoped<FormQuestionRepository>();
        builder.Services.AddScoped<FormsRepository>();
        builder.Services.AddScoped<FormTagsRepository>();
        builder.Services.AddScoped<IntegerAnswersRepository>();
        builder.Services.AddScoped<LikesRepository>();
        builder.Services.AddScoped<LongTextAnswersRepository>();
        builder.Services.AddScoped<QuestionTypesRepository>();
        builder.Services.AddScoped<ShortTextAnswersRepository>();
        builder.Services.AddScoped<TagsRepository>();
        builder.Services.AddScoped<TopicsRepository>();
        builder.Services.AddScoped<UsersRepository>();

        builder.Services.AddAutoMapper(typeof(UserProfile));
        builder.Services.AddAutoMapper(typeof(FormsProfile));
        builder.Services.AddAutoMapper(typeof(FormAnswersProfile));
    }
}