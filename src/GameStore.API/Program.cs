using GameStore.Extensions;
using System.Text.Json.Serialization;
using GameStore.Middleware;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDatabase(builder.Configuration);

builder.Services.AddRepositories();

builder.Services.AddApplicationServices();

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = 
            System.Text.Json.JsonNamingPolicy.CamelCase;
        options.JsonSerializerOptions.WriteIndented =
            builder.Environment.IsDevelopment();
        options.JsonSerializerOptions.DefaultIgnoreCondition =
            JsonIgnoreCondition.WhenWritingNull;
        options.JsonSerializerOptions.ReferenceHandler =
            ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.Converters.Add(
            new JsonStringEnumConverter());
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "GameStore API",
        Version = "v1.0",
        Description = "A basic game store management API built with ASP.NET Core 9.",
    });
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("Development", policy =>
    {
        policy.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
    
    options.AddPolicy("Production", policy =>
    {
        policy.WithOrigins("https://localhost:7000")
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials();
    });
});

builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();
builder.Logging.SetMinimumLevel(builder.Environment.IsDevelopment() ? 
    LogLevel.Debug : LogLevel.Information);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage(); // Detailed error pages
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "GameStore API V1");
        options.RoutePrefix = string.Empty; // Swagger at root URL
        options.DocumentTitle = "GameStore API Documentation";
        options.DisplayRequestDuration();
    });
}
else
{
    app.UseMiddleware<ExceptionHandler>(); // Custom exception handler in production
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseCors(app.Environment.IsDevelopment() ? "Development" : "Production");

app.MapControllers();

// TODO: Authentication & Authorization to be implemented in the future.

await app.ApplyMigrationsAsync();

app.Run();
