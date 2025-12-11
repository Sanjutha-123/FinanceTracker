using System.ComponentModel.DataAnnotations;

namespace FinanceTracker.Models
{
    public class Category
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(150)]
        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }
    }
}
