using System.ComponentModel.DataAnnotations;

namespace FinanceTrackerApi.Dtos
{
    public class CategoryDto
    {
        [Required]
        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }
    }
}
