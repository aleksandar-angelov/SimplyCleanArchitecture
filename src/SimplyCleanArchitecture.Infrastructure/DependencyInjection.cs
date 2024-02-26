using Aspekt.KICBIntegrations.Infrastructure.Data;
using Aspekt.KICBIntegrations.Infrastructure.HttpClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SimplyCleanArchitecture.Application.Common.Abstractions;
using SimplyCleanArchitecture.Infrastructure.Caching;
using SimplyCleanArchitecture.Infrastructure.HttpClient;

namespace SimplyCleanArchitecture.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var httpClientOptions = configuration.GetSection(nameof(HttpClientFactorySettings))
                     .Get<List<HttpClientFactorySettings>>();

        string connectionString = configuration.GetConnectionString("DefaultConnection");

        services
            .AddDbContext<AppDbContext>(
            (dbContextOptionsBuilder) =>
            {
                dbContextOptionsBuilder.UseSqlServer(connectionString);
            });

        services.AddTransient<SendAsyncHandler>();

        foreach (var options in httpClientOptions)
        {
            services
            .AddHttpClient(options.HttpClientName, c =>
            {
                c.BaseAddress = new Uri(options.HttpClientBaseAddress ?? "");
                c.DefaultRequestHeaders.Add("Accept", "application/json");
            })
            .AddHttpMessageHandler<SendAsyncHandler>();
        }

        services.AddSingleton<ICache, CacheService>();
        var cachingOptions = configuration.GetSection(nameof(CacheSettings))
                     .Get<CacheSettings>();

        if (cachingOptions?.Type is "Redis")
        {
            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = cachingOptions.Configuration;
                options.InstanceName = cachingOptions.InstanceName;
            });
        }
        else
        {
            services.AddDistributedMemoryCache();
        }

        services.AddScoped<IHttpClient, HttpClientService>();
        return services;
    }
}
