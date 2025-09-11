namespace FullStackRestaurantMVC.Models.ViewModels
{
    public class MenuItemViewModel
    {
        public int Id { get; set; } // ignored for create

        public string Name { get; set; } = string.Empty;

        public decimal Price { get; set; }

        public string? Description { get; set; }

        public bool IsPopular { get; set; }

        public string? ImageUrl { get; set; }
    }
}
