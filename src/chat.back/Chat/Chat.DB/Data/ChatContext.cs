using Chat.DB.Models;
using Microsoft.EntityFrameworkCore;
namespace Chat.DB.Data;

public sealed class ChatContext : DbContext
{
    public DbSet<ChatMessage> ChatMessages { get; set; } = null!;
    public DbSet<ChatConnection> ChatConnections { get; set; } = null!;

    public ChatContext()
    {
        Database.EnsureCreated();
    }
    
    public ChatContext(DbContextOptions<ChatContext> options) : base(options){}
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();
            var connectionString = configuration.GetConnectionString("DefaultConnection");
                
            optionsBuilder.UseNpgsql(connectionString);
        }
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.Entity<ChatMessage>()
            .HasKey(k => new { k.Room, k.User });
    }
}