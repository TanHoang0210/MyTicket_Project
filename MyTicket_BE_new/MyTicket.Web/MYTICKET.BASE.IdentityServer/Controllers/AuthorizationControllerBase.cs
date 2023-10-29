using Microsoft.AspNetCore.Mvc;
using OpenIddict.Abstractions;

namespace MYTICKET.BASE.IdentityServer.Controllers
{
    public abstract class AuthorizationControllerBase : Controller
    {
        protected readonly IOpenIddictApplicationManager _applicationManager;

        public AuthorizationControllerBase(IOpenIddictApplicationManager applicationManager)
        {
            _applicationManager = applicationManager;
        }
    }
}
