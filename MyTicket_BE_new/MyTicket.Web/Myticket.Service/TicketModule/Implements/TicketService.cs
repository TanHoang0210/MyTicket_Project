using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using MYTICKET.WEB.SERVICE.Common;
using MYTICKET.WEB.SERVICE.TicketModule.Abstracts;

namespace MYTICKET.WEB.SERVICE.TicketModule.Implements
{
    public class TicketService : ServiceBase, ITicketService
    {
        public TicketService(ILogger<TicketService> logger, IHttpContextAccessor httpContext) : base(logger, httpContext)
        {
        }
    }
}
