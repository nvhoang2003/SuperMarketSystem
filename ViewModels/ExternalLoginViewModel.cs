using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace SuperMarketSystem.ViewModels
{
    public class ExternalLoginViewModel
    {
        [BindProperty]
        public InputModel Input { get; set; }
        public string ProviderDisplayName { get; set; }
        public string ReturnUrl { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }
        public class InputModel
        {   
            [Required]
            [EmailAddress]
            [Display(Name = "Địa chỉ email")]
            public string Email { get; set; }
        }

    }
}
