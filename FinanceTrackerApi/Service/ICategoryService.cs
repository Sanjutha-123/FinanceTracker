using FinanceTracker.Models;
using FinanceTrackerApi.Dtos;

namespace FinanceTrackerApi.Services
{
    public interface ICategoryService
    {
        Task<Category> CreateAsync(CategoryDto dto);
        Task<List<Category>> GetAllAsync(string? type = null);
        Task<Category?> GetAsync(int id);
        Task<Category?> UpdateAsync(int id, CategoryDto dto);
        Task<bool> DeleteAsync(int id);
    }
}
