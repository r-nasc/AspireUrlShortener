using AspireUrlShortener.Api.UrlShortening.Database;
using AspireUrlShortener.Api.UrlShortening.Models;

namespace AspireUrlShortener.Api.UrlShortening.Services;

using ShortCode = string;

internal sealed class UrlShorteningService(UrlShorteningRepository repository)
{
    public async Task<ShortCode> ShortenUrl(string url, CancellationToken cancellationToken) => await repository.InsertUrlAsync(url, cancellationToken);

    public async Task<string?> GetOriginalUrl(ShortCode shortCode, CancellationToken cancellationToken) => await repository.GetUrlAsync(shortCode, cancellationToken);

    public async Task<IEnumerable<ShortenedUrl>> QueryShortenedUrls(CancellationToken cancellationToken) => await repository.GetAllUrlsAsync(cancellationToken);
}
