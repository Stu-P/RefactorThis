using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RefactorThis.Data.Contexts;

namespace RefactorThis.Api.Extensions
{
    public static class AppBuilderExtensions
    {
        public static void ApplyDbMigrations(this IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                var logger = serviceScope.ServiceProvider.GetService<ILogger<Startup>>();
                var context = serviceScope.ServiceProvider.GetService<ProductDbContext>();
                try
                {
                    logger.LogInformation("Run pending DB migration scripts");
                    context.Database.Migrate();
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Db migration scripts failed, see exception for details");
                }
            }
        }
    }
}