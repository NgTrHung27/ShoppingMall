using DAPM.Interfaces;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication;
using System.Security.Policy;

namespace DAPM.StrategyConcreteClasses
{
    public class GoogleAuthenticationStrategy : IAuthenticationStrategy
    {
        public async Task ChallengeAsync(HttpContext httpContext, string redirectUri)
        {
            await httpContext.ChallengeAsync(GoogleDefaults.AuthenticationScheme, new AuthenticationProperties {RedirectUri = redirectUri});
        }
    }
}
