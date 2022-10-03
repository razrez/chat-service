using Chat.Service.Hubs;
using Chat.Service.Hubs.Models;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSignalR();
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(corsPolicyBuilder =>
    {
        corsPolicyBuilder.WithOrigins("http://localhost:3000")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});

//ConnectionId - the key
builder.Services.AddSingleton<IDictionary<string, UserConnection>>(opts => 
    new Dictionary<string, UserConnection>());

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseHsts();
}

/*app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();*/

app.UseRouting();

app.UseCors();


app.MapGet("/", () => "Hello World!");
app.MapHub<ChatHub>("/chat");

app.Run();