using AspireUrlShortener.Api.UrlShortening.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Hybrid;

namespace AspireUrlShortener.Api.UrlShortening.Database;


internal sealed class UrlShorteningRepository(ApplicationDbContext dbContext, HybridCache cache)
{
    public async Task<string> InsertUrlAsync(string url, CancellationToken cancellationToken)
    {
        var newUrl = new ShortenedUrl { Url = url };

        await dbContext.AddAsync(newUrl, cancellationToken).ConfigureAwait(false);
        await dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        var shortCode = Encode(newUrl.Id);
        await cache.SetAsync(shortCode, newUrl.Url, cancellationToken: cancellationToken);

        return shortCode;
    }

    public async Task<string?> GetUrlAsync(string shortCode, CancellationToken cancellationToken)
    {
        return await cache.GetOrCreateAsync(
            key: shortCode,
            cancellationToken: cancellationToken,
            factory: async cancel =>
                {
                    try
                    {
                        var id = Decode(shortCode);
                        return await dbContext.ShortenedUrls
                            .AsNoTracking()
                            .Where(x => x.Id == id)
                            .Select(x => x.Url)
                            .FirstOrDefaultAsync(cancel)
                            .ConfigureAwait(false);
                    }
                    catch (FormatException)
                    {
                        return null;
                    };                    
                }
            );
        
    }

    public async Task<IEnumerable<ShortenedUrl>> GetAllUrlsAsync(CancellationToken cancellationToken)
    {
        return await dbContext.ShortenedUrls
            .AsNoTracking()
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);
    }

    private static string Encode(long value) => Convert.ToString(value, 16);
    private static long Decode(string value) => Convert.ToInt64(value, 16);
}
