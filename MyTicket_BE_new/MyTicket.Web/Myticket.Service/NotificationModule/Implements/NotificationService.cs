using Hangfire;
using Microsoft.Extensions.Logging;
using MYTICKET.BASE.Infrastructure.Hangfire.Attributes;
using MYTICKET.BASE.SERVICE.Common;
using MYTICKET.UTILS.ConstantVaribale.Shared;
using MYTICKET.UTILS.Linq;
using MYTICKET.WEB.DOMAIN.Entities;
using MYTICKET.WEB.Infrastructure.Persistence;
using MYTICKET.WEB.SERVICE.MailService.Abstracts;
using MYTICKET.WEB.SERVICE.NotificationModule.Abstracts;
using MYTICKET.WEB.SERVICE.NotificationModule.Dtos;

namespace MYTICKET.WEB.SERVICE.NotificationModule.Implements
{
    public class NotificationService : INotificationService
    {
        private readonly MyTicketDbContext _dbContext;
        private readonly ILogger<NotificationService> _logger;
        private readonly IEmailSenderService _mail;
        public NotificationService(ILogger<NotificationService> logger, IEmailSenderService mail, MyTicketDbContext dbContext)
        {
            _dbContext = dbContext;
            _logger = logger;
            _mail = mail;
        }

        public int CountNotificationNotSeen()
        {
             var count = _dbContext.Notifications.Where(s => !s.IsSeen);
             return count.Count();
        }

        public PagingResult<NotificationDto> GetAllNotifications(FilterNotificationDto input)
        {
            var result = new PagingResult<NotificationDto>();
            var query = _dbContext.Notifications.Where(s => input.IsSeen == null || input.IsSeen == s.IsSeen).Select(s => new NotificationDto
            {
                CreateDate = s.CreateDate,
                CustomerId = s.CustomerId,
                Description = s.Description,
                EventDetailId = s.EventDetailId,
                Id = s.Id,
                IsSeen = s.IsSeen,
                Title = s.Title
            });
            result.TotalItems = query.Count();
            query = query.OrderDynamic(input.Sort);

            if (input.PageSize != -1)
            {
                query = query.Skip(input.GetSkip()).Take(input.PageSize);
            }
            result.Items = query;
            return result;
        }
        public void UpdateStatusNoti(int id)
        {
            var noti = _dbContext.Notifications.FirstOrDefault(s => s.Id == id);
            if (noti != null)
            {
                noti.IsSeen = true;
            }
            _dbContext.SaveChanges();
        }
    }
}
