using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using MYTICKET.BASE.SERVICE;
using MYTICKET.WEB.Infrastructure.Persistence;

namespace MYTICKET.WEB.SERVICE.Common
{
    public abstract class ServiceBase : ServiceBase<MyTicketDbContext>
    {
        protected ServiceBase(ILogger logger, IHttpContextAccessor httpContext) : base(logger, httpContext)
        {
        }
    }
}
