using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using MYTICKET.WEB.SERVICE.Common;
using MYTICKET.WEB.SERVICE.EventModule.Abstracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MYTICKET.WEB.SERVICE.EventModule.Implements
{
    public class EventService : ServiceBase, IEventService
    {
        public EventService(ILogger<EventService> logger, IHttpContextAccessor httpContext) : base(logger, httpContext)
        {
        }
    }
}
