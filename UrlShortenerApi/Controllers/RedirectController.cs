using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UrlShortenerApi.Data;

namespace UrlShortenerApi.Controllers
{
    [ApiController]
    [Route("go")]
    public class RedirectController : ControllerBase
    {
        private readonly AppDbContext _context;

        public RedirectController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("{shortCode}")]
        public async Task<IActionResult> RedirectToOriginalUrl(string shortCode)
        {
            var shortUrl = await _context.ShortUrls
                .FirstOrDefaultAsync(x => x.ShortCode == shortCode);

            if (shortUrl == null)
            {
                return NotFound("Short URL not found.");
            }

            shortUrl.ClickCount++;
            await _context.SaveChangesAsync();

            return Redirect(shortUrl.OriginalUrl);
        }
    }
}