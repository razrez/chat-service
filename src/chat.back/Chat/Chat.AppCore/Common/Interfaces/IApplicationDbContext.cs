using Chat.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Chat.AppCore.Common.Interfaces;

public interface IApplicationDbContext 
{
    DbSet<ChatMessage> ChatMessages { get; set; }
    DbSet<ChatConnection> ChatConnections { get; set; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}