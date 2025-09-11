using System.ComponentModel.DataAnnotations;

namespace FullStackRestaurantMVC.Models.ViewModels
{
    public class TableViewModel
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Table Number")]
        public int TableNumber { get; set; }

        [Required]
        [Range(1, 20)]
        [Display(Name = "Seats Available")]
        public int Capacity { get; set; }
    }
}
