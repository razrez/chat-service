using Chat.AppCore.Common.Interfaces;
using Chat.AppCore.Common.Models;
using Chat.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Chat.Infrastructure.Persistence.Repository;
using MongoDB.Driver.Core.Configuration;

namespace Chat.Infrastructure;

public static class ConfigureServices
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(opt =>
            opt.UseNpgsql(configuration.GetConnectionString("DefaultConnection"), 
                builder => builder.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));

        services.AddScoped<IApplicationDbContext>(provider =>  provider.GetRequiredService<ApplicationDbContext>());
        
        services.AddScoped<IChatRepository, ChatRepository>();
        return services;
    }
}