using System.ComponentModel.DataAnnotations;

namespace FullStackRestaurantMVC.Models.ViewModels
{
    public class BookingWithDetailsVM
    {
        public int Id { get; set; }

        [Display(Name = "Starttid")]
        public DateTime Start { get; set; }

        [Display(Name = "Antal gäster")]
        public int Guests { get; set; }

        // Customer info
        [Display(Name = "Kundnamn")]
        public string CustomerName { get; set; } = string.Empty;

        [Display(Name = "Kundtelefon")]
        public string CustomerPhone { get; set; } = string.Empty;

        // Table info
        [Display(Name = "Bordsnummer")]
        public int TableNumber { get; set; }
    }
}
