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
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        EventDto CreateEvent(CreateEventDto input);

        /// <summary>
        /// danh sách sự kiện có phân trang
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        PagingResult<EventDto> FindAll(FilterEventDto input);

        /// <summary>
        /// chi tiết sự kiện 
        /// </summary>
        /// <param name="eventDetailId"></param>
        /// <returns></returns>
        EventDetailDto GetEventById(int eventDetailId);
    }
}
