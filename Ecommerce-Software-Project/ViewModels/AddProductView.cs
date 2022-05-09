using Microsoft.AspNetCore.Mvc.Rendering;

namespace Ecommerce_Software_Project.ViewModels;

public class AddProductView
{
    public Product? product { get; set; }
    [Display(Name = "Product Image")]
    public IFormFile? productImage { get; set; }

    public IEnumerable<SelectListItem>? categories { get; set; }
}