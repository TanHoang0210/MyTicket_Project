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
using MYTICKET.WEB.SERVICE.EventModule.Dtos;
using MYTICKET.WEB.SERVICE.TicketModule.Abstracts;
using MYTICKET.WEB.SERVICE.TicketModule.Dtos;
using System.Text.Json;

namespace MYTICKET.WEB.SERVICE.TicketModule.Implements
{
    public class TicketService : ServiceBase, ITicketService
    {
        public TicketService(ILogger<TicketService> logger, IHttpContextAccessor httpContext) : base(logger, httpContext)
        {
        }

        public void CreateTicket(CreateTicketDto input)
        {
            throw new NotImplementedException();
        }

        public void CreateTicketEvent(CreateTicketEventDto input)
        {
            var eventDetail = _dbContext.EventDetails.FirstOrDefault(s => s.Id == input.EventDetailId && !s.Deleted) ?? throw new UserFriendlyException(ErrorCode.EventDetailNotFound);
            var transaction = _dbContext.Database.BeginTransaction();
            var ticketEventAdd = _dbContext.TicketEvents.Add(new TicketEvent
            {
                Name = input.Name,
                Price = input.Price,
                EventDetailId = input.EventDetailId,
                Status = EventStatuses.INIT,
            }).Entity;
            _dbContext.SaveChanges();
            for (var i = 1; i <= input.Quantity; i++)
            {
                _dbContext.Tickets.Add(new Ticket
                {
                    TicketEventId = ticketEventAdd.Id,
                    TicketCode = GenerateCode(6),
                    SeatCode = GenerateCode(7),
                });
            };
            _dbContext.SaveChanges();
            transaction.Commit();
        }

        public List<TicketEventDto> GetAllTicket(int eventDetailId)
        {
            var ticketEvents = (from ticketEvent in _dbContext.TicketEvents
                               join ticket in _dbContext.Tickets on ticketEvent.Id equals ticket.TicketEventId into tickets
                               where ticketEvent.EventDetailId == eventDetailId
                               select new TicketEventDto
                               {
                                   Id = ticketEvent.Id,
                                   Name = ticketEvent.Name,
                                   EventDetailId = ticketEvent.EventDetailId,
                                   IntQuantity = tickets.Count(),
                                   Price = ticketEvent.Price,
                                   Status = ticketEvent.Status,
                               }).ToList();
            foreach (var ticketEvent in ticketEvents)
            {
                var orderQuantity = _dbContext.OrderDetails.Include(s => s.Order).Include(s => s.Order).Where(s => s.Ticket.TicketEventId == ticketEvent.Id && s.Order.Status == OrderStatuses.SUCCESS && !s.Deleted && !s.Order.Deleted).Count();
                ticketEvent.Quantity = ticketEvent.IntQuantity - orderQuantity;
            }
            return ticketEvents;
        }

        public PagingResult<TicketEventTransferDto> GetAllTicketTransfer(FilterTicketDto input)
        {
            var currentUserId = CommonUtils.GetCurrentUserId(_httpContext);
            _logger.LogInformation($"{nameof(GetAllTicketTransfer)}: input = {JsonSerializer.Serialize(input)}");
            var currentUser = _dbContext.Users.FirstOrDefault(s => s.Id == currentUserId && !s.Deleted);
            var currentCustomer = _dbContext.Customers.FirstOrDefault(s => s.Id == currentUser.CustomerId && !s.Deleted);
            var result = new PagingResult<TicketEventTransferDto>();

            var query = from od in _dbContext.Orders
                        join odDetail in _dbContext.OrderDetails on od.Id equals odDetail.OrderId
                        join tk in _dbContext.Tickets on odDetail.TicketId equals tk.Id
                        join tkEvent in _dbContext.TicketEvents on tk.TicketEventId equals tkEvent.Id
                        where odDetail.IsTransfer == 1 && od.Status == OrderStatuses.SUCCESS && odDetail.TransferStatus == TransferStatuses.READY_TO_TRANSFER 
                        && odDetail.EventDetailId == input.EventDetailId && !od.Deleted && !odDetail.Deleted && od.CustomerId != currentCustomer.Id
                        select new TicketEventTransferDto
                        {
                            Id = tk.Id,
                            CustomerTransferId = od.CustomerId,
                            EventDetailId = odDetail.EventDetailId,
                            Price = tkEvent.Price,
                            SeatCode = tk.SeatCode,
                            TicketName = tkEvent.Name
                        };
            result.TotalItems = query.Count();
            query = query.OrderDynamic(input.Sort);

            if (input.PageSize != -1)
            {
                query = query.Skip(input.GetSkip()).Take(input.PageSize);
            }

            result.Items = query;
            return result;
        }

        public TicketEventDto GetTicketById(int id)
        {
            var result = (from ticketEvent in _dbContext.TicketEvents
                                join ticket in _dbContext.Tickets on ticketEvent.Id equals ticket.TicketEventId into tickets
                                where ticketEvent.Id == id
                                select new TicketEventDto
                                {
                                    Id = ticketEvent.Id,
                                    Name = ticketEvent.Name,
                                    EventDetailId = ticketEvent.EventDetailId,
                                    IntQuantity = tickets.Count(),
                                    Price = ticketEvent.Price,
                                    Status = ticketEvent.Status,
                                }).FirstOrDefault() ?? throw new UserFriendlyException(ErrorCode.TicketNotFound);
                var orderQuantity = _dbContext.OrderDetails.Include(s => s.Order).Include(s => s.Order).Where(s => s.Ticket.TicketEventId == result.Id && s.Order.Status == OrderStatuses.SUCCESS && !s.Deleted && !s.Order.Deleted).Count();
            result.Quantity = result.IntQuantity - orderQuantity;
            return result;
        }

        private string GenerateCode(int length)
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
