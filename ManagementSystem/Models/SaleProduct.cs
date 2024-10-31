using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ManagementSystem.Models
{
    public class SaleProduct
    {
        [Key]
        public Guid SaleProductId { get; set; }

        [ForeignKey("SaleId")]
        [Required(ErrorMessage = "Sale is required.")]
        public int SaleId { get; set; }
        public Sale? Sale { get; set; }

        [ForeignKey("ProductId")]
        [Required(ErrorMessage = "Product is required.")]
        public Guid ProductId { get; set; }
        public Product? Product { get; set; }

        [Required(ErrorMessage = "Sale product quantity is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Sale product quantity must be at least 1.")]
        public int SaleProductQuantity { get; set; }
    }
}
