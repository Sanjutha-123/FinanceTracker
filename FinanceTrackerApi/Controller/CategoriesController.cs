using FinanceTracker.Models;
using FinanceTrackerApi.Dtos;
using Microsoft.AspNetCore.Mvc;
using FinanceTrackerApi.Services;

namespace FinanceTrackerApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpPost]
        public async Task<IActionResult> Create(CategoryDto dto)
        {
            var category = await _categoryService.CreateAsync(dto);
            return CreatedAtAction(nameof(Get), new { id = category.Id }, category);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] string? type)
        {
            var categories = await _categoryService.GetAllAsync(type);
            return Ok(categories);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var category = await _categoryService.GetAsync(id);
            if (category == null) return NotFound();
            return Ok(category);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, CategoryDto dto)
        {
            var updatedCategory = await _categoryService.UpdateAsync(id, dto);
            if (updatedCategory == null) return NotFound();
            return Ok(updatedCategory);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _categoryService.DeleteAsync(id);
            if (!deleted) return NotFound();
            return NoContent();
        }
    }
}
