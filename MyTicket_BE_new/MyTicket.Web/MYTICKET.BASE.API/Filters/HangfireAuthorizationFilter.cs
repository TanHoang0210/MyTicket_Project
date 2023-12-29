using Hangfire.Dashboard;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using MYTICKET.UTILS;
using MYTICKET.UTILS.ConstantVariables.User;
using MYTICKET.WEB.SERVICE.Shared.Authorization;

namespace MYTICKET.BASE.API.Filters
{
    public class HangfireAuthorizationFilter : IDashboardAuthorizationFilter
    {
        public bool Authorize(DashboardContext context)
        {
            var cookie = context.GetHttpContext().AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme).Result;
            if (cookie.Succeeded && cookie.Principal.Claims.Any(e => e.Type == UserClaimTypes.UserType && e.Value == UserTypes.ADMIN.ToString()))
            {
                return true;
            }
            context.GetHttpContext().Response.Redirect(AuthenticationPath.AuthenticateLogin);
            return true;
        }
    }
}
