using Microsoft.EntityFrameworkCore;
using MyShop_Logging.Data;
using MyShop_Logging.Mappings;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Add controllers
builder.Services.AddControllers();

// Add AutoMapper
builder.Services.AddAutoMapper(typeof(ProductProfile).Assembly);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Database
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseLazyLoadingProxies()
        .UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"));
});

// Serilog
var loggerConfiguration = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File($"logs/app_{DateTime.Now:yyyy-MM-dd}.log", rollingInterval: RollingInterval.Hour);

if (builder.Environment.IsDevelopment())
{
    loggerConfiguration.MinimumLevel.Information();
}
else if (builder.Environment.IsProduction())
{
    loggerConfiguration.MinimumLevel.Information();
}

Log.Logger = loggerConfiguration.CreateLogger();
builder.Logging.AddSerilog();

var app = builder.Build();

// Seed the database
using (var scope = app.Services.CreateScope())
{
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

string appStartLogMessage = @"
     /~___________________~\ 
     .---------------------. 
    (| APPLICATION STARTED |)
     '---------------------' 
     \_~~~~~~~~~~~~~~~~~~~_/ 
"
+
    $"\t\nApplication started at {DateTime.Now}"
+
    $"\t\nEnvironment: {app.Environment.EnvironmentName}\n";

app.Logger.LogInformation(appStartLogMessage);
app.Run();
