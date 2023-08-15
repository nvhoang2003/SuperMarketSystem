using System.ComponentModel.DataAnnotations;

namespace SuperMarketSystem.ViewModels.AccountViewModel
{

    public class ExternalLoginConfirmationViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Username")]
        public string UserName { get; set; }
    }
}
