using CatServiceAPI.Data;
using CatServiceAPI.DTO;
using CatServiceAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CatServiceAPI.Controllers
{
    /// <summary>
    /// Manages operations related to cats, including fetching, retrieving, and filtering cats.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class CatsController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly CatService _catService;

        public CatsController(AppDbContext context, CatService catService)
        {
            _context = context;
            _catService = catService;
        }

        /// <summary>
        /// Fetches 25 cat images from the Cat API and stores them in the database.
        /// </summary>
        /// <returns>A success message if cats are fetched and stored successfully.</returns>
        [HttpPost("fetch")]
        public async Task<IActionResult> FetchCats()
        {
            await _catService.FetchAndStoreCatsAsync();
            return Ok("Cats fetched and stored successfully.");
        }

        /// <summary>
        /// Retrieves a cat by its unique ID.
        /// </summary>
        /// <param name="id">The ID of the cat to retrieve.</param>
        /// <returns>The cat details, or a 404 if not found.</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCat(int id)
        {
            var cat = await _context.Cats
        .Include(c => c.CatTags)
        .ThenInclude(ct => ct.Tag)
        .FirstOrDefaultAsync(c => c.Id == id);

            if (cat == null) return NotFound();

            var catDto = new CatDto
            {
                Id = cat.Id,
                CatId = cat.CatId,
                Width = cat.Width,
                Height = cat.Height,
                Created = cat.Created,
                Image = Convert.ToBase64String(cat.Image),
                Tags = cat.CatTags?.Select(ct => new TagDto
                {
                    Id = ct.Tag.Id,
                    Name = ct.Tag.Name
                }).ToList() ?? new List<TagDto>()
            };

            return Ok(catDto);
        }

        /// <summary>
        /// Retrieves all cats with paging support.
        /// </summary>
        /// <param name="page">The page number to retrieve (default is 1).</param>
        /// <param name="pageSize">The number of records per page (default is 10).</param>
        /// <returns>A paged list of cats.</returns>
        [HttpGet]
        public async Task<IActionResult> GetCats([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var query = _context.Cats
        .Include(c => c.CatTags)
        .ThenInclude(ct => ct.Tag)
        .AsQueryable();

            var cats = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            // Map to DTOs
            var catDtos = cats.Select(cat => new CatDto
            {
                Id = cat.Id,
                CatId = cat.CatId,
                Width = cat.Width,
                Height = cat.Height,
                Created = cat.Created,
                Image = Convert.ToBase64String(cat.Image),
                Tags = cat.CatTags?.Select(ct => new TagDto
                {
                    Id = ct.Tag.Id,
                    Name = ct.Tag.Name
                }).ToList() ?? new List<TagDto>()
            }).ToList();

            return Ok(catDtos);
        }

        /// <summary>
        /// Retrieves cats with a specific tag and paging support.
        /// </summary>
        /// <param name="tag">The tag to filter by (e.g., temperament).</param>
        /// <param name="page">The page number to retrieve (default is 1).</param>
        /// <param name="pageSize">The number of records per page (default is 10).</param>
        /// <returns>A paged list of cats filtered by the specified tag.</returns>
        [HttpGet("tagged")]
        public async Task<IActionResult> GetCatsWithTag([FromQuery] string tag, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var query = _context.Cats
                .Include(c => c.CatTags)
                .ThenInclude(ct => ct.Tag)
                .Where(c => c.CatTags.Any(ct => ct.Tag.Name == tag))
                .AsQueryable();

            var cats = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var catDtos = cats.Select(cat => new CatDto
            {
                Id = cat.Id,
                CatId = cat.CatId,
                Width = cat.Width,
                Height = cat.Height,
                Created = cat.Created,
                Image = Convert.ToBase64String(cat.Image),
                Tags = cat.CatTags.Select(ct => new TagDto
                {
                    Id = ct.Tag.Id,
                    Name = ct.Tag.Name
                }).ToList()
            }).ToList();

            return Ok(catDtos);
        }
    }
}
