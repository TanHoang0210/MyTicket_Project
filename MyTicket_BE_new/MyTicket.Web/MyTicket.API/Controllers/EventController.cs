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
        [Authorize]
        [HttpPost("create")]
        public APIResponse CreateEvent([FromBody] CreateEventDto input)
        {
            _eventService.CreateEvent(input);
            return new();
        }
        /// <summary>
        /// cap nhat su kien
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPut("update")]
        public APIResponse UpdateEvent([FromBody] UpdateEventDto input)
        {
            _eventService.UpdateEvent(input);
            return new();
        }
        /// <summary>
        /// cap nhat su kien
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPut("detail/update")]
        public APIResponse UpdateEventDetail([FromBody] UpdateEventDetailDto input)
        {
            _eventService.UpdateEventDetail(input);
            return new();
        }
        /// <summary>
        /// thêm sự kiện
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost("create-detail")]
        public APIResponse CreateDetail([FromBody] CreateEventDetailDto input)
        {
            _eventService.CreateEventDetail(input);
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
        /// lây danh sách sự kiện mới nhất
        /// </summary>
        [HttpGet("find-top-new")]
        public APIResponse<List<EventDto>> FindAllNewEvent()
               => new(_eventService.FindAllNewEvent());

        /// <summary>
        /// lây danh sách sự kiện nổi bật
        /// </summary>
        [HttpGet("find-top-standing")]
        public APIResponse<List<EventDto>> FindAllOutStandingEvent()
               => new(_eventService.FindAllOutStandingEvent());

        /// <summary>
        /// lây danh sách bán chạy
        /// </summary>
        [HttpGet("find-top-sale")]
        public APIResponse<List<EventDto>> FindAllTopSaleEvent()
               => new(_eventService.FindAllTopSaleEvent());

        /// <summary>
        /// lấy chi tiết sự kiện
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("find-by-id")]
        public APIResponse<EventDto> FindProduct([FromQuery] int id)
               => new(_eventService.GetEventById(id));

        /// <summary>
        /// lấy 1 chi tiết sự kiện
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("detail/find-by-id")]
        public APIResponse<EventDetailDto> FindEventDetailbyId([FromQuery] int id)
               => new(_eventService.GetEventDetailTicketById(id));

        /// <summary>
        /// cap nhat trang thai chi tiet su kien
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPut("detail/update-status")]
        public APIResponse UpdateEventDetailStatus([FromBody] UpdateEventDetailStatus input)
        {
            _eventService.UpdateEventDetailStatus(input);
            return new();
        }

        /// <summary>
        /// cap nhat trang thai su kien
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPut("update-status")]
        public APIResponse UpdateEventStatus([FromBody] UpdateEventStatus input)
        {
            _eventService.UpdateEventStatus(input);
            return new();
        }

        /// <summary>
        /// lấy chi tiết sự kiệnadmin
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("admin/find-by-id")]
        public APIResponse<EventDto> FindByIdAdmin([FromQuery] int id)
               => new(_eventService.GetEventByIdAdmin(id));

        /// <summary>
        /// lây danh sách sự kiện nha cc
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet("find-by-supplier")]
        public APIResponse<PagingResult<EventDto>> FindAllEventSupplier([FromQuery] FilterEventDto input)
               => new(_eventService.FindAllEventBySupplier(input));

        /// <summary>
        /// cap nhat trang thai noi bat su kien
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPut("update-out-standing")]
        public APIResponse UpdateEventOutStandingStatus([FromBody] UpdateEventStatus input)
        {
            _eventService.UpdateEventOutStanding(input);
            return new();
        }
    }
}
