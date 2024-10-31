using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ManagementSystem.Models
{
    public class Purchase
    {
        [Key]
        public Guid PurchaseId { get; set; } = Guid.NewGuid();

        [ForeignKey("SupplierId")]
        [Required(ErrorMessage = "Supplier is required.")]
        public Guid SupplierId { get; set; }
        public Supplier? Supplier { get; set; }

        [Required(ErrorMessage = "Purchase date is required.")]
        public DateTime PurchaseDate { get; set; }

        public ICollection<PurchaseProduct>? PurchaseProducts { get; set; }
    }
}
