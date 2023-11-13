using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MYTICKET.BASE.SERVICE.Common;
using MYTICKET.UTILS;
using MYTICKET.WEB.SERVICE.EventModule.Abstracts;
using MYTICKET.WEB.SERVICE.EventModule.Dtos;
using MYTICKET.WEB.SERVICE.EventTypeModule.Dtos;
using MYTICKET.WEB.SERVICE.VenueModule.Dtos;

namespace MYTICKET.WEB.API.Controllers
{
    [Authorize]
    [Route("myticket/api/event")]
    [ApiController]
    public class EventController : ControllerBase
    {
        private readonly IEventService _eventService;

        public EventController(IEventService eventService)
        {
            _eventService = eventService;
        }

        /// <summary>
        /// thêm sự kiện
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("create")]
        public APIResponse CreateVenue([FromBody] CreateEventDto input)
        {
            _eventService.CreateEvent(input);
            return new();
        }

        /// <summary>
        /// lây danh sách sự kiện
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet("find")]
        public APIResponse<PagingResult<EventDto>> FindAllEvent([FromQuery] FilterEventDto input)
               => new(_eventService.FindAll(input));

        /// <summary>
        /// lấy chi tiết sự kiện
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("find-by-id")]
        public APIResponse<EventDetailDto> FindProduct(int id)
               => new(_eventService.GetEventById(id));
    }
}
