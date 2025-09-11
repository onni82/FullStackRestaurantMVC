using System.ComponentModel.DataAnnotations;

namespace FullStackRestaurantMVC.Models.ViewModels
{
    public class BookingViewModel
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Start Time")]
        public DateTime Start { get; set; }

        [Required]
        [Range(1, 20)]
        public int Guests { get; set; }

        [Required]
        [Display(Name = "Customer")]
        public int CustomerId { get; set; }

        [Required]
        [Display(Name = "Table")]
        public int TableId { get; set; }

        // Dropdown helpers
        public IEnumerable<CustomerViewModel>? Customers { get; set; }
        public IEnumerable<TableViewModel>? Tables { get; set; }
    }
}
