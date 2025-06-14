using Microsoft.EntityFrameworkCore;
using Repositories.Data;

namespace FormsAPI.Extensions
{
    public static class MigrationExtension
    {
        public static void ApplyMigrations(this IApplicationBuilder applicationBuilder)
        {
            using var scope = applicationBuilder.ApplicationServices.CreateScope();
            using var dbContext = scope.ServiceProvider.GetRequiredService<FormsDbContext>();
            dbContext!.Database.Migrate();
        }
    }
}
