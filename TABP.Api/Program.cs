using Serilog;
using TABP.Web.Configurations;
using TABP.Web.Middlewares;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();

builder.Configuration
    .AddJsonFile("appsettings.json");

builder.Services.AddApiInfrastructure();

builder.Host.UseSerilog();

builder.Services.AddHttpContextAccessor();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddControllers()
    .AddNewtonsoftJson();
builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

app.Lifetime.ApplicationStarted.Register(() => { Log.Information($"Application started at {DateTime.Now}"); });

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(opts =>
    {
        var descriptions = app.DescribeApiVersions();

        foreach (var description in descriptions)
        {
            var url = $"/swagger/{description.GroupName}/swagger.json";
            var name = description.GroupName.ToUpperInvariant();
            opts.SwaggerEndpoint(url, name);
        }
    });
}

app.UseMiddleware<GlobalExceptionHandler>();

app.UseSerilogRequestLogging();

app.UseHttpsRedirection();
app.MapControllers();

app.Lifetime.ApplicationStopped.Register(() => { Log.Information($"Application stopped at {DateTime.Now}"); });

app.Run();