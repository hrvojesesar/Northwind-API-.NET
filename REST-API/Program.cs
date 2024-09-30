using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using REST_API.Data;
using REST_API.Interfaces;
using REST_API.Repositories;
using System.Reflection;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "Northwind REST API", Version = "v1" });

    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
});


builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("NorthwindDb"));
});


builder.Services.AddHealthChecks()
    .AddSqlServer(builder.Configuration.GetConnectionString("NorthwindDb"), tags: new[] { "db" })
    .AddCheck("Memory Usage", () =>
    {
        var allocatedMemory = GC.GetTotalMemory(forceFullCollection: false);
        var maxMemory = 1_000_000_000; // 1 GB limita
        return allocatedMemory < maxMemory
            ? HealthCheckResult.Healthy("Sufficient memory", data: new Dictionary<string, object>
            {
                { "AllocatedMemoryBytes", allocatedMemory },
                { "MaxAllowedMemoryBytes", maxMemory }
            })
            : HealthCheckResult.Unhealthy("Memory usage is too high", data: new Dictionary<string, object>
            {
                { "AllocatedMemoryBytes", allocatedMemory },
                { "MaxAllowedMemoryBytes", maxMemory }
            });
    }, tags: new[] { "memory" })
    .AddCheck("Disk Storage", () =>
    {
        var drive = new DriveInfo("C");
        var freeSpace = drive.AvailableFreeSpace;
        var totalSpace = drive.TotalSize;
        var minFreeSpace = 1_000_000_000; // 1 GB limita
        return freeSpace > minFreeSpace
            ? HealthCheckResult.Healthy("Sufficient disk space", data: new Dictionary<string, object>
            {
                { "FreeSpaceBytes", freeSpace },
                { "TotalSpaceBytes", totalSpace }
            })
            : HealthCheckResult.Unhealthy("Low disk space", data: new Dictionary<string, object>
            {
                { "FreeSpaceBytes", freeSpace },
                { "TotalSpaceBytes", totalSpace }
            });
    }, tags: new[] { "disk" })
    .AddCheck("self", () => HealthCheckResult.Healthy(), tags: new[] { "self" });


builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<ISupplierRepository, SupplierRepository>();
builder.Services.AddScoped<IShipperRepository, ShipperRepository>();
builder.Services.AddScoped<IRegionRepository, RegionRepository>();
builder.Services.AddScoped<ICustomerDemographicRepository, CustomerDemographicRepository>();
builder.Services.AddScoped<ITerritoryRepository, TerritoryRepository>();
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapHealthChecks("/health", new HealthCheckOptions
{
    ResponseWriter = async (context, report) =>
    {
        context.Response.ContentType = "application/json";
        var response = new
        {
            status = report.Status.ToString(),
            totalDuration = report.TotalDuration,
            checks = report.Entries.Select(e => new
            {
                name = e.Key,
                status = e.Value.Status.ToString(),
                description = e.Value.Description,
                data = e.Value.Data,
                duration = e.Value.Duration
            })
        };
        await context.Response.WriteAsync(JsonSerializer.Serialize(response));
    }
});



app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
