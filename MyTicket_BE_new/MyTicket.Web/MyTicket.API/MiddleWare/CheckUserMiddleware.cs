using MYTICKET.UTILS.ConstantVariables.User;
using MYTICKET.UTILS.Localization;
using MYTICKET.UTILS;
using System.Net;
using MYTICKET.UTILS.ConstantVariables.Shared;
using MYTICKET.WEB.Infrastructure.Persistence;

namespace MYTICKET.WEB.API.MiddleWare
{
    /// <summary>
    /// Check user kế thừa từ WebAPIBase.Middlewares.CheckUserMiddleware
    /// </summary>
    public class CheckUserMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly MapErrorCodeBase _mapErrorCode;

        public CheckUserMiddleware(RequestDelegate next, MapErrorCodeBase mapErrorCode)
        {
            _next = next;
            _mapErrorCode = mapErrorCode;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var claim = context.User.FindFirst(UserClaimTypes.UserId);
            var dbContext = context.RequestServices.GetService<MyTicketDbContext>();

            if (claim != null)
            {
                int userId = int.Parse(claim.Value);
                var user = dbContext.Users.Select(u => new
                {
                    u.Id,
                    u.Status
                }).FirstOrDefault(u => u.Id == userId);
                if (user.Status != UserStatus.ACTIVE)
                {
                    context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                    await context.Response.WriteAsJsonAsync(new APIResponse(StatusCode.Error, string.Empty,
                        (int)ErrorCode.UserIsDeactive, _mapErrorCode.GetErrorMessage(ErrorCode.UserIsDeactive)));
                    return;
                }
            }
            await _next(context);
        }
    }

    /// <summary>
    /// Extension check user middleware
    /// </summary>
    public static class CheckUserMiddlewareExtensions
    {
        public static IApplicationBuilder UseCheckUser(
            this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<CheckUserMiddleware>();
        }
    }
}
