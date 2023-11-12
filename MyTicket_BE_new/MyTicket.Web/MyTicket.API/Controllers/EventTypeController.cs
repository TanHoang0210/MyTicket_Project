using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MYTICKET.BASE.SERVICE.Common;
using MYTICKET.UTILS;
using MYTICKET.WEB.SERVICE.EventModule.Abstracts;
using MYTICKET.WEB.SERVICE.EventTypeModule.Abstracts;
using MYTICKET.WEB.SERVICE.EventTypeModule.Dtos;
using MYTICKET.WEB.SERVICE.VenueModule.Dtos;

namespace MYTICKET.WEB.API.Controllers
{
    [Authorize]
    [Route("myticket/api/event-type")]
    [ApiController]
    public class EventTypeController : ControllerBase
    {
        private readonly IEventService _eventService;
        private readonly IEventTypeService _eventTypeService;

        public EventTypeController(IEventService eventService, IEventTypeService eventTypeService)
        {
            _eventService = eventService;
            _eventTypeService = eventTypeService;
        }
        /// <summary>
        /// lây danh sách loại sự kiện
        /// </summary>
        [HttpGet("find")]
        public APIResponse<List<EventTypeDto>> FindProduct()
               => new(_eventTypeService.FindAll());

        /// <summary>
        /// thêm loại sự kiện
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("create")]
        public APIResponse CreateVenue([FromBody] CreateEventTypeDto input)
        {
            _eventTypeService.Create(input);
            return new();
        }
    }
}
