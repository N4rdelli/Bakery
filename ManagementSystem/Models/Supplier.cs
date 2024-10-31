using System.ComponentModel.DataAnnotations;

namespace ManagementSystem.Models
{
    public class Supplier
    {
        [Key]
        public Guid SupplierId { get; set; } = Guid.NewGuid();

        [Required(ErrorMessage = "Supplier company name is required.")]
        [MaxLength(100, ErrorMessage = "Supplier company name cannot exceed 100 characters.")]
        public string SupplierCompanyName { get; set; }

        [Required(ErrorMessage = "Supplier CNPJ is required.")]
        [StringLength(14, MinimumLength = 14, ErrorMessage = "Invalid CNPJ. Please enter a valid 14-digit CNPJ.")]
        public string SupplierCnpj { get; set; }

        [Required(ErrorMessage = "Supplier address is required.")]
        [MaxLength(255, ErrorMessage = "Supplier address cannot exceed 255 characters.")]
        public string SupplierAddress { get; set; }

        [Required(ErrorMessage = "Supplier phone is required.")]
        [RegularExpression(@"^\d{10,11}$", ErrorMessage = "Invalid phone number. Please enter a valid 10 or 11-digit phone number.")]
        public string SupplierPhone { get; set; }

        [Required(ErrorMessage = "Supplier email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address. Please enter a valid email address.")]
        public string SupplierEmail { get; set; }
    }
}
