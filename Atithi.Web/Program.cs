using Atithi.Web.Context;
using Atithi.Web.Services;
using Atithi.Web.Services.Interface;
using Microsoft.AspNetCore.WebSockets;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers(options =>
{
    options.Conventions.Add(new LowercaseControllerNameConvention());
});
builder.Services.AddScoped<IMenuService, MenuService>(); // Register with DI
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IOrderService, OrderService>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration.GetConnectionString("AtithiDbConnectionStrings");
builder.Services.AddDbContext<AtithiDbContext>(options =>
{
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
});

builder.Services.AddWebSockets(options =>
{
    options.KeepAliveInterval = TimeSpan.FromMinutes(5); // Keep-alive interval
});

builder.Services.AddCors(options => options.AddPolicy(name: "FrontendUI",
    policy =>
    {
        policy.WithOrigins("http://localhost:4200").AllowAnyMethod().AllowAnyHeader();
        policy.WithOrigins("https://localhost:4200").AllowAnyMethod().AllowAnyHeader();
    }
    ));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("FrontendUI");
app.UseHttpsRedirection();

app.MapControllers();
app.UseWebSockets();
app.UseRouting();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    // WebSocket endpoint for order status
    endpoints.MapGet("/ws/orderstatus", async context =>
    {
        if (context.WebSockets.IsWebSocketRequest)
        {
            var webSocket = await context.WebSockets.AcceptWebSocketAsync();
            if (webSocket != null)
            {
                Console.WriteLine("WebSocket connection accepted");
                await OrderStatusWebSocket.HandleWebSocket(webSocket);
            }
            else
            {
                Console.WriteLine("WebSocket connection failed");
                context.Response.StatusCode = 400;
            }
        }
        else
        {
            context.Response.StatusCode = 400; // Not a WebSocket request
        }
    });
});

app.Run();
