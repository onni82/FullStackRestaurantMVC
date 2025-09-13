using System.ComponentModel.DataAnnotations;

namespace FullStackRestaurantMVC.Models.ViewModels
{
    public class CustomerViewModel
    {
        public int Id { get; set; } // used for Edit

        [Required(ErrorMessage = "Namn är obligatoriskt.")]
        [StringLength(100, ErrorMessage = "Namnet får inte vara längre än 100 tecken.")]
        [Display(Name = "Namn")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Telefonnummer är obligatoriskt.")]
        [Phone(ErrorMessage = "Ogiltigt telefonnummer.")]
        [Display(Name = "Telefonnummer")]
        public string PhoneNumber { get; set; } = string.Empty;
    }
}
