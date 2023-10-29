using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using MYTICKET.UTILS.Localization;

namespace MYTICKET.BASE.API.Filters
{
    public class PermissionFilter : Attribute, IAuthorizationFilter
    {
        private readonly string[] _permissions;
        private IHttpContextAccessor _httpContextAccessor;
        //private IPermissionService _permissionServices;
        private LocalizationBase _localization;

        public PermissionFilter(params string[] permissions)
        {
            _permissions = permissions;
        }

        private void GetServices(AuthorizationFilterContext context)
        {
            _httpContextAccessor = context.HttpContext.RequestServices.GetService(typeof(IHttpContextAccessor)) as IHttpContextAccessor;
            //_permissionServices = context.HttpContext.RequestServices.GetService(typeof(IPermissionService)) as IPermissionService;
            _localization = context.HttpContext.RequestServices.GetService(typeof(LocalizationBase)) as LocalizationBase;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            GetServices(context);
            //var permissions = _permissionServices.GetPermission();
            bool isGrant = false;
            //foreach (var permission in _permissions)
            //{
            //    if (permissions.Contains(permission))
            //    {
            //        isGrant = true;
            //        break;
            //    }
            //}
            if (!isGrant)
            {
                context.Result = new UnauthorizedObjectResult(new { message = _localization.Localize("error_UserNotHavePermission") });
            }
        }
    }
}
