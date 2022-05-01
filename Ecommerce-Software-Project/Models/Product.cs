namespace Ecommerce_Software_Project.Models;

public class Product
{
    [Key]
    public int Id { get; set; }
    [Required]

    public bool IsUsed { get; set; }
    [ForeignKey("Category")]

    [Required]
    public int CategoryID { get; set; }
    public virtual Category? Category { get; set; }
    [Required]
    public string? ProductName { get; set; }
    [ForeignKey("Seller")]
    public int? SellerId { get; set; }
    public virtual User? Seller { get; set; }
    [Required]
    [DataType(DataType.Currency)]
    public float? ProductPrice { get; set; }
    public DateTime? ProductAddedDate { get; set; }
    [Required]
    public bool ProductWarranty { get; set; }
    [Required]
    public string? ProductDescription { get; set; }
    [Required]
    public int? ProductQuantity { get; set; }
    public string? ProductImages { get; set; }
    public bool IsAccepted { get; set; } = false;
    public virtual ICollection<Review>? Reviews { get; set; }
    public virtual ICollection<FavouriteProduct>? FavouriteProducts { get; set; }
}
