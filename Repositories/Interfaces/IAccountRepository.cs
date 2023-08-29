using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SuperMarketSystem.ViewModels;

namespace SuperMarketSystem.Repositories.Interfaces
{
    public interface IAccountRepository
    {
         Task<IdentityResult> RegisterAsync(ApplicationUser user);
    }
}
