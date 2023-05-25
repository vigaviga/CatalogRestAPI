using CatalogRestAPI.Models.Catalog;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CatalogRestAPI.Controllers
{
    [ApiController]
    [Route("catalogs")]
    public class CatalogController : ControllerBase
    {
        private CatalogRestAPIContext _context;

        public CatalogController(CatalogRestAPIContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<Catalog>>> Get(
            [FromQuery] Guid? id, 
            [FromQuery] Guid? categoryId,
            [FromQuery] int pageIndex,
            [FromQuery] int pageSize)
        {
            List<Catalog> result = _context.Catalogs.ToList();

            if (id != null)
            {
                var catalog = await _context.Catalogs.FindAsync(id);

                if (catalog == null) return NotFound("No catalog exists with given id");

                return Ok(catalog);
            }

            if (categoryId != null)
            {
                result = result.Where(c => c.CategoryId == categoryId).ToList();
            }

            if (pageIndex != 0 && pageSize != 0)
            {
                result = result.Skip(pageIndex*pageSize-1).Take(pageSize).ToList();
            }

            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<Catalog>> Post([FromBody] CatalogCreate catalog)
        {
            if (_context.Catalogs.Any(p => p.Name == catalog.Name))
            {
                return BadRequest("Catalog with given name already exists.");
            }

            if (!_context.Categories.Any(c => c.Id == catalog.CategoryId))
            {
                return BadRequest("Category with given Id does not exist");
            }

            try
            {
                var newCatalog = new Catalog()
                {
                    Id = Guid.NewGuid(),
                    Name = catalog.Name,
                    CategoryId = catalog.CategoryId,
                };
                _context.Catalogs.Add(newCatalog);
                await _context.SaveChangesAsync();
                return Created(nameof(Catalog), newCatalog);
            }
            catch
            {
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] Catalog catalog)
        {
            if (!_context.Catalogs.Any(c => c.Id == catalog.Id))
            {
                return NotFound("Catalog with provided Id does not exist.");
            }

            try
            {
                _context.Update<Catalog>(catalog);
                await _context.SaveChangesAsync();

                return Ok();
            }
            catch
            {
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpDelete]
        public async Task<IActionResult> Delete([FromBody] Guid id)
        {
            var catalog = await _context.Catalogs.FindAsync(id);

            if (catalog == null)
            {
                return NotFound();
            }

            _context.Catalogs.Remove(catalog);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }

}