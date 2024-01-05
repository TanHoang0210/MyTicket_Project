using MYTICKET.BASE.SERVICE.Common;
using MYTICKET.WEB.SERVICE.EventModule.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MYTICKET.WEB.SERVICE.EventModule.Abstracts
{
    public interface IEventService
    {
        /// <summary>
        /// Tạo sự kiện
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        void CreateEvent(CreateEventDto input);

        /// <summary>
        /// Tạo các sự kiện diễn ra
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        void CreateEventDetail(CreateEventDetailDto input);

        /// <summary>
        /// Cập nhật sự kiện
        /// </summary>
        /// <param name="input"></param>
        void UpdateEvent(UpdateEventDto input);

        /// <summary>
        /// Cập nhật chi tiết
        /// </summary>
        /// <param name="input"></param>
        void UpdateEventDetail(UpdateEventDetailDto input);

        /// <summary>
        /// danh sách sự kiện có phân trang
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        PagingResult<EventDto> FindAll(FilterEventDto input);

        /// <summary>
        /// danh sách sự kiện mới nhất
        /// </summary>
        List<EventDto> FindAllNewEvent();

        /// <summary>
        /// danh sách sự kiện nổi bật
        /// </summary>
        List<EventDto> FindAllOutStandingEvent();

        /// <summary>
        /// danh sách sự kiện bán chạy nhất
        /// </summary>
        List<EventDto> FindAllTopSaleEvent();

        /// <summary>
        /// cthông tin sự kiện
        /// </summary>
        /// <param name="eventId"></param>
        /// <returns></returns>
        EventDto GetEventById(int eventId);

        /// <summary>
        /// lấy thông tin 1 chi tiết sự kiện theo id
        /// </summary>
        /// <param name="eventDetailId"></param>
        /// <returns></returns>
        EventDetailDto GetEventDetailTicketById(int eventDetailId);

        /// <summary>
        /// Cập nhật trạng thái sự kiện
        /// </summary>
        /// <param name="input"></param>
        void UpdateEventStatus(UpdateEventStatus input);

        /// <summary>
        /// Cập nhật trạng thái chi tiết sự kiện
        /// </summary>
        /// <param name="input"></param>
        void UpdateEventDetailStatus(UpdateEventDetailStatus input);

        /// <summary>
        /// cthông tin sự kiện
        /// </summary>
        /// <param name="eventId"></param>
        /// <returns></returns>
        EventDto GetEventByIdAdmin(int eventId);

        /// <summary>
        /// danh sách sự kiện có phân trang theo nhà cc
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        PagingResult<EventDto> FindAllEventBySupplier(FilterEventDto input);


        /// <summary>
        /// trạng thái nổi bật
        /// </summary>
        /// <param name="input"></param>
        void UpdateEventOutStanding(UpdateEventStatus input);

    }
}
