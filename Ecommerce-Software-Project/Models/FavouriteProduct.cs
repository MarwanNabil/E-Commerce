namespace Ecommerce_Software_Project.Models;

public class FavouriteProduct
{
    [Key]
    public int Id { get; set; }
    [ForeignKey("Product")]
    public int? productId { get; set; }
    public virtual Product? Product { get; set; }
    [ForeignKey("User")]
    public int? userId { get; set; }
    public virtual User? User { get; set; }
}
