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
    public class AccountController : Controller
    {
        #region Fields
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger<LoginViewModel> _loggerLogin;
        private readonly ILogger<RegisterViewModel> _loggerRegister;
        private readonly ILogger<ExternalLoginViewModel> _loggerExternalLogin;
        private readonly ILogger<LoginWith2faViewModel> _loggerWith2fa;
        private readonly ILogger<LogoutViewModel> _loggerLogout;
        private readonly ILogger<LoginWithRecoveryCodeModel> _loggerLoginWithRecoveryCode;
        private readonly ILogger<LogoutModel> _loggerLockout;
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
                                ILogger<LoginWith2faViewModel> loggerLoginWith2fa,
                                ILogger<LoginWithRecoveryCodeModel> loggerLoginWithRecoveryCode,
                                ILogger<LogoutModel> loggerLockout,
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
            _loggerWith2fa = loggerLoginWith2fa;
            _loggerLoginWithRecoveryCode = loggerLoginWithRecoveryCode;
            _loggerLockout = loggerLockout;
            _emailSender = emailSender;
            _mapper = mapper;
            _userStore = userStore;
            //_emailStore = GetEmailStore();            
        }
        #endregion
        #region ManageProfile
        [Authorize]
        public IActionResult ManagerProfile()
        {

            var user = _userManager.GetUserAsync(User).Result;

            var model = new ManageProfileViewModel
            {
                UserId = user.Id,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
            };

            return View(model);
        }
        #endregion
        #region Register
        [AllowAnonymous]
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
        [AllowAnonymous]
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
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    // Link trong email người dùng bấm vào, nó sẽ gọi Page: /Acount/ConfirmEmail để xác nhận
                    var callbackUrl = Url.Action(
                     "ConfirmEmail",
                     "Account",
                     new { userId = user.Id, code = code },
                     protocol: Request.Scheme);
                    // Gửi email    
                    await _emailSender.SendEmailAsync(model.Input.Email,  "Xác nhận địa chỉ email",
                        $"Hãy xác nhận địa chỉ email bằng cách <a href='{callbackUrl}'>Bấm vào đây</a>.");
                    await _userManager.AddToRoleAsync(user, "Customer");
                    if (!_userManager.Options.SignIn.RequireConfirmedEmail)
                    {
                        return RedirectToAction("RegisterConfirmation","Account", new { email = model.Input.Email});
                    }
                    else
                    {
                        // Không cần xác thực - đăng nhập luôn
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return RedirectToAction("Index","Home");
                    }
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                
            }
            return View(model);
        }
        #endregion
        #region RegisterConfirmation
        [AllowAnonymous]
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
        #region Confirm EmailChange
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
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
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
        #region ResendEmailConfirm
        [AllowAnonymous]
        [HttpGet]
        public IActionResult ResendEmailConfirmation()
        {
            var model = new ResendEmailConfirmationViewModel();
            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResendEmailConfirmation(ResendEmailConfirmationViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "Verification email sent. Please check your email.");
                return View(model);
            }

            var userId = await _userManager.GetUserIdAsync(user);
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            var callbackUrl = Url.Action(
                     "ConfirmEmail",
                     "Account",
                     new { userId = user.Id, code = code },
                     protocol: Request.Scheme);
            await _emailSender.SendEmailAsync(
                model.Email,
                "Confirm your email",
                $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

            ModelState.AddModelError(string.Empty, "Verification email sent. Please check your email.");
            return View(model);
        }
        #endregion
        #region ResetPassword
        [Authorize]
        public IActionResult ResetPassword(string code = null)
        {
            if (code == null)
            {
                return BadRequest("A code must be supplied for password reset.");
            }
            else
            {
                var model = new ResetPasswordViewModel();
                model.Input = new ResetPasswordViewModel.InputModel
                {
                    Code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code))
                };
                return View(model);
            }
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await _userManager.FindByEmailAsync(model.Input.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return RedirectToAction("ResetPasswordConfirmation","Account");
            }
            var result = await _userManager.ResetPasswordAsync(user, model.Input.Code,  model.Input.Password);
            if (result.Succeeded)
            {
                return RedirectToAction("ResetPasswordConfirmation","Account");
            }
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
            return View(model);
        }
        #endregion
        #region ResetPasswordConfirm
        [Authorize]
        [HttpGet]
        public IActionResult ResetPasswordConfirm()
        {
            var model = new ResetPasswordConfirmViewModel();
            return View(model);
        }
        #endregion
        #region ForgotPassword
        [AllowAnonymous]
        public IActionResult ForgotPassword()
        {
            var model = new ForgotPasswordViewModel();
            return View(model);
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {

            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Input.Email);
                if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    return RedirectToAction("ForgotPasswordConfirmation","Account");
                }
                // For more information on how to enable account confirmation and password reset please
                var code = await _userManager.GeneratePasswordResetTokenAsync(user);
                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                var callbackUrl = Url.Action(
                      "ResetPassword",
                      "Account",
                      new { userId = user.Id, code = code },
                      protocol: Request.Scheme);
                await _emailSender.SendEmailAsync(
                    model.Input.Email,
                    "Reset Password",
                    $"Please reset your password by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");
                return RedirectToAction("ForgotPasswordConfirmation","Account");
            }
            return View(model);
        }

        [AllowAnonymous]
        public IActionResult ForgotPasswordConfirmation()
        {
            var model = new ForgotPasswordConfirmationViewModel();
            return View(model);
        }
        #endregion
        #region Login
        [HttpGet]
        [AllowAnonymous]
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
        [AllowAnonymous]
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
        #region LoginWith2Fa
        [Authorize]
        public async Task<IActionResult> LoginWith2fa(bool rememberMe, string returnUrl = null)
        {
            // Ensure the user has gone through the username & password screen first
            var user = await _signInManager.GetTwoFactorAuthenticationUserAsync();

            if (user == null)
            {
                throw new InvalidOperationException($"Unable to load two-factor authentication user.");
            }
            var model = new LoginWith2faViewModel()
            {
                ReturnUrl = returnUrl,
                RememberMe = rememberMe
            };
            return View(model);
        }
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LoginWith2fa(LoginWith2faViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            model.ReturnUrl = model.ReturnUrl ?? Url.Content("~/");

            var user = await _signInManager.GetTwoFactorAuthenticationUserAsync();
            if (user == null)
            {
                throw new InvalidOperationException($"Unable to load two-factor authentication user.");
            }

            var authenticatorCode = model.Input.TwoFactorCode.Replace(" ", string.Empty).Replace("-", string.Empty);

            var result = await _signInManager.TwoFactorAuthenticatorSignInAsync(authenticatorCode, model.RememberMe, model.Input.RememberMachine);

            var userId = await _userManager.GetUserIdAsync(user);

            if (result.Succeeded)
            {
                _loggerWith2fa.LogInformation("User with ID '{UserId}' logged in with 2fa.", user.Id);
                return LocalRedirect(model.ReturnUrl);
            }
            else if (result.IsLockedOut)
            {
                _loggerWith2fa.LogWarning("User with ID '{UserId}' account locked out.", user.Id);
                return RedirectToAction("Lockout","Account");
            }
            else
            {
                _loggerWith2fa.LogWarning("Invalid authenticator code entered for user with ID '{UserId}'.", user.Id);
                ModelState.AddModelError(string.Empty, "Invalid authenticator code.");
                return View(model);
            }
        }
        #endregion
        #region LoginWithRecoveryCode
        [Authorize]
        public async Task<IActionResult> LoginWithRecoveryCode(string returnUrl = null)
        {
            // Ensure the user has gone through the username & password screen first
            var user = await _signInManager.GetTwoFactorAuthenticationUserAsync();
            if (user == null)
            {
                throw new InvalidOperationException($"Unable to load two-factor authentication user.");
            }
            var model = new LoginWithRecoveryCodeViewModel()
            {
                ReturnUrl = returnUrl,
            };
            return View(model);
        }
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LoginWithRecoveryCode(LoginWithRecoveryCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _signInManager.GetTwoFactorAuthenticationUserAsync();
            if (user == null)
            {
                throw new InvalidOperationException($"Unable to load two-factor authentication user.");
            }

            var recoveryCode = model.Input.RecoveryCode.Replace(" ", string.Empty);

            var result = await _signInManager.TwoFactorRecoveryCodeSignInAsync(recoveryCode);

            var userId = await _userManager.GetUserIdAsync(user);

            if (result.Succeeded)
            {
                _loggerLoginWithRecoveryCode.LogInformation("User with ID '{UserId}' logged in with a recovery code.", user.Id);
                return LocalRedirect(model.ReturnUrl ?? Url.Content("~/"));
            }
            if (result.IsLockedOut)
            {
                _loggerLoginWithRecoveryCode.LogWarning("User account locked out.");
                return RedirectToAction("Lockout","Account");
            }
            else
            {
                _loggerLoginWithRecoveryCode.LogWarning("Invalid recovery code entered for user with ID '{UserId}' ", user.Id);
                ModelState.AddModelError(string.Empty, "Invalid recovery code entered.");
                return View(model);
            }
        }
        #endregion
        #region Lockout
        [AllowAnonymous]
        public IActionResult Lockout()
        {
            var model = new LockoutViewModel();
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

        #region ExternalLoginCallback
        [AllowAnonymous]
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
        [AllowAnonymous]
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
        [AllowAnonymous]
        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }
        #endregion
    }
}


