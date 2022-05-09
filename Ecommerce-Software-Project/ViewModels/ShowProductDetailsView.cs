using Microsoft.AspNetCore.Mvc.Rendering;

namespace Ecommerce_Software_Project.ViewModels
{
    public class ShowProductDetailsView
    {
        public Product? product { get; set; }

        public Review? review { get; set; }

        public IEnumerable<Review>? reviews { get; set; }
    }
}
