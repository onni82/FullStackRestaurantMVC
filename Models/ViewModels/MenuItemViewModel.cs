using System.ComponentModel.DataAnnotations;

namespace FullStackRestaurantMVC.Models.ViewModels
{
    public class MenuItemViewModel
    {
        public int Id { get; set; } // ignored for create

        [Required(ErrorMessage = "Namn är obligatoriskt.")]
        [Display(Name = "Namn")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Pris är obligatoriskt.")]
        [Display(Name = "Pris")]
        public decimal Price { get; set; }

        [Display(Name = "Beskrivning")]
        public string? Description { get; set; }

        [Display(Name = "Populär rätt")]
        public bool IsPopular { get; set; }

        [Display(Name = "Bildlänk")]
        public string? ImageUrl { get; set; }
    }
}
