using FormsAPI.Extensions;
using FormsAPI.ModelProfiles;
using FormsAPI.Services;
using Microsoft.EntityFrameworkCore;
using Repositories;
using Repositories.Data;

public class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        ConfigureServices(builder);
        var app = builder.Build();
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
            /*app.ApplyMigrations();*/
        }
        app.UseHttpsRedirection();
        app.UseAuthorization();
        app.MapControllers();

        app.Run();
    }

    private static void ConfigureServices(WebApplicationBuilder builder)
    {
        builder.Services.AddDbContext<FormsDbContext>(options =>
        {
            options.UseNpgsql(builder.Configuration.GetConnectionString("default"));
            options.UseLazyLoadingProxies();
        });
        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddScoped<EmailService>();
        builder.Services.AddSingleton<EncryptionService>();
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
    }
}