using Microsoft.AspNetCore.Authentication.Facebook;
using Microsoft.AspNetCore.Authentication;
using System.Security.Policy;
using DAPM.Interfaces;

namespace DAPM.StrategyConcreteClasses
{
    public class FacebookAuthenticationStrategy : IAuthenticationStrategy
    {
        public async Task ChallengeAsync(HttpContext httpContext, string redirectUri)
        {
            await httpContext.ChallengeAsync(FacebookDefaults.AuthenticationScheme, new AuthenticationProperties { RedirectUri = redirectUri });
        }
    }
}
