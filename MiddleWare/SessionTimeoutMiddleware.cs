using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

public class SessionTimeoutMiddleware
{
    private readonly RequestDelegate _next;

    public SessionTimeoutMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        var sessionTimeout = context.Session.GetString("SessionTimeout");
        if (!string.IsNullOrEmpty(sessionTimeout) && DateTime.TryParse(sessionTimeout, out DateTime timeout))
        {
            if (DateTime.Now > timeout)
            {
                // Session đã timeout, thực hiện đăng xuất
                context.Response.Redirect("/Admin/Logout"); // Điều hướng đến action đăng xuất trong controller Admin
                return;
            }
        }

        await _next(context);
    }
}
