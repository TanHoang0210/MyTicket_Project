using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MYTICKET.BASE.SERVICE.Common;
using MYTICKET.UTILS.ConstantVariables.Shared;
using MYTICKET.UTILS.ConstantVaribale.Shared;
using MYTICKET.UTILS.CustomException;
using MYTICKET.UTILS.Linq;
using MYTICKET.WEB.DOMAIN.Entities;
using MYTICKET.WEB.SERVICE.Common;
using MYTICKET.WEB.SERVICE.MailService.Abstracts;
using MYTICKET.WEB.SERVICE.MailService.Dtos;
using MYTICKET.WEB.SERVICE.OrderModule.Abstracts;
using MYTICKET.WEB.SERVICE.OrderModule.Dtos;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Drawing;
using System.Text.Json;

using Order = MYTICKET.WEB.DOMAIN.Entities.Order;
using SixLabors.ImageSharp;
using MYTICKET.WEB.SERVICE.FileModule.Abstracts;
using QRCoder;

namespace MYTICKET.WEB.SERVICE.OrderModule.Implements
{
    public class OrderService : ServiceBase, IOrderService
    {
        private readonly IEmailSenderService _mail;
        private readonly IFileService _file;
        public OrderService(IEmailSenderService mail, IFileService file, ILogger<OrderService> logger, IHttpContextAccessor httpContext) : base(logger, httpContext)
        {
            _mail = mail;
            _file = file;
        }

        public async Task<OrderDto> CreateOrder(CreateOrderDto input)
        {
            var currentUserId = CommonUtils.GetCurrentUserId(_httpContext);
            var currentUser = await _dbContext.Users.FirstOrDefaultAsync(s => s.Id == currentUserId)
                ?? throw new UserFriendlyException(ErrorCode.CustomerNotFound);
            var currentCustomerId = await _dbContext.Customers
                .Where(s => s.Id == currentUser.CustomerId)
                .Select(s => s.Id)
                .FirstOrDefaultAsync();

            _logger.LogInformation($"{nameof(CreateOrder)}: input = {JsonSerializer.Serialize(input)}, currentUser= {currentUser}");

            decimal totalOrder = 0;
            var orderId = 0;

            await using (var transaction = await _dbContext.Database.BeginTransactionAsync())
            {
                try
                {
                    var orderAdd = await _dbContext.Orders.AddAsync(new Order
                    {
                        OrderCode = GenOrderCode(10),
                        OrderDate = DateTime.Now,
                        CustomerId = currentCustomerId,
                        Status = OrderStatuses.INIT,
                    });

                    await _dbContext.SaveChangesAsync();

                    orderId = orderAdd.Entity.Id;

                    if (input.TicketTypes != null && input.TicketTypes.Count() > 0)
                    {
                        foreach (var ticketType in input.TicketTypes)
                        {
                            var ticketQuery = _dbContext.Tickets
                                .Where(s => s.TicketEventId == ticketType.TicketEventId
                                    && (!_dbContext.OrderDetails
                                        .Any(x => x.TicketId == s.Id && !x.Deleted && (x.Order.Status != OrderStatuses.CANCEL))))
                                .OrderBy(s => s.Id).Take(ticketType.Quantity);

                            var ticketIds = await ticketQuery
                                .Select(ticket => ticket.Id)
                                .ToListAsync();

                            if (ticketIds.Count() < ticketType.Quantity)
                            {
                                throw new UserFriendlyException(ErrorCode.TicketInvalid);
                            }

                            foreach (var ticketId in ticketIds)
                            {
                                var orderDetail = new OrderDetail
                                {
                                    OrderId = orderId,
                                    EventDetailId = ticketType.EventDetailId,
                                    TicketId = ticketId,
                                    QrCode = null,
                                };
                                _dbContext.OrderDetails.Add(orderDetail);
                            }

                            await _dbContext.SaveChangesAsync();

                            var subTotal1 = (_dbContext.TicketEvents
                                .Where(s => s.Id == ticketType.TicketEventId)
                                .Select(s => s.Price)
                                .FirstOrDefault() * ticketType.Quantity);

                            totalOrder = totalOrder + subTotal1;
                        }
                    }

                    if (input.Tickets != null && input.Tickets.Count() > 0)
                    {
                        foreach (var ticket in input.Tickets)
                        {
                            if (_dbContext.OrderDetails
                                .Include(s => s.Order)
                                .Any(s => s.TicketId == ticket.TicketId
                                    && s.Order.Status != OrderStatuses.CANCEL && !s.Deleted))
                            {
                                throw new UserFriendlyException(ErrorCode.TicketInvalid);
                            }

                            _dbContext.OrderDetails.Add(new OrderDetail
                            {
                                OrderId = orderId,
                                EventDetailId = ticket.EventDetailId,
                                TicketId = ticket.TicketId,
                            });

                            await _dbContext.SaveChangesAsync();

                            var subTotal2 = _dbContext.TicketEvents
                                .Where(s => s.Id == ticket.TicketEventId)
                                .Select(s => s.Price)
                                .FirstOrDefault();

                            totalOrder = totalOrder + subTotal2;
                        }
                    }

                    orderAdd.Entity.Total = totalOrder;
                    orderAdd.Entity.Status = OrderStatuses.READY_TO_PAY;

                    await _dbContext.SaveChangesAsync();

                    await transaction.CommitAsync();
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    // Xử lý ngoại lệ
                }
            }

            var result = await _dbContext.Orders
                .Where(s => s.Id == orderId)
                .Select(s => new OrderDto
                {
                    Id = s.Id,
                    OrderCode = s.OrderCode,
                    CustomerId = s.CustomerId,
                    OrderDate = s.OrderDate,
                    Status = s.Status,
                    Total = s.Total,
                })
                .FirstOrDefaultAsync() ?? throw new UserFriendlyException(ErrorCode.OrderNotFound);

            var resultDetail = await _dbContext.OrderDetails
                .Include(s => s.EventDetail)
                .Include(s => s.Ticket)
                .Where(s => s.OrderId == orderId)
                .Select(s => new OrderDetailDto
                {
                    Id = s.Id,
                    OrderId = s.OrderId,
                    EventDetailId = s.EventDetailId,
                    EventName = _dbContext.Events
                        .Where(x => x.Id == s.EventDetail.EventId)
                        .Select(x => x.EventName)
                        .FirstOrDefault(),
                    OrganizationDay = s.EventDetail.OrganizationDay,
                    SeatCode = s.Ticket.SeatCode,
                    TicketCode = s.Ticket.TicketCode,
                    TicketId = s.TicketId,
                    Price = _dbContext.TicketEvents
                        .Where(x => x.Id == s.Ticket.TicketEventId)
                        .Select(x => x.Price)
                        .FirstOrDefault(),
                    TicketEventName = _dbContext.TicketEvents
                        .Where(x => x.Id == s.Ticket.TicketEventId)
                        .Select(x => x.Name)
                        .FirstOrDefault(),
                    VenueName = _dbContext.Venues
                        .Where(x => x.Id == s.EventDetail.VenueId)
                        .Select(s => s.Name)
                        .FirstOrDefault(),
                })
                .ToListAsync();

            result.OrderDetails = resultDetail;

            return result;
        }

        public IFormFile CreateQr(string input)
        {
            QRCodeGenerator qrGenerator = new();
            using FileStream fileTemplate = new("D:\\", FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            using MemoryStream ms = new();
            fileTemplate.CopyTo(ms);
            QRCode qrCode = new QRCode(qrGenerator.CreateQrCode(input, QRCodeGenerator.ECCLevel.Q));
            var image = qrCode.GetGraphic(20);
            using MemoryStream msQrCode = new();
            image.SaveAsPng(msQrCode);
            return (IFormFile)image;
        }

        public async Task DeleteOrderDetail(int id)
        {
            var currentUserId = CommonUtils.GetCurrentUserId(_httpContext);
            var currentUser = await _dbContext.Users.FirstOrDefaultAsync(s => s.Id == currentUserId)
                ?? throw new UserFriendlyException(ErrorCode.CustomerNotFound);

            var currentCustomerId = await _dbContext.Customers
                .Where(s => s.Id == currentUser.CustomerId)
                .Select(s => s.Id)
                .FirstOrDefaultAsync();

            _logger.LogInformation($"{nameof(DeleteOrderDetail)}: id = {id}, currentUser= {currentUser}");

            var orderDetails = await _dbContext.OrderDetails
                .Include(s => s.Order)
                .Where(s => s.Id == id &&
                    s.IsExchange == null && s.IsTransfer == null && !s.Deleted && s.Order.CustomerId == currentCustomerId)
                .FirstOrDefaultAsync()
                ?? throw new UserFriendlyException(ErrorCode.OrderDetailNotFound);

            var order = await _dbContext.Orders
                .FirstOrDefaultAsync(s => s.Id == orderDetails.OrderId)
                ?? throw new UserFriendlyException(ErrorCode.OrderNotFound);

            var ticket = await _dbContext.Tickets
                .Include(s => s.TicketEvent)
                .Where(s => s.Id == orderDetails.TicketId)
                .FirstOrDefaultAsync()
                ?? throw new UserFriendlyException(ErrorCode.TicketNotFound);

            orderDetails.Deleted = true;
            order.Total = order.Total - ticket.TicketEvent!.Price;

            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteOrderExpired()
        {
            var cutoffTime = DateTime.UtcNow.AddMinutes(-10);

            var ordersToDelete = await _dbContext.Orders
                .Where(s => (s.Status == OrderStatuses.READY_TO_PAY || s.Status == OrderStatuses.PAYING) && s.CreatedDate == cutoffTime)
                .ToListAsync();

            _dbContext.Orders.RemoveRange(ordersToDelete);
            await _dbContext.SaveChangesAsync();
        }

        public PagingResult<OrderDetailDto> FindAllOrderByCustomerId(FilterOrderCustomer input)
        {
            var currentUserId = CommonUtils.GetCurrentUserId(_httpContext);
            var currentUser = _dbContext.Users.FirstOrDefault(s => s.Id == currentUserId)
    ?? throw new UserFriendlyException(ErrorCode.CustomerNotFound);
            var currentCustomerId = _dbContext.Customers.Where(s => s.Id == currentUser.CustomerId).Select(s => s.Id).FirstOrDefault();
            _logger.LogInformation($"{nameof(FindAllOrderByCustomerId)}: input = {JsonSerializer.Serialize(input)}, currentUser= {currentUser}");
            var result = new PagingResult<OrderDetailDto>();
            var query = _dbContext.OrderDetails
                                                    .Include(s => s.EventDetail)
                                                    .Include(s => s.Ticket)
                                                    .Include(s => s.Order)
                                                    .Where(s => s.Order.CustomerId == currentCustomerId && s.Order.Status == OrderStatuses.SUCCESS && !s.Deleted)
                                                    .Select(s => new OrderDetailDto
                                                    {
                                                        Id = s.Id,
                                                        OrderId = s.OrderId,
                                                        OrderCode = s.Order.OrderCode,
                                                        OrderDate = s.Order.OrderDate,
                                                        EventDetailId = s.EventDetailId,
                                                        EventName = _dbContext.Events.Where(x => x.Id == s.EventDetail.EventId).Select(x => x.EventName).FirstOrDefault(),
                                                        OrganizationDay = s.EventDetail.OrganizationDay,
                                                        SeatCode = s.Ticket.SeatCode,
                                                        TicketCode = s.Ticket.TicketCode,
                                                        TicketId = s.TicketId,
                                                        Price = _dbContext.TicketEvents.Where(x => x.Id == s.Ticket.TicketEventId).Select(x => x.Price).FirstOrDefault(),
                                                        TicketEventName = _dbContext.TicketEvents.Where(x => x.Id == s.Ticket.TicketEventId).Select(x => x.Name).FirstOrDefault(),
                                                        VenueName = _dbContext.Venues.Where(x => x.Id == s.EventDetail.VenueId).Select(s => s.Name).FirstOrDefault(),
                                                        VenueAddress = _dbContext.Venues.Where(x => x.Id == s.EventDetail.VenueId).Select(s => s.Address).FirstOrDefault(),
                                                        QrCode = s.QrCode
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

        public OrderDetailDto FindOrderTicketById(int id)
        {
            var currentUserId = CommonUtils.GetCurrentUserId(_httpContext);
            var currentUser = _dbContext.Users.FirstOrDefault(s => s.Id == currentUserId)
    ?? throw new UserFriendlyException(ErrorCode.CustomerNotFound);
            var currentCustomerId = _dbContext.Customers.Where(s => s.Id == currentUser.CustomerId).Select(s => s.Id).FirstOrDefault();
            _logger.LogInformation($"{nameof(FindAllOrderByCustomerId)}: id = {id}, currentUser= {currentUser}");
            var result = new PagingResult<OrderDetailDto>();
            var query = (_dbContext.OrderDetails
                                                    .Include(s => s.EventDetail)
                                                    .Include(s => s.Ticket)
                                                    .Include(s => s.Order)
                                                    .Where(s => s.Order.CustomerId == currentCustomerId && s.Id == id && s.Order.Status == OrderStatuses.SUCCESS && !s.Deleted)
                                                    .Select(s => new OrderDetailDto
                                                    {
                                                        Id = s.Id,
                                                        OrderId = s.OrderId,
                                                        OrderCode = s.Order.OrderCode,
                                                        OrderDate = s.Order.OrderDate,
                                                        EventDetailId = s.EventDetailId,
                                                        EventName = _dbContext.Events.Where(x => x.Id == s.EventDetail.EventId).Select(x => x.EventName).FirstOrDefault(),
                                                        OrganizationDay = s.EventDetail.OrganizationDay,
                                                        SeatCode = s.Ticket.SeatCode,
                                                        TicketCode = s.Ticket.TicketCode,
                                                        TicketId = s.TicketId,
                                                        Price = _dbContext.TicketEvents.Where(x => x.Id == s.Ticket.TicketEventId).Select(x => x.Price).FirstOrDefault(),
                                                        TicketEventName = _dbContext.TicketEvents.Where(x => x.Id == s.Ticket.TicketEventId).Select(x => x.Name).FirstOrDefault(),
                                                        VenueName = _dbContext.Venues.Where(x => x.Id == s.EventDetail.VenueId).Select(s => s.Name).FirstOrDefault(),
                                                        VenueAddress = _dbContext.Venues.Where(x => x.Id == s.EventDetail.VenueId).Select(s => s.Address).FirstOrDefault(),
                                                        QrCode = s.QrCode
                                                    })).FirstOrDefault() ?? throw new UserFriendlyException(ErrorCode.OrderDetailNotFound);
            return query;
        }

        public OrderDto GetOrderReadyToPayByCustomer()
        {
            var currentUserId = CommonUtils.GetCurrentUserId(_httpContext);
            var currentUser = _dbContext.Users.FirstOrDefault(s => s.Id == currentUserId)
                ?? throw new UserFriendlyException(ErrorCode.CustomerNotFound);
            var currentCustomerId = _dbContext.Customers.Where(s => s.Id == currentUser.CustomerId).Select(s => s.Id).FirstOrDefault();
            _logger.LogInformation($"{nameof(GetOrderReadyToPayByCustomer)}: currentUser= {currentUser}");
            var result = _dbContext.Orders
            .Where(s => s.CustomerId == currentCustomerId && s.Status == OrderStatuses.READY_TO_PAY)
            .Select(s => new OrderDto
            {
                Id = s.Id,
                OrderCode = s.OrderCode,
                CustomerId = s.CustomerId,
                OrderDate = s.OrderDate,
                Status = s.Status,
                Total = s.Total,
            }).FirstOrDefault() ?? throw new UserFriendlyException(ErrorCode.OrderNotFound);
            var resultDetail = _dbContext.OrderDetails
                                                    .Include(s => s.EventDetail)
                                                    .Include(s => s.Ticket)
                                                    .Where(s => s.OrderId == result.Id && !s.Deleted)
                                                    .Select(s => new OrderDetailDto
                                                    {
                                                        Id = s.Id,
                                                        OrderId = s.OrderId,
                                                        EventDetailId = s.EventDetailId,
                                                        EventName = _dbContext.Events.Where(x => x.Id == s.EventDetail.EventId).Select(x => x.EventName).FirstOrDefault(),
                                                        OrganizationDay = s.EventDetail.OrganizationDay,
                                                        SeatCode = s.Ticket.SeatCode,
                                                        TicketCode = s.Ticket.TicketCode,
                                                        TicketId = s.TicketId,
                                                        Price = _dbContext.TicketEvents.Where(x => x.Id == s.Ticket.TicketEventId).Select(x => x.Price).FirstOrDefault(),
                                                        TicketEventName = _dbContext.TicketEvents.Where(x => x.Id == s.Ticket.TicketEventId).Select(x => x.Name).FirstOrDefault(),
                                                        VenueName = _dbContext.Venues.Where(x => x.Id == s.EventDetail.VenueId).Select(s => s.Name).FirstOrDefault(),
                                                        VenueAddress = _dbContext.Venues.Where(x => x.Id == s.EventDetail.VenueId).Select(s => s.Address).FirstOrDefault(),
                                                    }).ToList();
            result.OrderDetails = resultDetail;
            return result;
        }

        public async Task TransferTicket(TransferTicketDto input)
        {
            var currentUserId = CommonUtils.GetCurrentUserId(_httpContext);
            var currentUser = await _dbContext.Users.FirstOrDefaultAsync(s => s.Id == currentUserId)
                ?? throw new UserFriendlyException(ErrorCode.CustomerNotFound);
            var currentCustomer = await _dbContext.Customers.FirstOrDefaultAsync(s => s.Id == currentUser.CustomerId);
            var orderDetail = await _dbContext.OrderDetails.Include(s => s.Order).FirstOrDefaultAsync(s => s.Id == input.OrderDetailId
            && s.Order.CustomerId == currentCustomer.Id && !s.Deleted && s.Order.Status == OrderStatuses.SUCCESS);
            if (orderDetail == null)
            {
                throw new UserFriendlyException(ErrorCode.OrderDetailNotFound);
            }
            orderDetail.IsExchange = 1;
            orderDetail.TransferStatus = TransferStatuses.INIT;
            orderDetail.ExchangeCode = GenOrderCode(4);
            orderDetail.CustomerTransfer = input.CustomerTransferId;
            await _dbContext.SaveChangesAsync();
            if (orderDetail.TransferStatus == TransferStatuses.INIT)
            {
                try
                {
                    // Lấy dịch vụ sendmailservice
                    MailContent content = new MailContent
                    {
                        To = currentUser.Email,
                        Subject = $"[Yêu cầu chuyển nhượng vé của bạn đã được gửi đi thành công!]",
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
                                     <div style=""margin: auto; flex-direction: column; text-align: center; color: #555;"">
                                          Nhập mã sau để xác nhận chuyển nhượng vé: <strong>{orderDetail.ExchangeCode}</strong>
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

        public async Task UpdateOrderStatus(UpdateOrderStatusDto input)
        {
            var currentUserId = CommonUtils.GetCurrentUserId(_httpContext);
            var currentUser = await _dbContext.Users.FirstOrDefaultAsync(s => s.Id == currentUserId)
                ?? throw new UserFriendlyException(ErrorCode.CustomerNotFound);
            var currentCustomer = await _dbContext.Customers.FirstOrDefaultAsync(s => s.Id == currentUser.CustomerId);
            var order = await _dbContext.Orders.FirstOrDefaultAsync(s => s.Id == input.Id && s.CustomerId == currentCustomer.Id && !s.Deleted);
            if (order == null)
            {
                throw new UserFriendlyException(ErrorCode.OrderNotFound);
            }
            if (input.Status == OrderStatuses.SUCCESS && order.Status == OrderStatuses.PAYING)
            {
                order.Status = input.Status;
                try
                {
                    // Lấy dịch vụ sendmailservice
                    MailContent content = new MailContent
                    {
                        To = currentUser.Email,
                        Subject = $"[Chúc mừng bạn đã đặt vé thành công!]",
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
                                     <div style=""margin: auto; flex-direction: column; text-align: center; color: #555;"">
     
 
                                          <h2>ĐƠN HÀNG #{order.OrderCode} đã được đặt 
                                             <span style=""color: green;"">
                                                 thành công!
                                             </span>
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
            else
            {
                order.Status = input.Status;
            }
            await _dbContext.SaveChangesAsync();
        }

        private string GenOrderCode(int length)
        {
            Random random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

            // Tạo một chuỗi với ký tự ngẫu nhiên từ tập hợp chars
            string randomString = new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());

            return randomString;
        }
    }
}
