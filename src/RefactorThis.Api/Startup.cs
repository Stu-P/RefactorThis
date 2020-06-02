using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using RefactorThis.Api.Extensions;
using RefactorThis.Api.Helpers;
using RefactorThis.Api.Middlewares;
using Serilog;

namespace RefactorThis
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public IWebHostEnvironment HostingEnv { get; }

        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            HostingEnv = env;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvcServices();
            services.AddApplicationServices();
            services.AddRepository(Configuration, HostingEnv);
            services.AddSwaggerGen(c => c.SwaggerDoc("v1", new OpenApiInfo { Title = "RefactorThis Api", Version = "v1" }));
            services.AddSwaggerGenNewtonsoftSupport();

            services
                .AddHealthChecks()
                .AddSqlServer(Configuration.GetConnectionString("Products"));
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.ApplyDbMigrations();
            app.UseMiddleware<CorrelationMiddleware>();
            app.UseSerilogRequestLogging(opts =>
            {
                opts.EnrichDiagnosticContext = LogHelpers.EnrichFromRequest;
                opts.GetLevel = LogHelpers.ExcludeHealthChecks;
            });

            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHealthChecks("/health");
            });

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Blockchain Api");
            });
        }
    }
}