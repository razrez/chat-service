using Chat.API.Hubs;
using Chat.API.Hubs.Models;
using Chat.API.Publisher;
using Chat.Infrastructure;
using Chat.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddSignalR();
builder.Services.AddScoped<IMessagePublisher, MessagePublisher>();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(corsPolicyBuilder =>
    {
        corsPolicyBuilder.WithOrigins("http://localhost:3000", "http://192.168.1.3:3000")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});

//ConnectionId - the key
builder.Services.AddSingleton<IDictionary<string, UserConnection>>(_ => 
    new Dictionary<string, UserConnection>());

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();

app.UseCors();

//app.UseHttpsRedirection();

//app.UseAuthorization();

app.MapControllers();

app.MapHub<ChatHub>("/chat");

app.Map("/test", () => "test");

app.Run();