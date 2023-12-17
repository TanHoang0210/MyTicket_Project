using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MYTICKET.BASE.SERVICE.Common;
using MYTICKET.UTILS;
using MYTICKET.WEB.SERVICE.EventModule.Abstracts;
using MYTICKET.WEB.SERVICE.OrderModule.Dtos;
using MYTICKET.WEB.SERVICE.TicketModule.Abstracts;
using MYTICKET.WEB.SERVICE.TicketModule.Dtos;

namespace MYTICKET.WEB.API.Controllers
{
    [Route("myticket/api/ticket")]
    [ApiController]
    public class TicketController : ControllerBase
    {
        private readonly ITicketService _ticketService;

        public TicketController(ITicketService ticketService)
        {
            _ticketService = ticketService;
        }

        /// <summary>
        /// Danh sách vé chuyen nhuong
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [Authorize]
        [HttpGet("find-all-transfer")]
        public APIResponse<PagingResult<TicketEventTransferDto>> FindAllTransferTicket([FromQuery] FilterTicketDto input)
               => new(_ticketService.GetAllTicketTransfer(input));
    }
}
