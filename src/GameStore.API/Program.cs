using GameStore.Extensions;

var builder = WebApplication.CreateBuilder(args);

/* ----- DB CONTEXT ----- */
builder.Services.AddDatabase(builder.Configuration);

/* ----- REPOS ----- */
builder.Services.AddRepositories();

/* ----- SERVICES ----- */
builder.Services.AddApplicationServices();

/* ----- SWAGGER ----- */
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

/* ----- BUILD ----- */
var app = builder.Build();

/* ----- HTTP ----- */
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "GameStore API V1");
        c.RoutePrefix = string.Empty; // Swagger at root URL
    });
}

/* ----- DB MIGRATIONS ----- */
await app.ApplyMigrationsAsync();

/* --- RUN ---*/
app.Run();
