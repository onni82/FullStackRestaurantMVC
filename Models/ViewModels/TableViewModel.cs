using System.ComponentModel.DataAnnotations;

namespace FullStackRestaurantMVC.Models.ViewModels
{
    public class TableViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Bordsnummer är obligatoriskt.")]
        [Range(1, 500, ErrorMessage = "Bordsnumret måste vara mellan 1 och 500.")]
        [Display(Name = "Bordsnummer")]
        public int TableNumber { get; set; }

        [Required(ErrorMessage = "Kapacitet är obligatoriskt.")]
        [Range(1, 20, ErrorMessage = "Kapaciteten måste vara mellan 1 och 20.")]
        [Display(Name = "Antal platser")]
        public int Capacity { get; set; }
    }
}
