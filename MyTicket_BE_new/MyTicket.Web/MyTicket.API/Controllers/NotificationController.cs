using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MYTICKET.BASE.SERVICE.Common;
using MYTICKET.UTILS;
using MYTICKET.WEB.SERVICE.NotificationModule.Abstracts;
using MYTICKET.WEB.SERVICE.NotificationModule.Dtos;
using MYTICKET.WEB.SERVICE.OrderModule.Abstracts;
using MYTICKET.WEB.SERVICE.OrderModule.Dtos;
using MYTICKET.WEB.SERVICE.OrderModule.Implements;

namespace MYTICKET.WEB.API.Controllers
{
    [Authorize]
    [Route("myticket/api/notification")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationService _notificationService;

        public NotificationController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        /// <summary>
        /// danh sach thong bao
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet("find-all")]
        public APIResponse<PagingResult<NotificationDto>> FindAllNotification([FromQuery] FilterNotificationDto input)
               => new(_notificationService.GetAllNotifications(input));

        /// <summary>
        /// thong bao chua doc
        /// </summary>
        [HttpGet("count")]
        public APIResponse CountNoti()
               => new(_notificationService.CountNotificationNotSeen());
        /// <summary>
        /// danh dau la da doc
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut("update-status")]
        public APIResponse UpdateStatus([FromBody] int id)
        {
             _notificationService.UpdateStatusNoti(id);
            return new();
        }

    }
}
