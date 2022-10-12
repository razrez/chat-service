using Chat.AppCore.Common.Interfaces;
using Chat.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Chat.Infrastructure.Persistence;

public sealed class ApplicationDbContext : DbContext, IApplicationDbContext
{
    public DbSet<ChatMessage> ChatMessages { get; set; } = null!;
    public DbSet<ChatConnection> ChatConnections { get; set; } = null!;

    public ApplicationDbContext()
    {
        Database.EnsureCreated();
    }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) 
        : base(options) {}
    
    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await base.SaveChangesAsync(cancellationToken);
    }
}