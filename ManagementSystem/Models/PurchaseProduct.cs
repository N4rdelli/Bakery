using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ManagementSystem.Models
{
    public class PurchaseProduct
    {
        [Key]
        public Guid PurchaseProductId { get; set; } = Guid.NewGuid();

        [ForeignKey("PurchaseId")]
        [Required(ErrorMessage = "Report a purchase.")]
        public Guid PurchaseId { get; set; }
        public Purchase? Purchase { get; set; }

        [ForeignKey("ProductId")]
        [Required(ErrorMessage = "Product is required.")]
        public Guid ProductId { get; set; }
        public Product? Product { get; set; }

        [Required(ErrorMessage = "Purchase product quantity is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Purchase product quantity must be at least 1.")]
        public int PurchaseProductQuantity { get; set; }

        [Required(ErrorMessage = "Purchase product price is required.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Purchase product price must be greater than zero.")]
        public decimal PurchaseProductPrice { get; set; }
    }
}
