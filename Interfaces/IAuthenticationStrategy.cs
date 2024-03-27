namespace DAPM.Interfaces
{
    public interface IAuthenticationStrategy
    {
        Task ChallengeAsync(HttpContext httpContext, string redirectUri);
    }
}
