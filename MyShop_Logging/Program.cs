using Microsoft.EntityFrameworkCore;
using MyShop_Logging.Data;
using MyShop_Logging.Mappings;
using MyShop_Logging.Repositories;
using MyShop_Logging.Repositories.Interfaces;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Add controllers
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(
    c =>
    {
        c.SwaggerDoc("v1", new() { Title = "MyShop_Logging", Version = "v1" });
        c.EnableAnnotations();
    });

// Database
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseLazyLoadingProxies()
        .UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"));
});

// Serilog
var loggerConfiguration = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File(
        path: "logs/app_.log",
        rollingInterval: RollingInterval.Day,
        rollOnFileSizeLimit: true,
        fileSizeLimitBytes: 10 * 1024 * 1024, // 10 MB
        retainedFileCountLimit: 31, // How many log files to keep
        outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}"
    );

if (builder.Environment.IsDevelopment())
{
    loggerConfiguration.MinimumLevel.Verbose();
}
else if (builder.Environment.IsProduction())
{
    loggerConfiguration.MinimumLevel.Information();
}

Log.Logger = loggerConfiguration.CreateLogger();
builder.Logging.AddSerilog();

// Repositories
builder.Services.AddScoped<IProductRepository, ProductRepository>();

// Add AutoMapper
builder.Services.AddAutoMapper(typeof(ProductProfile).Assembly);

var app = builder.Build();

// Seed the database
if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<AppDbContext>();
        DbSeeder.Seed(context);
    }
    catch (Exception e)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(e, "An error occurred while seeding the database.");
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();

app.Run();