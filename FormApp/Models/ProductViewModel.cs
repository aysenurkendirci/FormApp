using Microsoft.AspNetCore.Mvc;

namespace FormsApp.Models
{
    public class ProductViewModel
    {
        public List<Product> Products { get; set; } = new();
        public List<Category> Categories { get; set; } = new();
        public string? SelectedCategory { get; set; }
    }
}
