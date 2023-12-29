using Hangfire;
using Microsoft.Extensions.Logging;
using MYTICKET.BASE.Infrastructure.Hangfire.Attributes;
using MYTICKET.BASE.SERVICE.Common;
using MYTICKET.UTILS.ConstantVaribale.Shared;
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
            throw new NotImplementedException();
        }

        public PagingResult<NotificationDto> GetAllNotifications(FilterNotificationDto input)
        {
            throw new NotImplementedException();
        }
    }
}
