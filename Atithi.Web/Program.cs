using Atithi.Web.Context;
using Atithi.Web.Services;
using Atithi.Web.Services.Interface;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers(options =>
{
    options.Conventions.Add(new LowercaseControllerNameConvention());
});
builder.Services.AddScoped<IMenuService, MenuService>(); // Register with DI
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<OrderedDeliveredService>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var corsOrigins = builder.Configuration.GetSection("AllowedOrigins").Get<string[]>();


var connectionString = builder.Configuration.GetConnectionString("AtithiDbConnectionStrings");
builder.Services.AddDbContext<AtithiDbContext>(options =>
{
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
});

builder.Services.AddCors(options => options.AddPolicy(name: "FrontendUI",
    policy =>
    {
        policy.WithOrigins(corsOrigins).AllowAnyMethod().AllowAnyHeader();
        //policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
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
app.UseRouting();
app.UseAuthorization();

app.MapGet("/send/{param1}/{param2}", async (Guid param1, int param2, CancellationToken ct,OrderedDeliveredService service, HttpContext ctx) =>
{
    ctx.Response.Headers.Add("Content-Type", "text/event-stream");

    await foreach (var item in service.GetNewItemsAsync(param1, param2, ct))
    {
        if (ct.IsCancellationRequested)
            break;

        await ctx.Response.WriteAsync($"data: {JsonSerializer.Serialize(item)}\n\n");
        await ctx.Response.Body.FlushAsync();
    }

    await ctx.Response.WriteAsync($"data: ");
});
app.Run();
