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
using MYTICKET.WEB.SERVICE.EventModule.Abstracts;
using MYTICKET.WEB.SERVICE.EventModule.Dtos;
using MYTICKET.WEB.SERVICE.TicketModule.Dtos;
using System.Text.Json;

namespace MYTICKET.WEB.SERVICE.EventModule.Implements
{
    public class EventService : ServiceBase, IEventService
    {
        public EventService(ILogger<EventService> logger, IHttpContextAccessor httpContext) : base(logger, httpContext)
        {
        }

        public void CreateEvent(CreateEventDto input)
        {
            var currentUser = CommonUtils.GetCurrentUserId(_httpContext);
            _logger.LogInformation($"{nameof(CreateEvent)}: input = {JsonSerializer.Serialize(input)}");
            var transaction = _dbContext.Database.BeginTransaction();
            var eventAdd = _dbContext.Events
                                     .Add(new Event
                                     {
                                         SupplierId = input.SupplierId,
                                         EventName = input.EventName,
                                         AdmissionPolicy = input.AdmissionPolicy,
                                         Status = EventStatuses.INIT,
                                         EventDescription = input.EventDescription,
                                         ExchangePolicy = input.ExchangePolicy,
                                         EventTypeId = input.EventTypeId,
                                         EventImage = input.EventImage,
                                         IsExChange = input.IsExchange,
                                     }).Entity;
            _dbContext.SaveChanges();
            if (input.EventDetails != null && input.EventDetails.Count() > 0)
            {
                foreach (var itemEvent in input.EventDetails)
                {
                    var eventDetailAdd = _dbContext.EventDetails
                                                .Add(new EventDetail
                                                {
                                                    EventId = eventAdd.Id,
                                                    VenueId = itemEvent.VenueId,
                                                    Status = EventStatuses.INIT,
                                                    StartSaleTicketDate = itemEvent.StartSaleTicketDate,
                                                    EndSaleTicketDate = itemEvent.EndSaleTicketDate,
                                                    OrganizationDay = itemEvent.OrganizationDay,
                                                    EventSeatMapImage = itemEvent.EventSeatMapImage,
                                                    HavingSeatMap = itemEvent.HavingSeatMap,
                                                    SeatSelectType = itemEvent.SelectSeatType
                                                }).Entity;
                    _dbContext.SaveChanges();
                    if (itemEvent.TicketEvents != null)
                    {
                        foreach (var ticket in itemEvent.TicketEvents)
                        {
                            var ticketEventAdd = _dbContext.TicketEvents
                                                          .Add(new TicketEvent
                                                          {
                                                              EventDetailId = eventDetailAdd.Id,
                                                              Name = ticket.Name,
                                                              Status = TicketEventStatuses.INIT,
                                                              Price = ticket.Price,                                                          
                                                          }).Entity;
                            _dbContext.SaveChanges();
                            for (int i = 1; i <= ticket.Quantity; i++)
                            {
                                _dbContext.Tickets
                                    .Add(new Ticket
                                    {
                                        TicketEventId = ticketEventAdd.Id,
                                        SeatCode = "SEAT" + i,
                                        Status = TicketStatus.ACTIVE,
                                        TicketCode = GenerateCode(5),                                      
                                    });
                                _dbContext.SaveChanges();
                            }
                        }
                    }
                }
            }
            var startDateEvent = _dbContext.EventDetails.Select(s => s.OrganizationDay).Min();
            eventAdd.StartEventDate = startDateEvent;
            _dbContext.SaveChanges();
            transaction.Commit();
        }

        public PagingResult<EventDto> FindAll(FilterEventDto input)
        {
            _logger.LogInformation($"{nameof(FindAll)}: input = {JsonSerializer.Serialize(input)}");
            var result = new PagingResult<EventDto>();
            var query = _dbContext.Events.Where(p => !p.Deleted
                                                      && (input.EventName == null || p.EventName.ToLower().Contains(input.EventName.ToLower())))
                .Select(s => new EventDto
                {
                    Id = s.Id,
                    EventName = s.EventName,
                    EventDescription = s.EventDescription,
                    EventImage = s.EventImage,
                    EventTypeId = s.EventTypeId,
                    EventTypeName = _dbContext.EventTypes.Where(x => x.Id == s.EventTypeId).Select(x => x.Name).FirstOrDefault(),
                    FirstEventDate = _dbContext.EventDetails.Where(o => o.EventId == s.Id).OrderBy(o => o.OrganizationDay).Select(s => s.OrganizationDay).First().Date,
                    LastEventDate = _dbContext.EventDetails.Where(o => o.EventId == s.Id).OrderBy(o => o.OrganizationDay).Select(s => s.OrganizationDay).Last().Date,
                    Status = s.Status
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

        public EventDto GetEventById(int eventId)
        {
            var eventinfo = _dbContext.Events.Where(s => s.Id == eventId && !s.Deleted)
                .Select(s => new EventDto
                {
                    Id = s.Id,
                    EventName = s.EventName,
                    EventDescription = s.EventDescription,
                    EventImage = s.EventImage,
                    EventTypeId = s.EventTypeId,
                    EventTypeName = _dbContext.EventTypes.Where(x => x.Id == s.EventTypeId).Select(x => x.Name).FirstOrDefault(),
                    FirstEventDate = _dbContext.EventDetails.Where(o => o.EventId == s.Id).OrderBy(o => o.OrganizationDay).Select(s => s.OrganizationDay).First().Date,
                    LastEventDate = _dbContext.EventDetails.Where(o => o.EventId == s.Id).OrderBy(o => o.OrganizationDay).Select(s => s.OrganizationDay).Last().Date,
                    Status = s.Status,
                    ExchangePolicy = s.ExchangePolicy,
                    AdmissionPolicy = s.AdmissionPolicy
                })
                .FirstOrDefault()
               ?? throw new UserFriendlyException(ErrorCode.EventNotFound);

            var eventDetails = _dbContext.EventDetails
                .Include(s => s.Event)
                .Include(s => s.TicketEvents.Where(s => !s.Deleted))
                .ThenInclude(x => x.Tickets.Where(s => s.Status == TicketStatus.ACTIVE))
                .Where(s => s.EventId == eventinfo.Id && !s.Deleted)
                .Select(s => new EventDetailDto
                {
                    Id = s.Id,
                    EventId = s.EventId,
                    EventName = s.Event!.EventName,
                    EventImage = s.Event!.EventImage,
                    EndSaleTicketDate = s.EndSaleTicketDate,
                    EventSeatMapImage = s.EventSeatMapImage,
                    OrganizationDay = s.OrganizationDay,
                    StartSaleTicketDate = s.StartSaleTicketDate,
                    Status = s.Status,
                    VenueId = s.VenueId,
                    HavingSeatMap = s.HavingSeatMap,
                    SeatSelectType = s.SeatSelectType,
                    VenueName = _dbContext.Venues.Where(x => x.Id == s.VenueId && !s.Deleted).Select(s => s.Name).FirstOrDefault(),
                    TicketEvents = s.TicketEvents.Select(x => new TicketEventDto
                    {
                        Id = x.Id,
                        Name = x.Name,
                        EventDetailId = x.EventDetailId,
                        Price = x.Price,
                        Quantity = x.Tickets.Count() < 10 ? x.Tickets.Count() : 10,
                        Status = x.Status
                    })
                });
            eventinfo.EventDetails = eventDetails;
            return eventinfo;
        }

        public EventDetailDto GetEventDetailTicketById(int eventDetailId)
        {
            var eventDetails = _dbContext.EventDetails
                .Include(s => s.Event)
                .Include(s => s.TicketEvents.Where(s => !s.Deleted))
                .ThenInclude(x => x.Tickets.Where(s => s.Status == TicketStatus.ACTIVE))
                .Where(s => s.Id == eventDetailId && !s.Deleted)
                .Select(s => new EventDetailDto
                {
                    Id = s.Id,
                    EventId = s.EventId,
                    EventImage = s.Event!.EventImage,
                    EventName = s.Event!.EventName,
                    EndSaleTicketDate = s.EndSaleTicketDate,
                    EventSeatMapImage = s.EventSeatMapImage,
                    OrganizationDay = s.OrganizationDay,
                    StartSaleTicketDate = s.StartSaleTicketDate,
                    Status = s.Status,
                    VenueId = s.VenueId,
                    VenueName = _dbContext.Venues.Where(x => x.Id == s.VenueId && !s.Deleted).Select(s => s.Name).FirstOrDefault(),
                    TicketEvents = s.TicketEvents.Select(x => new TicketEventDto
                    {
                        Id = x.Id,
                        Name = x.Name,
                        EventDetailId = x.EventDetailId,
                        Price = x.Price,
                        Quantity = x.Tickets.Count() < 10 ? x.Tickets.Count() : 10,
                        Status = x.Status
                    })
                }).FirstOrDefault() ?? throw new UserFriendlyException(ErrorCode.EventDetailNotFound);
            return eventDetails;
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
