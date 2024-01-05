using DocumentFormat.OpenXml.Drawing.Charts;
using Hangfire;
using MailKit.Search;
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
using MYTICKET.WEB.SERVICE.FileModule.Abstracts;
using MYTICKET.WEB.SERVICE.FileModule.Dtos.UploadFile;
using MYTICKET.WEB.SERVICE.MailService.Abstracts;
using MYTICKET.WEB.SERVICE.MailService.Dtos;
using MYTICKET.WEB.SERVICE.OrderModule.Abstracts;
using MYTICKET.WEB.SERVICE.OrderModule.Dtos;
using MYTICKET.WEB.SERVICE.SystemModule.Abstracts;
using QRCoder;
using SixLabors.ImageSharp;
using System.Text.Json;
using Order = MYTICKET.WEB.DOMAIN.Entities.Order;

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
            var currentUser = _dbContext.Users.FirstOrDefault(s => s.Id == currentUserId)
                ?? throw new UserFriendlyException(ErrorCode.CustomerNotFound);
            var currentCustomerId =  _dbContext.Customers
                .Where(s => s.Id == currentUser.CustomerId)
                .Select(s => s.Id)
                .FirstOrDefault();

            _logger.LogInformation($"{nameof(CreateOrder)}: input = {JsonSerializer.Serialize(input)}, currentUser= {currentUser}");
            decimal totalOrder = 0;
            var orderId = 0;
            var orderNoPay = _dbContext.Orders.FirstOrDefault(s => s.CustomerId == currentCustomerId && new int[] { OrderStatuses.READY_TO_PAY, OrderStatuses.PAYING }.Contains(s.Status) && !s.Deleted);
            if (orderNoPay != null)
            {
                if(_dbContext.OrderDetails.Where(s => s.OrderId == orderNoPay.Id && !s.Deleted).Count() >= 10)
                {
                    throw new UserFriendlyException(ErrorCode.OrderIsFullNow);
                }
                totalOrder = orderNoPay.Total;
                orderId = orderNoPay.Id;
                await using (var transaction = await _dbContext.Database.BeginTransactionAsync())
                {
                    try
                    {
                        if (input.TicketTypes != null && input.TicketTypes.Count() > 0)
                        {
                            foreach (var ticketType in input.TicketTypes)
                            {
                                var price = _dbContext.TicketEvents.Where(s => s.Id == ticketType.TicketEventId).Select(s => s.Price).FirstOrDefault();
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
                                        Status = OrderDetailStatuses.INIT,
                                        Price = price
                                    };
                                    _dbContext.OrderDetails.Add(orderDetail);
                                }

                                await _dbContext.SaveChangesAsync();

                                var subTotal1 = (await _dbContext.TicketEvents
                                    .Where(s => s.Id == ticketType.TicketEventId)
                                    .Select(s => s.Price)
                                    .FirstOrDefaultAsync() * ticketType.Quantity);

                                totalOrder = totalOrder + subTotal1;
                            }
                            if (input.Tickets != null && input.Tickets.Count() > 0)
                            {
                                foreach (var ticket in input.Tickets)
                                {
                                    var tk = _dbContext.Tickets.Where(s => s.Id == ticket.TicketId).FirstOrDefault();
                                    var price = _dbContext.TicketEvents.Where(s => s.Id == tk.TicketEventId).Select(s => s.Price).FirstOrDefault();
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
                                        Status = OrderDetailStatuses.INIT,
                                        Price = price
                                    });

                                    await _dbContext.SaveChangesAsync();

                                    var subTotal2 = _dbContext.TicketEvents
                                        .Where(s => s.Id == ticket.TicketEventId)
                                        .Select(s => s.Price)
                                        .FirstOrDefault();

                                    totalOrder = totalOrder + subTotal2;
                                }
                            }

                            orderNoPay.Total = totalOrder;
                            orderNoPay.Status = OrderStatuses.READY_TO_PAY;
                            await _dbContext.SaveChangesAsync();
                            await transaction.CommitAsync();
                        }
                    }
                    catch (Exception ex)
                    {
                        await transaction.RollbackAsync();
                        // Xử lý ngoại lệ
                    }
                }
            }
            else
            {
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
                                var price = _dbContext.TicketEvents.Where(s => s.Id == ticketType.TicketEventId).Select(s => s.Price).FirstOrDefault();
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
                                    await _dbContext.OrderDetails.AddAsync(new OrderDetail
                                    {
                                        OrderId = orderId,
                                        EventDetailId = ticketType.EventDetailId,
                                        TicketId = ticketId,
                                        Status = OrderDetailStatuses.INIT,
                                        Price = price
                                    });
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
                                var tk = _dbContext.Tickets.Where(s => s.Id == ticket.TicketId).FirstOrDefault();
                                var price = _dbContext.TicketEvents.Where(s => s.Id == tk.TicketEventId).Select(s => s.Price).FirstOrDefault();
                                if (_dbContext.OrderDetails
                                    .Include(s => s.Order)
                                    .Any(s => s.TicketId == ticket.TicketId
                                        && s.Order.Status != OrderStatuses.CANCEL && !s.Deleted))
                                {
                                    throw new UserFriendlyException(ErrorCode.TicketInvalid);
                                }

                                await _dbContext.OrderDetails.AddAsync(new OrderDetail
                                {
                                    OrderId = orderId,
                                    EventDetailId = ticket.EventDetailId,
                                    TicketId = ticket.TicketId,
                                    Status = OrderDetailStatuses.INIT,
                                    Price = price
                                });


                                var subTotal2 = _dbContext.TicketEvents
                                    .Where(s => s.Id == ticket.TicketEventId)
                                    .Select(s => s.Price)
                                    .FirstOrDefault();

                                totalOrder = totalOrder + subTotal2;
                            }
                        }
                        var jobId = BackgroundJob.Schedule<ISystemService>(x => x.CancelOrderExpired(orderId), TimeSpan.FromSeconds(290));

                        orderAdd.Entity.Total = totalOrder;
                        orderAdd.Entity.Status = OrderStatuses.READY_TO_PAY;
                        orderAdd.Entity.BackgroundJobId = jobId;
                        await _dbContext.SaveChangesAsync();
                        await transaction.CommitAsync();
                    }
                    catch (Exception ex)
                    {
                        await transaction.RollbackAsync();
                        // Xử lý ngoại lệ
                    }
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
        private async Task<string> CreateQr(QRCodeDto input)
        {
            var currentUserId = CommonUtils.GetCurrentUserId(_httpContext);
            var currentUser = await _dbContext.Users.FirstOrDefaultAsync(s => s.Id == currentUserId)
                ?? throw new UserFriendlyException(ErrorCode.CustomerNotFound);
            var currentCustomerId = await _dbContext.Customers
                .Where(s => s.Id == currentUser.CustomerId)
                .Select(s => s.Id)
                .FirstOrDefaultAsync();
            var qrInfo = JsonSerializer.Serialize(input);
            QRCodeGenerator qrGenerator = new();
            QRCode qrCode = new QRCode(qrGenerator.CreateQrCode(qrInfo, QRCodeGenerator.ECCLevel.Q));
            var image = qrCode.GetGraphic(20);

            using MemoryStream msQrCode = new();
            image.SaveAsPng(msQrCode);

            // Convert MemoryStream to byte array
            byte[] imageBytes = msQrCode.ToArray();

            // Create FormFile from byte array
            IFormFile formFile = new FormFile(new MemoryStream(imageBytes), 0, imageBytes.Length, "QrCode", "TK" + GenOrderCode(5) + ".png");

            UploadFileModel qr = new UploadFileModel { File = formFile, Folder = "QRCode" };
            var result = _file.UploadFile(qr);

            return result;
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
                                                    .Where(s=> s.Order.CustomerId == currentCustomerId
                                                    && s.Order.Status == OrderStatuses.SUCCESS && !s.Deleted
                                                    && (s.IsTransfer == null || (s.IsTransfer != null && s.TransferStatus == TransferStatuses.CANCEL))
                                                    && (s.IsExchange == null || (s.IsExchange != null && s.ExchangeStatus == ExchangeStatuses.CANCEL)))
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
                                                        QrCode = s.QrCode,
                                                        status = s.Status,
                                                        IsExchange = _dbContext.Events.Where(x => x.Id == s.EventDetail.EventId).Select(x => x.IsExChange).FirstOrDefault(),
                                                        EventStatus = s.EventDetail.Status
                                                    });
            var listTransfer = _dbContext.OrderDetails.Include(s => s.EventDetail)
                                                    .Include(s => s.Ticket)
                                                    .Include(s => s.Order)
                                                    .Where(s => s.IsTransfer != null && s.TransferStatus == TransferStatuses.SUCCESS && s.CustomerTransfer == currentCustomerId)
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
                                                        QrCode = s.QrCode,
                                                        status = 10,
                                                        IsExchange = _dbContext.Events.Where(x => x.Id == s.EventDetail.EventId).Select(x => x.IsExChange).FirstOrDefault(),
                                                        EventStatus = s.EventDetail.Status
                                                    });
            query = query.Concat(listTransfer);
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
            _logger.LogInformation($"{nameof(FindAllOrderByCustomerId)}: id = {id}");
            var query = (_dbContext.OrderDetails
                                                    .Include(s => s.EventDetail)
                                                    .Include(s => s.Ticket)
                                                    .Include(s => s.Order)
                                                    .Where(s => s.Id == id && !s.Deleted)
                                                    .Select(s => new OrderDetailDto
                                                    {
                                                        Id = s.Id,
                                                        OrderId = s.OrderId,
                                                        OrderCode = s.Order.OrderCode, 
                                                        OrderDate = s.CustomerTransfer == null ? s.Order.OrderDate : s.TransferDoneDate,
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
                                                        QrCode = s.QrCode,
                                                        status = s.Status,
                                                        IsExchange = _dbContext.Events.Where(x => x.Id == s.EventDetail.EventId).Select(x => x.IsExChange).FirstOrDefault(),
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
            .Where(s => s.CustomerId == currentCustomerId && new int[] { OrderStatuses.READY_TO_PAY, OrderStatuses.PAYING }.Contains(s.Status) && !s.Deleted)
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
            var dateNow = DateTime.Now.Date;
            var eventDetail = _dbContext.EventDetails.FirstOrDefault(s => s.Id == orderDetail.EventDetailId && !s.Deleted && s.Status != EventDetailStatuses.CANCEL);
            if (dateNow.AddDays(2) >= eventDetail.OrganizationDay.Date)
            {
                throw new UserFriendlyException(ErrorCode.CannotTransfer);
            }
            orderDetail.IsTransfer = 1;
            orderDetail.TransferStatus = TransferStatuses.INIT;
            orderDetail.TransferCode = GenOrderCode(4);
            orderDetail.TransferDate = DateTime.Now;
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
                                     <div style=""margin: auto; flex-direction: column; text-align: center; color: #555; font-size:1.3rem;"">
                                          Nhập mã sau để xác nhận chuyển nhượng vé: <strong>{orderDetail.TransferCode}</strong>
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
                await _dbContext.SaveChangesAsync();
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
            if (input.Status == OrderStatuses.SUCCESS)
            {
                BackgroundJob.Delete(order.BackgroundJobId);
                order.BackgroundJobId = null;
                order.Status = input.Status;
                order.TransactionNo = input.TransactionNo;
                order.TransDate = input.TransDate;
                var orderDetails = await _dbContext.OrderDetails.Include(s => s.Ticket).ThenInclude(s => s.TicketEvent).Where(s => s.OrderId == input.Id && !s.Deleted).ToListAsync();
                foreach (var item in orderDetails)
                {
                    item.Status = OrderDetailStatuses.SUCCESS;
                    item.QrCode = await CreateQr(new QRCodeDto
                    {
                        TicketId = item.TicketId,
                        OrderDetailId = item.Id,
                        Status = item.Status,
                        CustomerId = item.Order.CustomerId
                    });
                }
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
                                     <div style=""margin: auto; flex-direction: column; text-align: center; color: #555; font-size:1.3rem;"">
     
 
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
                if(order.Status == OrderStatuses.CANCEL)
                {
                    BackgroundJob.Delete(order.BackgroundJobId);
                    order.BackgroundJobId = null;
                }
                var orderDetails = await _dbContext.OrderDetails.Where(s => s.OrderId == input.Id && !s.Deleted).ToListAsync();
                foreach (var item in orderDetails)
                {
                    item.Status = order.Status;
                }
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

        public string QrTest(string input)
        {
            QRCodeGenerator qrGenerator = new();
            QRCode qrCode = new QRCode(qrGenerator.CreateQrCode(input, QRCodeGenerator.ECCLevel.Q));
            var image = qrCode.GetGraphic(20);

            using MemoryStream msQrCode = new();
            image.SaveAsPng(msQrCode);

            // Convert MemoryStream to byte array
            byte[] imageBytes = msQrCode.ToArray();

            // Create FormFile from byte array
            IFormFile formFile = new FormFile(new MemoryStream(imageBytes), 0, imageBytes.Length, "QrCode", "QrCode.png");

            UploadFileModel qr = new UploadFileModel { File = formFile, Folder = "QRCode" };
            var result = _file.UploadFile(qr);

            return result;
        }

        public PagingResult<TicketTransferDto> FindAllTransferCustomerId(FilterOrderCustomer input)
        {
            var currentUserId = CommonUtils.GetCurrentUserId(_httpContext);
            var currentUser = _dbContext.Users.FirstOrDefault(s => s.Id == currentUserId)
                               ?? throw new UserFriendlyException(ErrorCode.CustomerNotFound);
            var currentCustomerId = _dbContext.Customers.Where(s => s.Id == currentUser.CustomerId).Select(s => s.Id).FirstOrDefault();
            _logger.LogInformation($"{nameof(FindAllOrderByCustomerId)}: input = {JsonSerializer.Serialize(input)}, currentUser= {currentUser}");
            var result = new PagingResult<TicketTransferDto>();
            var query = _dbContext.OrderDetails
                                                    .Include(s => s.EventDetail)
                                                    .Include(s => s.Ticket)
                                                    .Include(s => s.Order)
                                                    .Where(s => s.Order.CustomerId == currentCustomerId
                                                    && s.Order.Status == OrderStatuses.SUCCESS && !s.Deleted
                                                    && s.IsTransfer != null && new int[] { TransferStatuses.INIT, TransferStatuses.READY_TO_TRANSFER, TransferStatuses.TRANSFERING, TransferStatuses.SUCCESS }.Contains(s.TransferStatus.Value))
                                                    .Select(s => new TicketTransferDto
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
                                                        Status = s.Status,
                                                        Price = _dbContext.TicketEvents.Where(x => x.Id == s.Ticket.TicketEventId).Select(x => x.Price).FirstOrDefault(),
                                                        TicketEventName = _dbContext.TicketEvents.Where(x => x.Id == s.Ticket.TicketEventId).Select(x => x.Name).FirstOrDefault(),
                                                        VenueName = _dbContext.Venues.Where(x => x.Id == s.EventDetail.VenueId).Select(s => s.Name).FirstOrDefault(),
                                                        VenueAddress = _dbContext.Venues.Where(x => x.Id == s.EventDetail.VenueId).Select(s => s.Address).FirstOrDefault(),
                                                        TransferDate = s.TransferDate,
                                                        TransferStatus = s.TransferStatus,
                                                        TransferCancelDate = s.TransferCancelDate,
                                                        TransferDoneDate = s.TransferDoneDate
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

        public PagingResult<TicketExchangeDto> FindAllExchangeCustomerId(FilterOrderCustomer input)
        {
            var currentUserId = CommonUtils.GetCurrentUserId(_httpContext);
            var currentUser = _dbContext.Users.FirstOrDefault(s => s.Id == currentUserId)
                               ?? throw new UserFriendlyException(ErrorCode.CustomerNotFound);
            var currentCustomerId = _dbContext.Customers.Where(s => s.Id == currentUser.CustomerId).Select(s => s.Id).FirstOrDefault();
            _logger.LogInformation($"{nameof(FindAllOrderByCustomerId)}: input = {JsonSerializer.Serialize(input)}, currentUser= {currentUser}");
            var result = new PagingResult<TicketExchangeDto>();
            var query = _dbContext.OrderDetails
                                                    .Include(s => s.EventDetail)
                                                    .Include(s => s.Ticket)
                                                    .Include(s => s.Order)
                                                    .Where(s => s.Order.CustomerId == currentCustomerId
                                                    && s.Order.Status == OrderStatuses.SUCCESS && !s.Deleted
                                                    && s.IsExchange != null && new int[] { ExchangeStatuses.INIT, ExchangeStatuses.READY_TO_EXCHANGE, TransferStatuses.SUCCESS }.Contains(s.ExchangeStatus.Value))
                                                    .Select(s => new TicketExchangeDto
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
                                                        ExchangeDate = s.ExchangeDate,
                                                        ExchangeStatus = s.ExchangeStatus,
                                                        ExchangeCancelDate = s.ExchangeCancelDate,
                                                        ExchangeDoneDate = s.ExchangeDoneDate,
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

        public TicketTransferDto FindTransferTicketById(int id)
        {
            _logger.LogInformation($"{nameof(FindAllOrderByCustomerId)}: id = {id}");
            var query = (_dbContext.OrderDetails
                                                    .Include(s => s.EventDetail)
                                                    .Include(s => s.Ticket)
                                                    .Include(s => s.Order)
                                                    .Where(s => s.Id == id
                                                    && s.Order.Status == OrderStatuses.SUCCESS && !s.Deleted
                                                    && (s.IsTransfer != null))
                                                    .Select(s => new TicketTransferDto
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
                                                        Status = s.Status,
                                                        Price = _dbContext.TicketEvents.Where(x => x.Id == s.Ticket.TicketEventId).Select(x => x.Price).FirstOrDefault(),
                                                        TicketEventName = _dbContext.TicketEvents.Where(x => x.Id == s.Ticket.TicketEventId).Select(x => x.Name).FirstOrDefault(),
                                                        VenueName = _dbContext.Venues.Where(x => x.Id == s.EventDetail.VenueId).Select(s => s.Name).FirstOrDefault(),
                                                        VenueAddress = _dbContext.Venues.Where(x => x.Id == s.EventDetail.VenueId).Select(s => s.Address).FirstOrDefault(),
                                                        TransferDate = s.TransferDate,
                                                        TransferStatus = s.TransferStatus,
                                                        TransferDoneDate = s.TransferDoneDate,
                                                        TransferCancelDate = s.TransferCancelDate,
                                                    })).FirstOrDefault() ?? throw new UserFriendlyException(ErrorCode.OrderDetailNotFound);
            return query;
        }

        public TicketExchangeDto FindExchangeTicketById(int id)
        {
            _logger.LogInformation($"{nameof(FindAllOrderByCustomerId)}: id = {id}");
            var query = (_dbContext.OrderDetails
                                                    .Include(s => s.EventDetail)
                                                    .Include(s => s.Ticket)
                                                    .Include(s => s.Order)
                                                    .Where(s => s.Id == id
                                                    && s.Order.Status == OrderStatuses.SUCCESS && !s.Deleted
                                                    && s.IsExchange != null)
                                                    .Select(s => new TicketExchangeDto
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
                                                        ExchangeDate = s.ExchangeDate,
                                                        ExchangeStatus = s.ExchangeStatus,
                                                        ExchangeDoneDate = s.ExchangeDoneDate,
                                                        ExchangeCancelDate = s.ExchangeCancelDate,
                                                    })).FirstOrDefault() ?? throw new UserFriendlyException(ErrorCode.OrderDetailNotFound);
            return query;
        }

        public async Task CancelTransferTicket(TransferTicketDto input)
        {
            var currentUserId = CommonUtils.GetCurrentUserId(_httpContext);
            var currentUser = await _dbContext.Users.FirstOrDefaultAsync(s => s.Id == currentUserId)
                ?? throw new UserFriendlyException(ErrorCode.CustomerNotFound);
            var currentCustomer = await _dbContext.Customers.FirstOrDefaultAsync(s => s.Id == currentUser.CustomerId);
            var ticketTransfer = await _dbContext.OrderDetails.Include(s => s.Order).Include(s => s.EventDetail)
                .FirstOrDefaultAsync(s => s.Id == input.OrderDetailId && s.Order.CustomerId == currentCustomer.Id && !s.Deleted
            && (new int[] { TransferStatuses.INIT, TransferStatuses.READY_TO_TRANSFER, TransferStatuses.CANCEL }.Contains(s.TransferStatus.Value) && s.CustomerTransfer == null));
            if (ticketTransfer == null)
            {
                throw new UserFriendlyException(ErrorCode.CannotCancelTransfer);
            }
            ticketTransfer.TransferStatus = TransferStatuses.CANCEL;
            ticketTransfer.TransferCancelDate = DateTime.Now;
            await _dbContext.SaveChangesAsync();
        }

        public async Task CancelExchangeTicket(TransferTicketDto input)
        {
            var currentUserId = CommonUtils.GetCurrentUserId(_httpContext);
            var currentUser = await _dbContext.Users.FirstOrDefaultAsync(s => s.Id == currentUserId)
                ?? throw new UserFriendlyException(ErrorCode.CustomerNotFound);
            var currentCustomer = await _dbContext.Customers.FirstOrDefaultAsync(s => s.Id == currentUser.CustomerId);
            var ticketExchange = await _dbContext.OrderDetails.Include(s => s.Order).Include(s => s.EventDetail)
                .FirstOrDefaultAsync(s => s.Id == input.OrderDetailId && s.Order.CustomerId == currentCustomer.Id && !s.Deleted
            && (new int[] { ExchangeStatuses.INIT, ExchangeStatuses.READY_TO_EXCHANGE, ExchangeStatuses.CANCEL }.Contains(s.ExchangeStatus.Value)));
            if (ticketExchange == null)
            {
                throw new UserFriendlyException(ErrorCode.CannotCancelExchange);
            }
            ticketExchange.ExchangeStatus = ExchangeStatuses.CANCEL;
            ticketExchange.ExchangeCancelDate = DateTime.Now;
            ticketExchange.ExchangeRefundRequest = false;
            await _dbContext.SaveChangesAsync();
        }

        public async Task ExchangeTicket(TransferTicketDto input)
        {
            var currentUserId = CommonUtils.GetCurrentUserId(_httpContext);
            var currentUser = await _dbContext.Users.FirstOrDefaultAsync(s => s.Id == currentUserId)
                ?? throw new UserFriendlyException(ErrorCode.CustomerNotFound);
            var currentCustomer = await _dbContext.Customers.FirstOrDefaultAsync(s => s.Id == currentUser.CustomerId);
            var orderDetail = await _dbContext.OrderDetails.Include(s => s.Order).Include(s => s.EventDetail).FirstOrDefaultAsync(s => s.Id == input.OrderDetailId
            && s.Order.CustomerId == currentCustomer.Id && !s.Deleted && s.Order.Status == OrderStatuses.SUCCESS
            && (s.IsTransfer == null || s.IsTransfer != null && s.TransferStatus == TransferStatuses.CANCEL));
            if (orderDetail == null)
            {
                throw new UserFriendlyException(ErrorCode.OrderDetailNotFound);
            }
            var dateNow = DateTime.Now.Date.AddDays(2);
            var eventDetail = _dbContext.EventDetails.FirstOrDefault(s => s.Id == orderDetail.EventDetailId && !s.Deleted && s.Status != EventDetailStatuses.CANCEL);
            if (dateNow >= eventDetail.OrganizationDay.Date)
            {
                throw new UserFriendlyException(ErrorCode.CannotExchange);
            }
            orderDetail.IsExchange = 1;
            orderDetail.ExchangeStatus = ExchangeStatuses.INIT;
            orderDetail.ExchangeCode = GenOrderCode(4);
            orderDetail.ExchangeDate = DateTime.Now;
            await _dbContext.SaveChangesAsync();
            if (orderDetail.ExchangeStatus == ExchangeStatuses.INIT)
            {
                try
                {
                    // Lấy dịch vụ sendmailservice
                    MailContent content = new MailContent
                    {
                        To = currentUser.Email,
                        Subject = $"[Yêu cầu Trả vé của bạn đã được gửi đi thành công!]",
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
                                          Nhập mã sau để xác nhận trả vé: <strong>{orderDetail.ExchangeCode}</strong>
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

        public async Task ConfirmExchange(ConfirmExchangeTransferDto input)
        {
            var currentUserId = CommonUtils.GetCurrentUserId(_httpContext);
            var currentUser = await _dbContext.Users.FirstOrDefaultAsync(s => s.Id == currentUserId)
                ?? throw new UserFriendlyException(ErrorCode.CustomerNotFound);
            var currentCustomer = await _dbContext.Customers.FirstOrDefaultAsync(s => s.Id == currentUser.CustomerId);
            var orderDetail = await _dbContext.OrderDetails.Include(s => s.Order).Include(s => s.EventDetail).FirstOrDefaultAsync(s => s.Id == input.Id
            && s.Order.CustomerId == currentCustomer.Id && !s.Deleted && s.Order.Status == OrderStatuses.SUCCESS
            && s.IsExchange != null && s.ExchangeStatus == ExchangeStatuses.INIT && s.CustomerTransfer == null);
            if (orderDetail == null)
            {
                throw new UserFriendlyException(ErrorCode.OrderDetailNotFound);
            }
            var dateNow = DateTime.Now.Date;
            var eventDetail = _dbContext.EventDetails.FirstOrDefault(s => s.Id == orderDetail.EventDetailId && !s.Deleted && s.Status != EventDetailStatuses.CANCEL);
            if(dateNow.AddDays(2) >= eventDetail.OrganizationDay.Date)
            {
                throw new UserFriendlyException(ErrorCode.CannotExchange);
            }
            if (orderDetail.ExchangeCode.ToLower() == input.ConfirmCode.ToLower())
            {
                orderDetail.ExchangeStatus = ExchangeStatuses.READY_TO_EXCHANGE;
                orderDetail.ExchangeRefundRequest = true;
                try
                {
                    // Lấy dịch vụ sendmailservice
                    MailContent content = new MailContent
                    {
                        To = currentUser.Email,
                        Subject = $"[Xác nhân hoàn trả vé thành công!]",
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
                                          <strong>Tiền hoàn trả sẽ được MyTicket.com kiểm tra và thanh toán cho bạn trong vòng 1 tuần nhé!</strong>
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
                await _dbContext.SaveChangesAsync();
            }
            else
            {
                throw new UserFriendlyException(ErrorCode.ComfirmExchangeCodeWrong);
            }
        }

        public async Task ConfirmTransfer(ConfirmExchangeTransferDto input)
        {
            var currentUserId = CommonUtils.GetCurrentUserId(_httpContext);
            var currentUser = await _dbContext.Users.FirstOrDefaultAsync(s => s.Id == currentUserId)
                ?? throw new UserFriendlyException(ErrorCode.CustomerNotFound);
            var currentCustomer = await _dbContext.Customers.FirstOrDefaultAsync(s => s.Id == currentUser.CustomerId);
            var orderDetail = await _dbContext.OrderDetails.Include(s => s.Order).FirstOrDefaultAsync(s => s.Id == input.Id
            && s.Order.CustomerId == currentCustomer.Id && !s.Deleted && s.Order.Status == OrderStatuses.SUCCESS
            && s.IsTransfer != null && s.TransferStatus == TransferStatuses.INIT && s.CustomerTransfer == null);
            if (orderDetail == null)
            {
                throw new UserFriendlyException(ErrorCode.OrderDetailNotFound);
            }
            var dateNow = DateTime.Now.Date;
            var eventDetail = _dbContext.EventDetails.FirstOrDefault(s => s.Id == orderDetail.EventDetailId && !s.Deleted && s.Status != EventDetailStatuses.CANCEL);
            if (dateNow.AddDays(2) >= eventDetail.OrganizationDay.Date)
            {
                throw new UserFriendlyException(ErrorCode.CannotTransfer);
            }
            if (orderDetail.TransferCode == input.ConfirmCode)
            {
                orderDetail.TransferStatus = TransferStatuses.READY_TO_TRANSFER;
                try
                {
                    // Lấy dịch vụ sendmailservice
                    MailContent content = new MailContent
                    {
                        To = currentUser.Email,
                        Subject = $"[Xác nhân chuyển nhượng vé thành công!]",
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
                                          <strong>MyTicket sẽ giúp bạn chuyển nhượng lại vé cho người khác giúp bạn!
                                                Nếu 2 ngày trước khi diễn ra sự kiện vé chưa chuyển nhượng được thì vé sẽ được hoàn trả lại cho bạn nhé!</strong>
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
                await _dbContext.SaveChangesAsync();
            }
            else
            {
                throw new UserFriendlyException(ErrorCode.ComfirmTransferCodeWrong);
            }
        }

        public async Task UpdateTransferStatus(UpdateTransferStatusDto input)
        {
            var currentUserId = CommonUtils.GetCurrentUserId(_httpContext);
            var currentUser = await _dbContext.Users.FirstOrDefaultAsync(s => s.Id == currentUserId)
                ?? throw new UserFriendlyException(ErrorCode.CustomerNotFound);
            var currentCustomer = await _dbContext.Customers.FirstOrDefaultAsync(s => s.Id == currentUser.CustomerId);
            var orderDetail = await _dbContext.OrderDetails.Include(s => s.Order).FirstOrDefaultAsync(s => s.TicketId == input.TicketId
            && s.Order.CustomerId == input.CustomerTransferOwnerId && !s.Deleted && s.Order.Status == OrderStatuses.SUCCESS
            && s.IsTransfer != null && s.CustomerTransfer == null);
            if (orderDetail == null)
            {
                throw new UserFriendlyException(ErrorCode.OrderDetailNotFound);
            }
            if (input.TransferStatus == TransferStatuses.SUCCESS && orderDetail.TransferStatus == TransferStatuses.TRANSFERING)
            {
                orderDetail.TransferStatus = input.TransferStatus;
                orderDetail.CustomerTransfer = currentCustomer.Id;
                orderDetail.TransferRefundRequest = true;
                orderDetail.TransferTransactionNo = input.TransferTransactionNo;
                orderDetail.TransferTransDate = input.TransferTransdate;
                orderDetail.TransferDoneDate = DateTime.Now;
                orderDetail.QrCode = await CreateQr(new QRCodeDto
                {
                    TicketId = orderDetail.TicketId,
                    OrderDetailId = orderDetail.Id,
                    Status = orderDetail.Status,
                    CustomerId = currentCustomer.Id
                });
                BackgroundJob.Enqueue<ISystemService>(x => x.NotiSuccessTransfer(currentUser.Email,input.CustomerTransferOwnerId));
            }
            else
            {
                orderDetail.TransferStatus = input.TransferStatus;
            }
            await _dbContext.SaveChangesAsync();
        }

        public PagingResult<OrderDetailDto> FindAllOrderByCustomerIdAdmin(FilterOrderCustomer input)
        {
            var result = new PagingResult<OrderDetailDto>();
            var query = _dbContext.OrderDetails
                                                    .Include(s => s.EventDetail)
                                                    .Include(s => s.Ticket)
                                                    .Include(s => s.Order)
                                                    .Where(s => s.Order.CustomerId == input.CustomerId && !s.Deleted
                                                    && (s.IsTransfer == null || (s.IsTransfer != null && s.TransferStatus == TransferStatuses.CANCEL))
                                                    && (s.IsExchange == null || (s.IsExchange != null && s.ExchangeStatus == ExchangeStatuses.CANCEL))
                                                    && (input.Keyword == null || s.Order.OrderCode.Contains(input.Keyword))
                                                    && (input.Status == null || s.Status == input.Status))
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
                                                        QrCode = s.QrCode,
                                                        status = s.Status,
                                                        IsExchange = _dbContext.Events.Where(x => x.Id == s.EventDetail.EventId).Select(x => x.IsExChange).FirstOrDefault(),
                                                        RefundRequest = s.RefundRequest
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

        public PagingResult<TicketTransferDto> FindAllOrderTransferByCustomerId(FilterOrderCustomer input)
        {
            var result = new PagingResult<TicketTransferDto>();
            var query = _dbContext.OrderDetails.Include(s => s.EventDetail)
                                                    .Include(s => s.Ticket)
                                                    .Include(s => s.Order)
                                                    .Where(s => s.IsTransfer != null && s.CustomerTransfer == input.CustomerId)
                                                    .Select(s => new TicketTransferDto
                                                    {
                                                        Id = s.Id,
                                                        OrderId = s.OrderId,
                                                        OrderCode = s.Order.OrderCode,
                                                        OrderDate = s.TransferDoneDate,
                                                        EventDetailId = s.EventDetailId,
                                                        EventName = _dbContext.Events.Where(x => x.Id == s.EventDetail.EventId).Select(x => x.EventName).FirstOrDefault(),
                                                        OrganizationDay = s.EventDetail.OrganizationDay,
                                                        SeatCode = s.Ticket.SeatCode,
                                                        TicketCode = s.Ticket.TicketCode,
                                                        TicketId = s.TicketId,
                                                        Status = s.Status,
                                                        Price = _dbContext.TicketEvents.Where(x => x.Id == s.Ticket.TicketEventId).Select(x => x.Price).FirstOrDefault(),
                                                        TicketEventName = _dbContext.TicketEvents.Where(x => x.Id == s.Ticket.TicketEventId).Select(x => x.Name).FirstOrDefault(),
                                                        VenueName = _dbContext.Venues.Where(x => x.Id == s.EventDetail.VenueId).Select(s => s.Name).FirstOrDefault(),
                                                        VenueAddress = _dbContext.Venues.Where(x => x.Id == s.EventDetail.VenueId).Select(s => s.Address).FirstOrDefault(),
                                                        TransferDate = s.TransferDate,
                                                        TransferStatus = s.TransferStatus,
                                                        TransferCancelDate = s.TransferCancelDate,
                                                        TransferDoneDate = s.TransferDoneDate,
                                                        RefundRequest = s.RefundRequest,
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

        public PagingResult<TicketTransferDto> FindAllTransferCustomerIdAdmin(FilterOrderCustomer input)
        {
            _logger.LogInformation($"{nameof(FindAllOrderByCustomerId)}: input = {JsonSerializer.Serialize(input)}");
            var result = new PagingResult<TicketTransferDto>();
            var query = _dbContext.OrderDetails
                                                    .Include(s => s.EventDetail)
                                                    .Include(s => s.Ticket)
                                                    .Include(s => s.Order)
                                                    .Where(s => s.Order.CustomerId == input.CustomerId
                                                    && s.Order.Status == OrderStatuses.SUCCESS
                                                    && s.TransferStatus != TransferStatuses.INIT
                                                    && !s.Deleted && s.IsTransfer != null)
                                                    .Select(s => new TicketTransferDto
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
                                                        Status = s.Status,
                                                        Price = _dbContext.TicketEvents.Where(x => x.Id == s.Ticket.TicketEventId).Select(x => x.Price).FirstOrDefault(),
                                                        TicketEventName = _dbContext.TicketEvents.Where(x => x.Id == s.Ticket.TicketEventId).Select(x => x.Name).FirstOrDefault(),
                                                        VenueName = _dbContext.Venues.Where(x => x.Id == s.EventDetail.VenueId).Select(s => s.Name).FirstOrDefault(),
                                                        VenueAddress = _dbContext.Venues.Where(x => x.Id == s.EventDetail.VenueId).Select(s => s.Address).FirstOrDefault(),
                                                        TransferDate = s.TransferDate,
                                                        TransferStatus = s.TransferStatus,
                                                        TransferCancelDate = s.TransferCancelDate,
                                                        TransferDoneDate = s.TransferDoneDate,
                                                        TransferRefundRequest = s.TransferRefundRequest
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

        public PagingResult<TicketExchangeDto> FindAllExchangeCustomerIdAdmin(FilterOrderCustomer input)
        {
            _logger.LogInformation($"{nameof(FindAllOrderByCustomerId)}: input = {JsonSerializer.Serialize(input)}");
            var result = new PagingResult<TicketExchangeDto>();
            var query = _dbContext.OrderDetails
                                                    .Include(s => s.EventDetail)
                                                    .Include(s => s.Ticket)
                                                    .Include(s => s.Order)
                                                    .Where(s => s.Order.CustomerId == input.CustomerId
                                                    && s.Order.Status == OrderStatuses.SUCCESS && !s.Deleted
                                                    && s.IsExchange != null)
                                                    .Select(s => new TicketExchangeDto
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
                                                        ExchangeDate = s.ExchangeDate,
                                                        ExchangeStatus = s.ExchangeStatus,
                                                        ExchangeDoneDate = s.ExchangeDoneDate,
                                                        ExchangeCancelDate = s.ExchangeCancelDate,
                                                        ExchangeRefundRequest = s.ExchangeRefundRequest
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
    }
}
