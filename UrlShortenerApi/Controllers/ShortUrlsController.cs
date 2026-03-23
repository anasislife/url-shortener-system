using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UrlShortenerApi.Data;
using UrlShortenerApi.Dtos;
using UrlShortenerApi.Models;

namespace UrlShortenerApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ShortUrlsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ShortUrlsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<object>>> GetAll()
        {
            var urls = await _context.ShortUrls.ToListAsync();

            var result = urls.Select(x => new
            {
                x.Id,
                x.OriginalUrl,
                x.ShortCode,
                ShortUrl = $"{Request.Scheme}://{Request.Host}/{x.ShortCode}",
                x.CreatedAt,
                x.ClickCount
            });

            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<object>> Create(CreateShortUrlRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.OriginalUrl))
                return BadRequest("الرابط مطلوب");

            var existing = await _context.ShortUrls
                .FirstOrDefaultAsync(x => x.OriginalUrl == request.OriginalUrl);

            if (existing != null)
            {
                return Ok(new
                {
                    existing.Id,
                    existing.OriginalUrl,
                    existing.ShortCode,
                    ShortUrl = $"{Request.Scheme}://{Request.Host}/{existing.ShortCode}",
                    existing.CreatedAt,
                    existing.ClickCount
                });
            }

            var shortCode = Guid.NewGuid().ToString("N").Substring(0, 6);

            var shortUrl = new ShortUrl
            {
                OriginalUrl = request.OriginalUrl,
                ShortCode = shortCode,
                CreatedAt = DateTime.UtcNow,
                ClickCount = 0
            };

            _context.ShortUrls.Add(shortUrl);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                shortUrl.Id,
                shortUrl.OriginalUrl,
                shortUrl.ShortCode,
                ShortUrl = $"{Request.Scheme}://{Request.Host}/{shortUrl.ShortCode}",
                shortUrl.CreatedAt,
                shortUrl.ClickCount
            });
        }
    }
}
