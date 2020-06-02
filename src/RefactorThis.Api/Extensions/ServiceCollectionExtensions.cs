using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using RefactorThis.Api.Filters;
using RefactorThis.Core.Interfaces;
using RefactorThis.Core.Services;
using RefactorThis.Data.Contexts;
using RefactorThis.Data.Repositories;

namespace RefactorThis.Api.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddMvcServices(this IServiceCollection services)
        {
            services
                .AddControllers(opts => opts.Filters.Add<ExceptionFilter>())
                .AddNewtonsoftJson(opts => opts.SerializerSettings.NullValueHandling = NullValueHandling.Ignore)
                .AddJsonOptions(opts => opts.JsonSerializerOptions.IgnoreNullValues = true)
                .SetCompatibilityVersion(CompatibilityVersion.Version_3_0);

            return services;
        }

        public static IServiceCollection AddRepository(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment env)
        {
            services.AddScoped<IProductRepository, ProductRepository>();

            var connectionString = configuration.GetConnectionString("Products");
            if (env.IsDevelopment())
            {
                services.AddDbContext<ProductDbContext>(options => options.UseSqlite(connectionString));
            }
            else
            {
                services.AddDbContextPool<ProductDbContext>(options =>
                    options.UseSqlServer(connectionString, opts => opts.EnableRetryOnFailure(2)));
            }
            return services;
        }

        public static IServiceCollection AddApplicationServices(this IServiceCollection services) =>
             services
                .AddScoped<IProductService, ProductService>()
                .AddScoped<IGuidGenerator, GuidGenerator>()
                .AddAutoMapper(typeof(Startup));
    }
}