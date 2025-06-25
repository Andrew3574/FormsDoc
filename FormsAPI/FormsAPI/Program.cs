using FormsAPI.Extensions;
using FormsAPI.ModelProfiles;
using FormsAPI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using Models.Enums;
using Repositories;
using Repositories.Data;
using System.Text.Json;
using System.Threading.Tasks;

public class Program
{
    private static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        ConfigureServices(builder);

        var app = builder.Build();

        if(app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
            app.ApplyMigrations();
        }

        app.UseAuthorization();
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
        builder.Services.AddControllers().AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
        });
        builder.Services.AddMemoryCache();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddScoped<EmailService>(); 
        builder.Services.AddSingleton<ImageService>(); 
        builder.Services.AddSingleton<EncryptionService>();
        builder.Services.AddSingleton<ElasticsearchService>();
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
    }
}