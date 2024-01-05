using DocumentFormat.OpenXml.Drawing.Charts;
using Hangfire;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MYTICKET.BASE.Infrastructure.Hangfire.Attributes;
using MYTICKET.UTILS.ConstantVariables.Shared;
using MYTICKET.UTILS.ConstantVariables.User;
using MYTICKET.UTILS.ConstantVaribale.Shared;
using MYTICKET.UTILS.CustomException;
using MYTICKET.WEB.DOMAIN.Entities;
using MYTICKET.WEB.Infrastructure.Persistence;
using MYTICKET.WEB.SERVICE.MailService.Abstracts;
using MYTICKET.WEB.SERVICE.MailService.Dtos;
using MYTICKET.WEB.SERVICE.SystemModule.Abstracts;

namespace MYTICKET.WEB.SERVICE.SystemModule.Implements
{
    public class SystemService : ISystemService
    {
        private readonly MyTicketDbContext _dbContext;
        private readonly ILogger<SystemService> _logger;
        private readonly IEmailSenderService _mail;
        public SystemService(ILogger<SystemService> logger, IEmailSenderService mail, MyTicketDbContext dbContext)
        {
            _dbContext = dbContext;
            _logger = logger;
            _mail = mail;
        }
        [AutomaticRetry(Attempts = 6, DelaysInSeconds = new int[] { 10, 20, 20, 60, 120, 60 })]
        [HangfireLogEverything]
        public async Task CancelAllExchange()
        {
            var orderDetails = _dbContext.OrderDetails.Include(s => s.Order).Include(s => s.EventDetail)
                .Where(s => s.IsExchange != null 
                && new int[] { ExchangeStatuses.INIT }.Contains(s.ExchangeStatus.Value)
                && DateTime.Now.Date.AddDays(1) >= s.EventDetail.OrganizationDay);
            foreach (var orderDetail in orderDetails)
            {
                orderDetail.ExchangeStatus = ExchangeStatuses.CANCEL;
                orderDetail.ExchangeCode = null;
                await _dbContext.SaveChangesAsync();
                var user = _dbContext.Users.FirstOrDefault(s => s.CustomerId == orderDetail.Order.CustomerId);
                try
                {
                    // Lấy dịch vụ sendmailservice
                    MailContent content = new MailContent
                    {
                        To = user.Email,
                        Subject = $"[Thông báo về việc hủy đơn chuyển nhượng vé!]",
                        Body = $@"
                                <div  style=""background-color: rgb(226, 168, 140);
                                 width: 50%;flex-direction: column; margin: auto;
                                 "">
                                    <h1 style=""font-weight: bold; width: 100%;
                                    text-align: center;
                                    background-color:rgb(188, 101, 60) ; 
                                    color: white;
                                    padding: 10px 0;
                                    "">
                                    MyTicket - Ứng dụng đặt vé số 1 Việt Nam
                                    </h1>
                                 <div style="" display: flex; padding: 20px 0;"">
                                     <div>
                                         <img style=""width: 200px; height: 200px;"" src=""https://i.postimg.cc/jdzQ25TR/logo-pink-textcolor.png"" alt="""">
                                     </div>
                                     <div style=""margin: auto; flex-direction: column; text-align: center; color: #555; font-size:1.3rem;"">
                                          <h2>
                                            Đơn chuyển nhượng vé của bạn đã bị hủy do sự kiện sắp diễn ra và không có khách hàng mua vé của bạn
                                         </h2>
                                     </div>
                                 </div>
                                </div>
                                "

                    };
                    await _mail.SendMail(content);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Failed to send email: " + ex.Message);
                }
            }
        }
        [AutomaticRetry(Attempts = 6, DelaysInSeconds = new int[] { 10, 20, 20, 60, 120, 60 })]
        [HangfireLogEverything]
        public async Task CancelAllTransfer()
        {
            var orderDetails = _dbContext.OrderDetails.Include(s => s.Order).Where(s => s.IsTransfer != null 
            && new int[] { TransferStatuses.INIT, TransferStatuses.READY_TO_TRANSFER}.Contains(s.TransferStatus.Value)
            && DateTime.Now.Date.AddDays(2) >= s.EventDetail.OrganizationDay);
            foreach(var orderDetail in orderDetails)
            {
                orderDetail.TransferStatus = TransferStatuses.CANCEL;
                orderDetail.TransferCode = null;
                await _dbContext.SaveChangesAsync();
                var user = _dbContext.Users.FirstOrDefault(s => s.CustomerId == orderDetail.Order.CustomerId);
                try
                {
                    // Lấy dịch vụ sendmailservice
                    MailContent content = new MailContent
                    {
                        To = user.Email,
                        Subject = $"[Thông báo về việc hủy đơn trả vé!]",
                        Body = $@"
                                <div  style=""background-color: rgb(226, 168, 140);
                                 width: 50%;flex-direction: column; margin: auto;
                                 "">
                                    <h1 style=""font-weight: bold; width: 100%;
                                    text-align: center;
                                    background-color:rgb(188, 101, 60) ; 
                                    color: white;
                                    padding: 10px 0;
                                    "">
                                    MyTicket - Ứng dụng đặt vé số 1 Việt Nam
                                    </h1>
                                 <div style="" display: flex; padding: 20px 0;"">
                                     <div>
                                         <img style=""width: 200px; height: 200px;"" src=""https://i.postimg.cc/jdzQ25TR/logo-pink-textcolor.png"" alt="""">
                                     </div>
                                     <div style=""margin: auto; flex-direction: column; text-align: center; color: #555; font-size:1.3rem;"">
                                          <h2>
                                            Đơn trả vé của bạn đã bị hủy do chưa xác nhận đơn trả 2 ngày trước khi diễn ra sự kiện
                                         </h2>
                                     </div>
                                 </div>
                                </div>
                                "

                    };
                    await _mail.SendMail(content);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Failed to send email: " + ex.Message);
                }
            }
        }

        [AutomaticRetry(Attempts = 6, DelaysInSeconds = new int[] { 10, 20, 20, 60, 120, 60 })]
        [HangfireLogEverything]
        public async Task CancelEventNotification(int eventDetailId)
        {
            var customerInfos = (from user in _dbContext.Users
                                 join customer in _dbContext.Customers on user.CustomerId equals customer.Id
                                 join order in _dbContext.Orders on customer.Id equals order.CustomerId into orders
                                 from order in orders.DefaultIfEmpty()
                                 join orderDetail in _dbContext.OrderDetails on order.Id equals orderDetail.OrderId
                                 join eventDetail in _dbContext.EventDetails on orderDetail.EventDetailId equals eventDetail.Id
                                 join evt in _dbContext.Events on eventDetail.EventId equals evt.Id
                                 where order.Status == OrderStatuses.SUCCESS && eventDetail.Id == eventDetailId
                                 && orderDetail.ExchangeStatus != ExchangeStatuses.SUCCESS
                                 && orderDetail.TransferStatus != TransferStatuses.SUCCESS
                                 select new
                                 {
                                     Email = user.Email,
                                     CustomerName = customer.FirstName + " " + customer.LastName,
                                     EventName = evt.EventName,
                                     EventDetailId = eventDetailId,
                                 });
            var customerTransfers = (from user in _dbContext.Users
                                     join customer in _dbContext.Customers on user.CustomerId equals customer.Id
                                     join orderDetail in _dbContext.OrderDetails on customer.Id equals orderDetail.CustomerTransfer
                                     join eventDetail in _dbContext.EventDetails on orderDetail.EventDetailId equals eventDetail.Id
                                     join order in _dbContext.Orders on orderDetail.OrderId equals order.Id into orders
                                     from order in orders.DefaultIfEmpty()
                                     join evt in _dbContext.Events on eventDetail.EventId equals evt.Id
                                     where order.Status == OrderStatuses.SUCCESS
                                     && orderDetail.ExchangeStatus == ExchangeStatuses.SUCCESS
                                     && orderDetail.TransferStatus == TransferStatuses.SUCCESS
                                     && !order.Deleted && !orderDetail.Deleted
                                     select new
                                     {
                                         Email = user.Email,
                                         CustomerName = customer.FirstName + " " + customer.LastName,
                                         EventName = evt.EventName,
                                         EventDetailId = eventDetailId,
                                     });
            customerInfos.Concat(customerTransfers);
            customerInfos = customerInfos.Distinct();
            if (!customerInfos.Any())
            {
                return;
            }
            foreach (var customerInfo in customerInfos)
            {
                if (customerInfo.Email != null)
                {
                    try
                    {
                        // Lấy dịch vụ sendmailservice
                        MailContent content = new MailContent
                        {
                            To = customerInfo.Email,
                            Subject = $"[Thông báo về việc hủy sự kiện!]",
                            Body = $@"
                                <div  style=""background-color: rgb(226, 168, 140);
                                 width: 50%;flex-direction: column; margin: auto;
                                 "">
                                    <h1 style=""font-weight: bold; width: 100%;
                                    text-align: center;
                                    background-color:rgb(188, 101, 60) ; 
                                    color: white;
                                    padding: 10px 0;
                                    "">
                                    MyTicket - Ứng dụng đặt vé số 1 Việt Nam
                                    </h1>
                                 <div style="" display: flex; padding: 20px 0;"">
                                     <div>
                                         <img style=""width: 200px; height: 200px;"" src=""https://i.postimg.cc/jdzQ25TR/logo-pink-textcolor.png"" alt="""">
                                     </div>
                                     <div style=""margin: auto; flex-direction: column; text-align: center; color: #555; font-size:1.3rem;"">
     
 
                                          <h2>
                                            Thân gửi bạn {customerInfo.CustomerName} <br>
                                            Sự kiện {customerInfo.EventName} mà bạn đã đặt vé đã bị hủy.
                                            Hệ thống sẽ thu hồi lại vé và tiến hành hoàn tiền cho bạn sớm nhé <3
                                         </h2>
                                     </div>
                                 </div>
                                </div>
                                "

                        };
                        await _mail.SendMail(content);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Failed to send email: " + ex.Message);
                    }
                }
            }
        }
        [AutomaticRetry(Attempts = 6, DelaysInSeconds = new int[] { 10, 20, 20, 60, 120, 60 })]
        [HangfireLogEverything]
        public async Task CancelOrderExpired(int orderId)
        {
            var order = _dbContext.Orders.FirstOrDefault(s => s.Id == orderId && !s.Deleted) ?? throw new UserFriendlyException(ErrorCode.OrderNotFound);
            if (!new int[] { OrderStatuses.PAYING, OrderStatuses.SUCCESS }.Contains(order.Status))
            {
                order.Status = OrderStatuses.CANCEL;
                order.BackgroundJobId = null;
            }
            await _dbContext.SaveChangesAsync();
        }

        [AutomaticRetry(Attempts = 6, DelaysInSeconds = new int[] { 10, 20, 20, 60, 120, 60 })]
        [HangfireLogEverything]
        public async Task HappyBirthDayCustomerNotification()
        {
            var customerInfos = (from user in _dbContext.Users
                                 join customer in _dbContext.Customers on user.CustomerId equals customer.Id
                                 where !user.Deleted && !customer.Deleted && user.UserType == UserTypes.CUSTOMER
                                 && (customer.DateOfBirth != null && customer.DateOfBirth.Value.Day == DateTime.UtcNow.Day)
                                 select new
                                 {
                                     customerName = customer.FirstName + customer.LastName,
                                     Email = user.Email,
                                     DateOfBirth = customer.DateOfBirth
                                 });
            if (!customerInfos.Any())
            {
                return;
            }
            foreach (var customerInfo in customerInfos)
            {
                if (customerInfo.DateOfBirth != null)
                {
                    try
                    {
                        // Lấy dịch vụ sendmailservice
                        MailContent content = new MailContent
                        {
                            To = customerInfo.Email,
                            Subject = $"[Chúc mừng sinh nhật quý khách!]",
                            Body = $@"
                                <div  style=""background-color: rgb(226, 168, 140);
                                 width: 50%;flex-direction: column; margin: auto;
                                 "">
                                    <h1 style=""font-weight: bold; width: 100%;
                                    text-align: center;
                                    background-color:rgb(188, 101, 60) ; 
                                    color: white;
                                    padding: 10px 0;
                                    "">
                                    MyTicket - Ứng dụng đặt vé số 1 Việt Nam
                                    </h1>
                                 <div style="" display: flex; padding: 20px 0;"">
                                     <div>
                                         <img style=""width: 200px; height: 200px;"" src=""https://i.postimg.cc/jdzQ25TR/logo-pink-textcolor.png"" alt="""">
                                     </div>
                                     <div style=""margin: auto; flex-direction: column; text-align: center; color: #555; font-size:1.3rem;"">
     
 
                                          <h2>
                                            Chúc {customerInfo.customerName} sinh nhật vui vẻ!
                                         </h2>
                                     </div>
                                 </div>
                                </div>
                                "

                        };
                        await _mail.SendMail(content);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Failed to send email: " + ex.Message);
                    }
                }
            }
        }
        [AutomaticRetry(Attempts = 6, DelaysInSeconds = new int[] { 10, 20, 20, 60, 120, 60 })]
        [HangfireLogEverything]
        public async Task NotiSuccessTransfer(string buyTicketUserEmail, int ownerUserId)
        {
            try
            {
                // Lấy dịch vụ sendmailservice
                MailContent content = new MailContent
                {
                    To = buyTicketUserEmail,
                    Subject = $"[Chúc mừng bạn đã mua lại vé thành công!]",
                    Body = $@"
                                <div  style=""background-color: rgb(226, 168, 140);
                                 width: 50%;flex-direction: column; margin: auto;
                                 "">
                                    <h1 style=""font-weight: bold; width: 100%;
                                    text-align: center;
                                    background-color:rgb(188, 101, 60) ; 
                                    color: white;
                                    padding: 10px 0;
                                    "">
                                    MyTicket - Ứng dụng đặt vé số 1 Việt Nam
                                    </h1>
                                 <div style="" display: flex; padding: 20px 0;"">
                                     <div>
                                         <img style=""width: 200px; height: 200px;"" src=""https://i.postimg.cc/jdzQ25TR/logo-pink-textcolor.png"" alt="""">
                                     </div>
                                     <div style=""margin: auto; flex-direction: column; text-align: center; color: #555; font-size:1.3rem;"">
     
 
                                          <h2>Bạn đã đặt lại vé
                                             <span style=""color: green;"">
                                                 thành công!
                                             </span>
                                                Vé đã được chuyển vào giỏ vé của bạn. Bạn sẽ không thể trả lại hay chuyển nhượng vé này
                                         </h2>
                                          <button style=""background-color: rgb(188, 101, 60);height: 50px; margin: auto;
                                           font-size: 20px; border-radius: 4px 4px; "">
                                              <a style=""text-decoration: none; color: white;""  href=""http://localhost:8080/order"">Kiểm tra vé của bạn tại đây</a>
                                          </button>
                                     </div>
                                 </div>
                                </div>
                                "
                };
                await _mail.SendMail(content);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to send email: " + ex.Message);
            }
            var ownerUser = _dbContext.Users.FirstOrDefault(s => s.CustomerId == ownerUserId);
            try
            {
                // Lấy dịch vụ sendmailservice
                MailContent content = new MailContent
                {
                    To = ownerUser.Email,
                    Subject = $"[Vé chuyển nhượng của bạn đã được bán lại thành công]",
                    Body = $@"
                                <div  style=""background-color: rgb(226, 168, 140);
                                 width: 50%;flex-direction: column; margin: auto;
                                 "">
                                    <h1 style=""font-weight: bold; width: 100%;
                                    text-align: center;
                                    background-color:rgb(188, 101, 60) ; 
                                    color: white;
                                    padding: 10px 0;
                                    "">
                                    MyTicket - Ứng dụng đặt vé số 1 Việt Nam
                                    </h1>
                                 <div style="" display: flex; padding: 20px 0;"">
                                     <div>
                                         <img style=""width: 200px; height: 200px;"" src=""https://i.postimg.cc/jdzQ25TR/logo-pink-textcolor.png"" alt="""">
                                     </div>
                                     <div style=""margin: auto; flex-direction: column; text-align: center; color: #555; font-size:1.3rem;"">
     
 
                                          <h2>
                                                Vé đã được chuyển nhượng thành công. Hệ thống MyTicket sẽ hoàn tiền lại cho bạn trong vòng 1 tuần.
                                         </h2>
                                          <button style=""background-color: rgb(188, 101, 60);height: 50px; margin: auto;
                                           font-size: 20px; border-radius: 4px 4px; "">
                                              <a style=""text-decoration: none; color: white;""  href=""http://localhost:8080/order"">Kiểm tra vé của bạn tại đây</a>
                                          </button>
                                     </div>
                                 </div>
                                </div>
                                "
                };
                await _mail.SendMail(content);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to send email: " + ex.Message);
            }
        }

        [AutomaticRetry(Attempts = 6, DelaysInSeconds = new int[] { 10, 20, 20, 60, 120, 60 })]
        [HangfireLogEverything]
        public async Task NotRefundExchangeNotificationForAdmin()
        {
            var exchanges = _dbContext.OrderDetails.Include(s => s.Order).Where(s => s.Status == OrderStatuses.SUCCESS && s.IsExchange == 1
            && s.ExchangeStatus == ExchangeStatuses.READY_TO_EXCHANGE && !s.Deleted && s.CustomerTransfer == null && s.ExchangeRefundRequest);
            foreach (var exchange in exchanges)
            {
                var title = $@"Thông báo chưa tiến hành hoàn tiền đơn trả vé thành công";
                var description = $@"Vé của khách hàng đã được trả thành công vui lòng hoàn tiền";
                await _dbContext.Notifications.AddAsync(new Notification
                {
                    CreateDate = DateTime.Now,
                    Description = description,
                    CustomerId = exchange.Order.CustomerId,
                    IsSeen = false,
                    Title = title,
                });
            }
            await _dbContext.SaveChangesAsync();
        }

        [AutomaticRetry(Attempts = 6, DelaysInSeconds = new int[] { 10, 20, 20, 60, 120, 60 })]
        [HangfireLogEverything]
        public async Task NotRefundNotificationForAdmin()
        {
            var events = _dbContext.EventDetails.Include(s => s.Event).Where(s => s.Status == EventStatuses.CANCEL && !s.Deleted);
            foreach (var evt in events)
            {
                var orders = _dbContext.OrderDetails.Where(s => s.EventDetailId == evt.Id && s.RefundRequest 
                && !s.Deleted && s.Status == OrderDetailStatuses.SUCCESS).Count();
                var ordersTransfer = _dbContext.OrderDetails.Where(s => s.EventDetailId == evt.Id && s.RefundRequest
                && !s.Deleted && s.Status == OrderDetailStatuses.SUCCESS && s.TransferStatus == TransferStatuses.SUCCESS && s.CustomerTransfer != null).Count();
                var title = $@"Thông báo chưa tiến hành hoàn tiền sự kiện bị hủy";
                var description = $@"Sự kiện {evt.Event.EventName} diễn ra vào ngày {evt.OrganizationDay} đã bị hủy. Vẫn còn {orders + ordersTransfer} đơn vé chưa được hoàn tiền";
                await _dbContext.Notifications.AddAsync(new Notification
                {
                    CreateDate = DateTime.Now,
                    Description = description,
                    EventDetailId = evt.Id,
                    IsSeen = false,
                    Title = title,
                });
            }
            await _dbContext.SaveChangesAsync();
        }
        [AutomaticRetry(Attempts = 6, DelaysInSeconds = new int[] { 10, 20, 20, 60, 120, 60 })]
        [HangfireLogEverything]
        public async Task NotRefundTransferNotificationForAdmin()
        {
            var transfers = _dbContext.OrderDetails.Include(s => s.Order).Where(s => s.Status == OrderStatuses.SUCCESS && s.IsTransfer == 1
            && s.TransferStatus == TransferStatuses.SUCCESS && !s.Deleted && s.CustomerTransfer != null && s.TransferRefundRequest);
            foreach(var transfer in transfers) 
            {
                var title = $@"Thông báo chưa tiến hành hoàn tiền cho đơn chuyển nhượng vé thành công";
                var description = $@"Vé của khách hàng đã được chuyển nhượng thành công vui lòng hoàn tiền";
                await _dbContext.Notifications.AddAsync(new Notification
                {
                    CreateDate = DateTime.Now,
                    Description = description,
                    CustomerId = transfer.Order.CustomerId,
                    IsSeen = false,
                    Title = title,
                });
            }
            await _dbContext.SaveChangesAsync();
        }
        [AutomaticRetry(Attempts = 6, DelaysInSeconds = new int[] { 10, 20, 20, 60, 120, 60 })]
        [HangfireLogEverything]
        public async Task ReceiveRefundNotification(string title,string type,string CustomerMail, string TicketType, string TicketCode,string SeatCode,string EventName, DateTime OrganDay)
        {
            try
            {
                // Lấy dịch vụ sendmailservice
                MailContent mymail = new MailContent
                {
                    To = CustomerMail,
                    Subject = $"[{title}]",
                    Body = $@"
                                <div  style=""background-color: rgb(226, 168, 140);
                                 width: 50%;flex-direction: column; margin: auto;
                                 "">
                                    <h1 style=""font-weight: bold; width: 100%;
                                    text-align: center;
                                    background-color:rgb(188, 101, 60) ; 
                                    color: white;
                                    padding: 10px 0;
                                    "">
                                    MyTicket - Ứng dụng đặt vé số 1 Việt Nam
                                    </h1>
                                 <div style="" display: flex; padding: 20px 0;"">
                                     <div>
                                         <img style=""width: 200px; height: 200px;"" src=""https://i.postimg.cc/jdzQ25TR/logo-pink-textcolor.png"" alt="""">
                                     </div>
                                     <div style=""margin: auto; flex-direction: column; text-align: center; color: #555; font-size:1.3rem;"">
     
 
                                          <h2>
                                            {type} hạng: {TicketType} | Mã vé: {TicketCode}
                                                    | Mã chỗ ngồi: {SeatCode} của
                                                sự kiện {EventName} diễn ra vào ngày {OrganDay} đã được hoàn tiền thành công!
                                         </h2>
                                     </div>
                                 </div>
                                </div>
                                "

                };
                await _mail.SendMail(mymail);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to send email: " + ex.Message);
            }
        }
    }
}
