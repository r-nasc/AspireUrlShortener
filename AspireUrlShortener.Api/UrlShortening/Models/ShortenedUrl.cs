namespace AspireUrlShortener.Api.UrlShortening.Models;

public record ShortenedUrl {
    public long Id { get; init; }
    public required string Url { get; init; }
};
