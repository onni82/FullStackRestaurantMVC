using System.ComponentModel.DataAnnotations;

namespace FullStackRestaurantMVC.Models.ViewModels
{
    public class MenuItemCreateViewModel
    {
        [Required]
        [StringLength(200)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [Range(0.01, 9999.99)]
        public decimal Price { get; set; }

        [StringLength(4000)]
        public string? Description { get; set; }

        public bool IsPopular { get; set; }

        [Url]
        [StringLength(500)]
        public string? ImageUrl { get; set; }
    }
}
