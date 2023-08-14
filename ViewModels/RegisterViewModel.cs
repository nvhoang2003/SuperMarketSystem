using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace SuperMarketSystem.ViewModels
{
    public class RegisterViewModel
    {
        [BindProperty]
        public InputModel Input { get; set; }


        // Xác thực từ dịch vụ ngoài (Googe, Facebook ... bài này chứa thiết lập)
        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        // Lớp InputModel chứa thông tin Post tới dùng để tạo User
        public class InputModel
        {
            [Required]
            [EmailAddress]
            [Display(Name = "Địa chỉ Email")]
            public string Email { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "{0} dài từ {2} đến {1} ký tự.", MinimumLength = 3)]
            [DataType(DataType.Password)]
            [Display(Name = "Mật khẩu")]

            public string Password { get; set; }
            [Required]
            [DataType(DataType.Text)]
            [Display(Name = "Phone Number")]
            public string PhoneNumber { get; set; }
            [DataType(DataType.Password)]
            [Display(Name = "Nhập lại mật khẩu")]
            [Compare("Password", ErrorMessage = "Mật khẩu không giống nhau")]
            public string ConfirmPassword { get; set; }

        }
    }
}
