using SuperMarketSystem.Models;
using System.ComponentModel.DataAnnotations;

namespace SuperMarketSystem.DTOs
{
    public class CustomerDTO
    {
        public int Id { get; set; }

        [Required]
        public Guid CustomerCode { get; set; }

        public string UserId { get; set; }

        public virtual ApplicationUser User { get; set; }

        public int CustomerInfoId { get; set; }

        public virtual CustomerInfo CustomerInfo { get; set; }

        public int CustomerAddressId { get; set; }

        public virtual CustomerAddress Address { get; set; }
    }
    public class CreateCustomerDtoInfo 
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 2)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 2)]
        public string LastName { get; set; }

        [Required]
        [Phone]
        public string PhoneNumber { get; set; }

    }
    public class CreateCustomerDtoAddress
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Street { get; set; }

        [Required]
        [StringLength(50)]
        public string City { get; set; }

        [Required]
        [StringLength(50)]
        public string State { get; set; }

        [Required]
        [StringLength(50)]
        public string Country { get; set; }

    }

}
