using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ManagementSystem.Models
{
    public class Product
    {
        [Key]
        public Guid ProductId { get; set; } = Guid.NewGuid();

        [Required(ErrorMessage = "Product name is required.")]
        [MaxLength(100, ErrorMessage = "Product name cannot exceed 100 characters.")]
        public string ProductName { get; set; }

        [Required(ErrorMessage = "Product description is required.")]
        [MaxLength(500, ErrorMessage = "Product description cannot exceed 500 characters.")]
        public string ProductDescription { get; set; }

        [Required(ErrorMessage = "Product price is required.")]
        [DataType(DataType.Currency)]
        [Range(0.00, double.MaxValue, ErrorMessage = "Product price must be greater than zero.")]
        public decimal ProductPrice { get; set; }

        [Required(ErrorMessage = "Product stock quantity is required.")]
        [Range(0, int.MaxValue, ErrorMessage = "Product stock quantity must be a non-negative integer.")]
        public int ProductStockQuantity { get; set; }

        [ForeignKey("SupplierId")]
        [Required(ErrorMessage = "Supplier is required.")]
        public Guid? SupplierId { get; set; }
        public Supplier? Supplier { get; set; }
    }
}
