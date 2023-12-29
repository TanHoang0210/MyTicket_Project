using MYTICKET.BASE.SERVICE.Common;
using MYTICKET.WEB.SERVICE.NotificationModule.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MYTICKET.WEB.SERVICE.NotificationModule.Abstracts
{
    public interface INotificationService
    {
        /// <summary>
        /// Cac thong bao chua doc
        /// </summary>
        /// <returns></returns>
        int CountNotificationNotSeen();

        /// <summary>
        /// Danh sach thong bao
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        PagingResult<NotificationDto> GetAllNotifications(FilterNotificationDto input);
    }
}
