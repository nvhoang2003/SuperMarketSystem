using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SuperMarketSystem.ViewModels;

namespace SuperMarketSystem.Repositories.Interfaces
{
    public interface IAccountRepository
    {
         //Task<string> LoginAsync(ApplicationUser user);
         Task<IdentityResult> RegisterAsync(ApplicationUser user);
    }
}
