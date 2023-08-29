using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using SuperMarketSystem.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Claims;

public class ApplicationUser : IdentityUser
{
    public string RoleType { get; set; }
}
