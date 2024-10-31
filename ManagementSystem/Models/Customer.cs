using System.ComponentModel.DataAnnotations;

namespace ManagementSystem.Models
{
    public class Customer
    {
        [Key]
        public Guid CustomerId { get; set; } = Guid.NewGuid();

        [Required(ErrorMessage = "Customer name is required.")]
        [MaxLength(100, ErrorMessage = "Customer name cannot exceed 100 characters.")]
        public string CustomerName { get; set; }

        [Required(ErrorMessage = "Customer CPF is required.")]
        [StringLength(11, MinimumLength = 11, ErrorMessage = "Customer CPF must have exactly 11 digits.")]
        public string CustomerCpf { get; set; }

        [Required(ErrorMessage = "Customer phone is required.")]
        [RegularExpression(@"^\d{10,11}$", ErrorMessage = "Invalid phone number. Please enter a valid 10 or 11-digit phone number.")]
        public string CustomerPhone { get; set; }


        [StringLength(14, MinimumLength = 14, ErrorMessage = "Invalid CNPJ. Please enter a valid 14-digit CNPJ.")]
        public string? CustomerCnpj { get; set; }

        [MaxLength(200, ErrorMessage = "CNPJ address cannot exceed 200 characters.")]
        public string? CustomerCnpjAddress { get; set; }
    }
}
