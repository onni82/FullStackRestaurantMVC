namespace FullStackRestaurantMVC.Models
{
    public class Booking
    {
        public int Id { get; set; }
        public DateTime Start {  get; set; }
        public DateTime End { get; set; }
        public int Guests { get; set; }
        public int TableId { get; set; }
        public int CustomerId { get; set; }
    }
}
