using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MYTICKET.BASE.SERVICE.Common;
using MYTICKET.UTILS;
using MYTICKET.WEB.SERVICE.EventModule.Abstracts;
using MYTICKET.WEB.SERVICE.EventModule.Dtos;
using MYTICKET.WEB.SERVICE.EventModule.Implements;
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
        /// <summary>
        /// thêm sự kiện
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost("create")]
        public APIResponse CreateTicket([FromBody] CreateTicketEventDto input)
        {
            _ticketService.CreateTicketEvent(input);
            return new();
        }

        /// <summary>
        /// Danh sách vé
        /// </summary>
        /// <param name="eventDetailId"></param>
        /// <returns></returns>
        [Authorize]
        [HttpGet("find-all")]
        public APIResponse<List<TicketEventDto>> FindAllTicket([FromQuery] int eventDetailId)
               => new(_ticketService.GetAllTicket(eventDetailId));

        /// <summary>
        /// Thong tin ve
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize]
        [HttpGet("find-by-id")]
        public APIResponse<TicketEventDto> FindTicketById([FromQuery] int id)
               => new(_ticketService.GetTicketById(id));

        /// <summary>
        /// Cập nhật trạng thái loại vé sự kiện
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPut("update-status")]
        public APIResponse UpdateTicketStatus([FromBody] UpdateTicketStatusDto input)
        {
            _ticketService.UpdateTicketStatus(input);
            return new();
        }

        /// <summary>
        /// Danh sasch ve theo loai
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize]
        [HttpGet("list/find-all")]
        public APIResponse<List<TicketDto>> GetListTicketByType([FromQuery] int id)
        {
            return new(_ticketService.GetAllTicketByType(id));
        }
    }
}
