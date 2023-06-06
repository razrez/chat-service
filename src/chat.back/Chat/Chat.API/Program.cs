using Chat.API.Consumer;
using Chat.API.Hubs;
using Chat.API.Hubs.Models;
using Chat.AppCore.Common.Models;
using Chat.AppCore.Extensions;
using Chat.AppCore.Publisher;
using Chat.AppCore.Services;
using Chat.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddAwsService(builder.Configuration);

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

//determines who needs administrator assistance
builder.Services.AddSingleton<UsersQueue>();

//MongoDB Services
builder.Services.Configure<MongoDbSettings>(builder.Configuration.GetSection("MongoDB"));
builder.Services.AddSingleton<MetadataService>();
builder.Services.AddSingleton<StatisticService>();

//Redis Service
builder.Services.AddMultiplexer(builder.Configuration);
builder.Services.AddHostedService<RedisSubscriber>();

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
