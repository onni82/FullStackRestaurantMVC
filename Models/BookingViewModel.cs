namespace FullStackRestaurantMVC.Models
{
    public class BookingViewModel
    {
        public int Id { get; set; }
        public int TableId { get; set; }
        public string TableName { get; set; } = string.Empty;
        public int CustomerId { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public DateTime Start { get; set; }
        public int Guests { get; set; }
    }
}
