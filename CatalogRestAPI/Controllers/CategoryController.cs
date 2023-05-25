using CatalogRestAPI.Models.Category;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CatalogRestAPI.Controllers
{
    [ApiController]
    [Route("categories")]
    public class CategoriesController : ControllerBase
    {
        private CatalogRestAPIContext _context;

        public CategoriesController(CatalogRestAPIContext context) 
        {
            _context = context;
        }

        [HttpGet("id")]
        public async Task<ActionResult<List<Category>>> Get(Guid? id)
        {
            if (id == null) return Ok(_context.Categories);

            var category = await _context.Categories.FindAsync(id);

            if (category == null) return NotFound("No category exists with given id");

            return Ok(category);
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] CategoryCreate category)
        {
            if (_context.Categories.Any((c) => c.Name == category.Name))
            {
                return BadRequest("Category with given name already exists.");
            }

            try
            {
                var newCategory = new Category()
                {
                    Id = Guid.NewGuid(),
                    Name = category.Name,
                };

                await _context.AddAsync(newCategory);
                await _context.SaveChangesAsync();
                return Created(nameof(Category), newCategory);
            }
            catch
            {
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPut]
        public async Task<ActionResult> Put([FromBody] Category categoryUpdate)
        {
            if(!_context.Categories.Any(c => c.Id == categoryUpdate.Id))
            {
                return NotFound("Category with provided Id does not exist.");
            }
            try
            {
                _context.Update<Category>(categoryUpdate);
                await _context.SaveChangesAsync();

                return Ok();
            }
            catch
            {
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpDelete]
        public async Task<ActionResult> Delete([FromBody] Guid id)
        {
            var categoryToDelete = await _context.Categories.FirstOrDefaultAsync(c => c.Id == id);
            
            if(categoryToDelete == null)
            { 
                return NotFound();
            }

            var categoriesToDelete = _context.Catalogs.Where(c => c.CategoryId == id).ToList();
            _context.RemoveRange(categoriesToDelete);
 
            _context.Categories.Remove(categoryToDelete);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
