using System;
using System.ComponentModel.DataAnnotations;

namespace FullStackRestaurantMVC.Models
{
    public class BookingViewModel
    {
        [Required]
        [Display(Name = "Table")]
        public int TableId { get; set; }

        [Required]
        [Display(Name = "Customer")]
        public int CustomerId { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Booking Date")]
        [FutureDate(ErrorMessage = "Date must be today or in the future.")]
        public DateTime Date { get; set; }

        [Required]
        [DataType(DataType.Time)]
        [Display(Name = "Booking Time")]
        [OpeningHours(10, 22, ErrorMessage = "Time must be within opening hours (10:00–22:00).")]
        public TimeSpan Time { get; set; }

        [Required]
        [Display(Name = "Number of Guests")]
        [Range(1, 20, ErrorMessage = "Guests must be between 1 and 20.")]
        public int Guests { get; set; }
    }

    public class FutureDateAttribute : ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            return value is DateTime date && date.Date >= DateTime.Today;
        }
    }

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
            return value is TimeSpan time && time.Hours >= _openHour && time.Hours < _closeHour;
        }
    }
}
