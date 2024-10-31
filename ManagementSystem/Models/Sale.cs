using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ManagementSystem.Models
{
    public class Sale
    {
        [Key]
        public int SaleId { get; set; }

        [ForeignKey("CostumerId")]
        [Required(ErrorMessage = "Customer is required.")]
        public Guid CustomerId { get; set; }
        public Customer? Customer { get; set; }
        public DateTime IssueDate { get; set; }
        public ICollection<SaleProduct>? SaleProducts { get; set; }

        public decimal TotalValue
        {
            get
            {
                return SaleProducts?.Sum(sp => sp.SaleProductQuantity * sp.Product.ProductPrice) ?? 0;
            }
        }
    }
}
