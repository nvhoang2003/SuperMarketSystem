using System.ComponentModel.DataAnnotations;

namespace SuperMarketSystem.Models
{
    public class CustomerInfo
    {
        [Key]
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string PhoneNumber { get; set; }
    }
}
