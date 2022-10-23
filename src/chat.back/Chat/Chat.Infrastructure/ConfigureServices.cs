using Chat.AppCore.Common.Interfaces;
using Chat.AppCore.Common.Models;
using Chat.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Chat.Infrastructure.Persistence.Repository;
using Chat.Infrastructure.Persistence.Services;
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

        var config = configuration.GetSection("MetadataDB");
        var cst = config.GetSection("ConnectionString");
        var dbMame = config.GetSection("DatabaseName");
        var collectionName = config.GetSection("BooksCollectionName");
        
        services.AddSingleton<MetadataService>();
        return services;
    }
}