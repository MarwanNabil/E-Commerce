namespace Ecommerce_Software_Project.Models;

public class Review
{
    [Key]
    public int Id { get; set; }
    [Required]
    public string? Description { get; set; }
    [Required]
    public int? rate { get; set; }
    [ForeignKey("Product")]
    public int? ProductId { get; set; }
    public virtual Product? Product { get; set; }
    [ForeignKey("User")]
    public int? UserId { get; set; }
    public virtual User? User { get; set; }
    public DateTime? Date { get; set; }
}
