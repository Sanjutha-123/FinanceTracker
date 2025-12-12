using FinanceTracker.Models;
using FinanceTrackerApi.Dtos;
using Microsoft.EntityFrameworkCore;

namespace FinanceTrackerApi.Service
{
    public class CategoryService
    {
        private readonly Data.ApplicationDbContext _context;

        public CategoryService(Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Category>> GetAllAsync()
        {
            return await _context.Categories.ToListAsync();
        }

        public async Task<Category?> GetByIdAsync(int id)
        {
            return await _context.Categories.FindAsync(id);
        }

        public async Task<Category> CreateAsync(CategoryDto dto)
        {
            var category = new Category
            {
                Name = dto.Name,
                Type = dto.Type
            
            };

            _context.Categories.Add(category);
            await _context.SaveChangesAsync();

            return category;
        }

        public async Task<bool> UpdateAsync(int id, CategoryDto dto)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null)
                return false;

            category.Name = dto.Name;
            category.Type = dto.Type;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null)
                return false;

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
