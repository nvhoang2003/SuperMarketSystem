using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Sprache;
using SuperMarketSystem.ViewModels.AccountViewModel;
using System.Text.Encodings.Web;
using System.Text;
using Microsoft.AspNetCore.Identity.UI.Services;
using SuperMarketSystem.DTOs;
using AutoMapper;
using SuperMarketSystem.Services;
using Newtonsoft.Json;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.Cookies;
using MimeKit;
using SuperMarketSystem.Services.EmailService;
using Microsoft.AspNetCore.Identity.UI.V4.Pages.Account.Internal;

namespace SuperMarketSystem.Controllers
{
    [AllowAnonymous]
    public class AccountController : Controller
    {
        #region Fields
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger<LoginViewModel> _loggerLogin;
        private readonly ILogger<RegisterViewModel> _loggerRegister;
        private readonly ILogger<ExternalLoginViewModel> _loggerExternalLogin;
        private readonly ILogger<LogoutViewModel> _loggerLogout;
        private readonly IUserStore<ApplicationUser> _userStore;
        private readonly IUserEmailStore<ApplicationUser> _emailStore;
        private readonly IEmailService _emailSender;
        private readonly IMapper _mapper;
        #endregion
        #region Cter
        public AccountController(UserManager<ApplicationUser> userManager,
                                 SignInManager<ApplicationUser> signInManager,
                                ILogger<LoginViewModel> loggerLogin,
                                ILogger<RegisterViewModel> loggerRegister,
                                ILogger<ExternalLoginViewModel> loggerExternalLogin,
                                ILogger<LogoutViewModel> loggerLogout,
                                IUserStore<ApplicationUser> userStore,
                                IEmailService emailSender,                              
                                 IMapper mapper)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _loggerLogin = loggerLogin;
            _loggerRegister = loggerRegister;
            _loggerExternalLogin = loggerExternalLogin;
            _loggerLogout = loggerLogout;
            _emailSender = emailSender;
            _mapper = mapper;
            _userStore = userStore;
            //_emailStore = GetEmailStore();            
        }
        #endregion
        #region RegisterMethod
        public async Task<IActionResult> Register()
        {
            // Đăng ký tài khoản theo dữ liệu form post tới
            var externalSchemes = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            var model = new RegisterViewModel
            {
                ExternalLogins = externalSchemes,
            };
            var externalSchemesJson = JsonConvert.SerializeObject(externalSchemes);
            HttpContext.Session.SetString("ExternalSchemes", externalSchemesJson);
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            model.ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (ModelState.IsValid)
            {
                // Tạo ApplicationUser sau đó tạo User mới (cập nhật vào db)
                var user = new ApplicationUser { UserName = model.Input.Email, Email = model.Input.Email, PhoneNumber = model.Input.PhoneNumber};
                var result = await _userManager.CreateAsync(user, model.Input.Password);

                if (result.Succeeded)
                {
                    _loggerRegister.LogInformation("Vừa tạo mới tài khoản thành công.");

                    // phát sinh token theo thông tin user để xác nhận email
                    // mỗi user dựa vào thông tin sẽ có một mã riêng, mã này nhúng vào link
                    // trong email gửi đi để người dùng xác nhận
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

                    // callbackUrl = /Account/ConfirmEmail?userId=useridxx&code=codexxxx
                    // Link trong email người dùng bấm vào, nó sẽ gọi Page: /Acount/ConfirmEmail để xác nhận
                    var callbackUrl = Url.Action(
                     "ConfirmEmail",
                     "Account",
                     new { userId = user.Id, code = code },
                     protocol: Request.Scheme);
                    // Gửi email    
                    await _emailSender.SendEmailAsync(model.Input.Email,  "Xác nhận địa chỉ email",
                        $"Hãy xác nhận địa chỉ email bằng cách <a href='{callbackUrl}'>Bấm vào đây</a>.");

                    if (!_userManager.Options.SignIn.RequireConfirmedEmail)
                    {
                        // Nếu cấu hình phải xác thực email mới được đăng nhập thì chuyển hướng đến trang
                        // RegisterConfirmation - chỉ để hiện thông báo cho biết người dùng cần mở email xác nhận
                        return RedirectToAction("RegisterConfirmation","Account", new { email = model.Input.Email});
                    }
                    else
                    {
                        // Không cần xác thực - đăng nhập luôn
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return RedirectToAction("Index","Home");
                    }
                }
                // Có lỗi, đưa các lỗi thêm user vào ModelState để hiện thị ở html helpper: asp-validation-summary
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                
            }
            return View(model);
        }
        #endregion
        #region RegisterConfirmation
        public async Task<IActionResult> RegisterConfirmation(string email)
        {
              if (email == null)
            {
                return RedirectToAction("Register", "Account");
            }
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return NotFound($"Unable to load user with email '{email}'.");
            }
            var model = new RegisterConfirmationViewModel()
            {
                Email = email,
            };
            return View(model);
        }
        #endregion
        #region Confirm Email
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return RedirectToAction("Error", "Home"); // Hoặc chuyển hướng đến trang báo lỗi nếu cần
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return RedirectToAction("Error", "Home"); // Hoặc chuyển hướng đến trang báo lỗi nếu cần
            }

            code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
            var result = await _userManager.ConfirmEmailAsync(user, code);

            if (result.Succeeded)
            {
                // Xác thực email thành công, bạn có thể chuyển hướng đến trang thông báo thành công hoặc trang đăng nhập
                return View("ConfirmEmailSuccess"); // Tạo view ConfirmEmailSuccess để hiển thị thông báo thành công
            }
            else
            {
                return View("ConfirmEmailFailure"); // Tạo view ConfirmEmailFailure để hiển thị thông báo thất bại
            }
        }
        #endregion
        #region EmailChange     
        public async Task<IActionResult> ConfirmEmailChange(string userId, string email, string code)
        {
            if (userId == null || email == null || code == null)
            {
                return RedirectToAction("Manager","Account");
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{userId}'.");
            }

            code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
            var model = new ConfirmEmailChangeViewModel();
            var result = await _userManager.ChangeEmailAsync(user, email, code);
            if (!result.Succeeded)
            {
                model.StatusMessage = "Error changing email.";
                return View(model);
            }

            // In our UI email and user name are one and the same, so when we update the email
            // we need to update the user name.
            var setUserNameResult = await _userManager.SetUserNameAsync(user, email);
            if (!setUserNameResult.Succeeded)
            {
                model.StatusMessage = "Error changing user name.";
                return View(model);
            }

            await _signInManager.RefreshSignInAsync(user);
            model.StatusMessage = "Thank you for confirming your email change.";
            return View(model);
        }
        #endregion
        #region LoginMethod
        [HttpGet]
        public async Task<IActionResult> Login()
        {
            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);
            var externalSchemes = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            var model = new LoginViewModel
            {
                ExternalLogins = externalSchemes,
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.Input.Email, model.Input.Password, false, false);

                if (result.Succeeded)
                {
                    _loggerLogin.LogInformation("User logged in.");
                    return RedirectToAction("Index", "Home");
                }
                if (result.RequiresTwoFactor)
                {
                    return RedirectToAction("LoginWith2fa", "Account", new { RememberMe = model.Input.RememberMe });
                }
                if (result.IsLockedOut)
                {
                    _loggerLogin.LogWarning("User account locked out.");
                    return RedirectToAction("Logout", "Account");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    model.ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
                    return View(model);
                }
            }

            ModelState.AddModelError("", "Username or Password was invalid.");
            return View(model);
        }
        #endregion
        #region Logout
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            if (!_signInManager.IsSignedIn(User)) return RedirectToAction("Login","Account");

            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);
            _loggerLogout.LogInformation("Người dùng đăng xuất");
            return RedirectToAction("Login", "Account");
        }
        #endregion
        // Post yêu cầu login bằng dịch vụ ngoài
        // Provider = Google, Facebook ... 
        #region ExternalLogin
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public IActionResult ExternalLogin(string provider, string returnUrl)
        {
            var redirectUrl = Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl });
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            return new ChallengeResult(provider, properties);
        }

        #endregion
        public async Task<IActionResult> ExternalLoginCallback(string returnUrl = null)
        {
            var info = await _signInManager.GetExternalLoginInfoAsync();

            if (info == null)
            {
                // Handle the case where external login info is not available
                return RedirectToAction("Login");
            }

            var result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false, bypassTwoFactor: true);

            if (result.Succeeded)
            {
                // User is successfully authenticated, redirect to the desired page
                if (!string.IsNullOrEmpty(returnUrl))
                {
                    return LocalRedirect(returnUrl);
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            else
            {
                // The user does not have an account or other login failure, handle as needed
                ViewData["ReturnUrl"] = returnUrl;
                return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel
                {
                    Email = info.Principal.FindFirstValue(ClaimTypes.Email)
                });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl = null)
        {
            if (ModelState.IsValid)
            {
                var info = await _signInManager.GetExternalLoginInfoAsync();

                if (info == null)
                {
                    // Handle the case where external login info is not available
                    return RedirectToAction("Login");
                }

                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };

                var result = await _userManager.CreateAsync(user);

                if (result.Succeeded)
                {
                    result = await _userManager.AddLoginAsync(user, info);

                    if (result.Succeeded)
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return RedirectToAction("Index", "Home");
                    }
                }

                // If we reach here, there was an error, add the error to ModelState
                AddErrors(result);
            }

            ViewData["ReturnUrl"] = returnUrl;
            return View(model);
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }
    }
}


