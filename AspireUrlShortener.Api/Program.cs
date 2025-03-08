using AspireUrlShortener.Api.UrlShortening.Database;
using AspireUrlShortener.Api.UrlShortening.Extensions;
using AspireUrlShortener.Api.UrlShortening.Services;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

#pragma warning disable EXTEXP0018
builder.Services.AddHybridCache();
#pragma warning restore EXTEXP0018

builder.Services.AddOpenApi();
builder.Services.AddScoped<UrlShorteningRepository>();
builder.Services.AddScoped<UrlShorteningService>();
builder.Services.AddCors();

builder.AddNpgsqlDbContext<ApplicationDbContext>("urlshortener-postgres-db");

/* 
 DB initialization and seeding should be done on a different project/container
 to avoid race conditions on multi-instances. 
 */
builder.Services.AddSingleton<UrlShorteningDbInitializer>();
builder.Services.AddHostedService(sp => sp.GetRequiredService<UrlShorteningDbInitializer>());
builder.Services.AddHealthChecks()
    .AddCheck<HistoryDbInitializerHealthCheck>("HistoryDbInitializer", null);

builder.AddRedisDistributedCache("urlshortener-redis");

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseCors(policy => policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
    app.MapOpenApi();
    app.UseSwaggerUI(options =>
    {
        options.DisplayRequestDuration();
        options.RoutePrefix = string.Empty;
        options.SwaggerEndpoint("/openapi/v1.json", "OpenAPI v1");
    });
}

app.MapUrlShortenerApi();
app.MapDefaultEndpoints();

app.UseHttpsRedirection();
app.Run();

