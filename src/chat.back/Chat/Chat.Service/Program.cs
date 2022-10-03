using Chat.Service.Hubs;

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