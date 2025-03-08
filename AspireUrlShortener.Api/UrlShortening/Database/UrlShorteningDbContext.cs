using AspireUrlShortener.Api.UrlShortening.Models;
using Microsoft.EntityFrameworkCore;

namespace AspireUrlShortener.Api.UrlShortening.Database;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions options) : base(options)  { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("urls");
    }

    public DbSet<ShortenedUrl> ShortenedUrls { get; set; }
}
