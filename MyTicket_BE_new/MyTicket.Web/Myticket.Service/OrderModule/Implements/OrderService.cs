using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MYTICKET.BASE.SERVICE.Common;
using MYTICKET.UTILS.ConstantVariables.Shared;
using MYTICKET.UTILS.ConstantVaribale.Shared;
using MYTICKET.UTILS.CustomException;
using MYTICKET.WEB.DOMAIN.Entities;
using MYTICKET.WEB.SERVICE.Common;
using MYTICKET.WEB.SERVICE.OrderModule.Abstracts;
using MYTICKET.WEB.SERVICE.OrderModule.Dtos;
using System.Net.Sockets;
using System.Text.Json;

namespace MYTICKET.WEB.SERVICE.OrderModule.Implements
{
    public class OrderService : ServiceBase, IOrderService
    {
        public OrderService(ILogger<OrderService> logger, IHttpContextAccessor httpContext) : base(logger, httpContext)
        {
        }

        public OrderDto CreateOrder(CreateOrderDto input)
        {
            var currentUser = CommonUtils.GetCurrentUserId(_httpContext);
            _logger.LogInformation($"{nameof(CreateOrder)}: input = {JsonSerializer.Serialize(input)}, currentUser= {currentUser}");
            decimal totalOrder = 0;
            var transaction = _dbContext.Database.BeginTransaction();
            var orderAdd = _dbContext.Orders.Add(new Order
            {
                OrderCode = GenOrderCode(10),
                OrderDate = DateTime.Now,
                CustomerId = currentUser,
                Status = OrderStatuses.INIT,
            }).Entity;
            _dbContext.SaveChanges();
            if (input.TicketTypes != null && input.TicketTypes.Count() > 0)
            {
                foreach (var ticketType in input.TicketTypes)
                {
                    var ticketList = _dbContext.Tickets
                                        .Where(s => s.TicketEventId == ticketType.TicketEventId
                                        && s.CustomerId == null && s.Status == TicketStatus.ACTIVE)
                                        .OrderBy(s => s.SeatCode).Take(ticketType.Quantity).ToList();
                    foreach (var ticket in ticketList)
                    {
                        _dbContext.OrderDetails.Add(new OrderDetail
                        {
                            OrderId = orderAdd.Id,
                            EventDetailId = ticketType.EventDetailId,
                            TicketId = ticket.Id,
                            TicketEventId = ticketType.TicketEventId,
                        });
                        _dbContext.SaveChanges();
                    }
                    var subTotal1 = (_dbContext.TicketEvents.Where(s => s.Id == ticketType.TicketEventId).Select(s => s.Price).FirstOrDefault() * ticketType.Quantity);
                    totalOrder = totalOrder + subTotal1;
                }
            }
            if (input.Tickets != null && input.Tickets.Count() > 0)
            {
                foreach (var ticket in input.Tickets)
                {
                    _dbContext.OrderDetails.Add(new OrderDetail
                    {
                        OrderId = orderAdd.Id,
                        EventDetailId = ticket.EventDetailId,
                        TicketId = ticket.TicketId,
                        TicketEventId = ticket.TicketEventId,
                    });
                    _dbContext.SaveChanges();
                    var subTotal2 = _dbContext.TicketEvents.Where(s => s.Id == ticket.TicketEventId).Select(s => s.Price).FirstOrDefault();
                    totalOrder = totalOrder + subTotal2;
                }
            }
            orderAdd.Total = totalOrder;
            transaction.Commit();
            var result = _dbContext.Orders
                        .Where(s => s.Id == orderAdd.Id)
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
                                                    .Include(s => s.TicketEvent)
                                                    .Where(s => s.OrderId == orderAdd.Id)
                                                    .Select(s => new OrderDetailDto
                                                    {
                                                        Id = s.Id,
                                                        OrderId = s.OrderId,
                                                        EventDetailId = s.EventDetailId,
                                                        EventName = _dbContext.Events.Where(x => x.Id == s.EventDetail.EventId).Select(x => x.EventName).FirstOrDefault(),
                                                        OrganizationDay = s.EventDetail.OrganizationDay,
                                                        Price = s.TicketEvent.Price,
                                                        SeatCode = s.Ticket.SeatCode,
                                                        TicketCode = s.Ticket.TicketCode,
                                                        TicketEventName = s.TicketEvent.Name,
                                                        TicketId = s.TicketId,
                                                        VenueName = _dbContext.Venues.Where(x => x.Id == s.EventDetail.VenueId).Select(s => s.Name).FirstOrDefault(),
                                                    }).ToList();
            result.OrderDetails = resultDetail;
            return result;
        }

        public OrderDto FindAllOrderByCustomerId(FilterOrderCustomer input)
        {
            throw new NotImplementedException();
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
