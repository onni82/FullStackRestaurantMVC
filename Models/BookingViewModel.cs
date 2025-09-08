using System;
using System.ComponentModel.DataAnnotations;

namespace FullStackRestaurantMVC.Models
{
    public class BookingViewModel
    {
        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Datum")]
        [FutureDate(ErrorMessage = "Date must be today or in the future.")]
        public DateTime Date { get; set; }

        [Required]
        [DataType(DataType.Time)]
        [Display(Name = "Tid")]
        [OpeningHours(10, 22, ErrorMessage = "Time must be within opening hours (10:00–22:00).")]
        public TimeSpan Time { get; set; }

        [Required]
        [Display(Name = "Antal gäster")]
        [Range(1, 20, ErrorMessage = "Guests must be between 1 and 20.")]
        public int Guests { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name = "Namn")]
        public string Name { get; set; } = string.Empty;

        [Required]
        [Phone]
        [Display(Name = "Telefonnummer")]
        public string Phone { get; set; } = string.Empty;
    }

    // Custom validation for future date
    public class FutureDateAttribute : ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            if (value is DateTime date)
            {
                return date.Date >= DateTime.Today;
            }
            return false;
        }
    }

    // Custom validation for opening hours
    public class OpeningHoursAttribute : ValidationAttribute
    {
        private readonly int _openHour;
        private readonly int _closeHour;

        public OpeningHoursAttribute(int openHour, int closeHour)
        {
            _openHour = openHour;
            _closeHour = closeHour;
        }

        public override bool IsValid(object? value)
        {
            if (value is TimeSpan time)
            {
                return time.Hours >= _openHour && time.Hours < _closeHour;
            }
            return false;
        }
    }
}
