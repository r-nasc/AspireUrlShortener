using AspireUrlShortener.Api.UrlShortening.Models;
using AspireUrlShortener.Api.UrlShortening.Services;
using Microsoft.AspNetCore.Mvc;

namespace AspireUrlShortener.Api.UrlShortening.Extensions;

public static class UrlShorteningApiWebApplicationExtensions
{
    public static IEndpointRouteBuilder MapUrlShortenerApi(this IEndpointRouteBuilder builder)
    {
        builder.MapGet("api/v1/{shortCode}", async (string shortCode, CancellationToken cancellationToken, UrlShorteningService urlShorteningService) =>
        {
            var res = await urlShorteningService.GetOriginalUrl(shortCode, cancellationToken);
            return string.IsNullOrEmpty(res) ? Results.NotFound() : Results.Redirect(res);
        })
        .WithName("RedirectToOriginalUrl");


        builder.MapPost("api/v1/shorten", async ([FromBody] ShortenUrlRequest request, CancellationToken cancellationToken, UrlShorteningService urlShorteningService) =>
        {
            if (!Uri.TryCreate(request.Url, UriKind.Absolute, out _))
            {
                return Results.BadRequest($"{request.Url} is not a valid url format");
            }

            var shortCode = await urlShorteningService.ShortenUrl(request.Url, cancellationToken);
            return Results.Ok(new { shortCode });
        })
        .WithName("ShortenUrl");

        return builder;
    }
}
