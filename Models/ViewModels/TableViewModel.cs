using System.ComponentModel.DataAnnotations;

namespace FullStackRestaurantMVC.Models.ViewModels
{
    public class TableViewModel
    {
        public int Id { get; set; }

        [Required]
        [Range(1, 500, ErrorMessage = "Table number must be between 1 and 500.")]
        [Display(Name = "Table Number")]
        public int TableNumber { get; set; }

        [Required]
        [Range(1, 20, ErrorMessage = "Capacity must be between 1 and 20.")]
        [Display(Name = "Seats Available")]
        public int Capacity { get; set; }
    }
}
