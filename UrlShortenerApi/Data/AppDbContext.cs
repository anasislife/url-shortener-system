using Microsoft.EntityFrameworkCore;
using UrlShortenerApi.Models;

namespace UrlShortenerApi.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<ShortUrl> ShortUrls { get; set; }
    }
}