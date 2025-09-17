using System.ComponentModel.DataAnnotations;

namespace FullStackRestaurantMVC.Models.ViewModels
{
    public class BookingViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Startdatum är obligatorisk.")]
        [Display(Name = "Startdatum")]
        public DateOnly StartDate { get; set; }

        [Required(ErrorMessage = "Starttid är obligatorisk.")]
        [Display(Name = "Starttid")]
        public TimeOnly StartTime { get; set; }

        [Required(ErrorMessage = "Antal gäster är obligatoriskt.")]
        [Range(1, 20, ErrorMessage = "Antalet gäster måste vara mellan 1 och 20.")]
        [Display(Name = "Antal gäster")]
        public int Guests { get; set; }

        [Required(ErrorMessage = "Kund måste anges.")]
        [Display(Name = "Kund")]
        public int CustomerId { get; set; }

        [Required(ErrorMessage = "Bord måste anges.")]
        [Display(Name = "Bord")]
        public int TableId { get; set; }

        // Dropdown helpers
        public IEnumerable<CustomerViewModel>? Customers { get; set; }
        public IEnumerable<TableViewModel>? Tables { get; set; }
    }
}
