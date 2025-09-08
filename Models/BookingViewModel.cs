using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;

namespace FullStackRestaurantMVC.Models
{
    public class BookingViewModel
    {
        public string CustomerName { get; set; } = "";
        public string PhoneNumber { get; set; } = "";
        public int Guests { get; set; }
        public DateTime Start { get; set; }

        // table dropdown
        public int TableId { get; set; }
        public List<SelectListItem> AvailableTables { get; set; } = new();
    }
}
