using Amazon.S3;
using Chat.API.Hubs;
using Chat.API.Hubs.Models;
using Chat.API.Publisher;
using Chat.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddInfrastructure(builder.Configuration);

//test AWS Services
builder.Services.AddDefaultAWSOptions(builder.Configuration.GetAWSOptions());
builder.Services.AddAWSService<IAmazonS3>();

builder.Services.AddSignalR();
builder.Services.AddScoped<IMessagePublisher, MessagePublisher>();

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "anybody",
        corsPolicyBuilder =>
        {
            corsPolicyBuilder
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials()
                .SetIsOriginAllowed(_ => true);
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

app.UseCors("anybody");

app.MapControllers();

app.MapHub<ChatHub>("/chat");

app.Run();