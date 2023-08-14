using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Sprache;
using SuperMarketSystem.Repositories.Interfaces;

namespace SuperMarketSystem.Repositories.Implements
{
    public class AccountRepository : IAccountRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IConfiguration _config;
        public AccountRepository(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IConfiguration config)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _config = config;
        }

        //public async Task<string> LoginAsync(ApplicationUser user)
        //{
        //    var result = await _signInManager.PasswordSignInAsync(user.Email,user.PasswordHash,false,false);
        //    return result;
        //}

        public async Task<IdentityResult> RegisterAsync(ApplicationUser user)
        {
            return await _userManager.CreateAsync(user);
        }
    }
}
