using DataAccessLayer.DataObject;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace SuperMarketSystem.DTOs
{
    public class UserDTO : IdentityUser
    {
        [Required]
        [StringLength(255)]
        public string FirstName { get; set; }
        [Required]
        [StringLength(255)]
        public string LastName { get; set; }
    }
}

