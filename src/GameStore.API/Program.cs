using GameStore.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Register DbContext
builder.Services.AddDatabase(builder.Configuration);

var app = builder.Build();

// Ensure migrations are applied (schema in sync with code)
await app.ApplyMigrationsAsync();

app.Run();
