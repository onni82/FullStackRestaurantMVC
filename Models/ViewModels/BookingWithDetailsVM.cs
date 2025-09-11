namespace FullStackRestaurantMVC.Models.ViewModels
{
    public class BookingWithDetailsVM
    {
        public int Id { get; set; }
        public DateTime Start { get; set; }
        public int Guests { get; set; }

        // Customer info
        public string CustomerName { get; set; } = string.Empty;
        public string CustomerPhone { get; set; } = string.Empty;

        // Table info
        public int TableNumber { get; set; }
    }
}
