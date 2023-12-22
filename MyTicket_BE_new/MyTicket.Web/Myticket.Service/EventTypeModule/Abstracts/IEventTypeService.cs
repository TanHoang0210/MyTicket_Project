using MYTICKET.BASE.SERVICE.Common;
using MYTICKET.WEB.SERVICE.EventTypeModule.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MYTICKET.WEB.SERVICE.EventTypeModule.Abstracts
{
    public interface IEventTypeService
    {
        /// <summary>
        /// Thêm loại sự kiện
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        EventTypeDto Create(CreateEventTypeDto input);
        /// <summary>
        /// Danh sách loại sự kiện
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        PagingResult<EventTypeDto> FindAll(FitlerEventTypeDto input);

        /// <summary>
        /// Thông tin chi tiết loại sự kiện
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        EventTypeDto FindById(int id);

        /// <summary>
        /// Cập nhật thông tin loại
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        void Update(UpdateEventTypeDto input);

    }
}
