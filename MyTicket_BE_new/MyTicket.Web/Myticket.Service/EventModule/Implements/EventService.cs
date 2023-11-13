using Microsoft.AspNetCore.Http;
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
using MYTICKET.WEB.SERVICE.VenueModule.Dtos;
using System.Text.Json;

namespace MYTICKET.WEB.SERVICE.EventModule.Implements
{
    public class EventService : ServiceBase, IEventService
    {
        public EventService(ILogger<EventService> logger, IHttpContextAccessor httpContext) : base(logger, httpContext)
        {
        }

        public EventDto CreateEvent(CreateEventDto input)
        {
            var currentUser = CommonUtils.GetCurrentSupplierId(_httpContext); ;
            _logger.LogInformation($"{nameof(CreateEvent)}: input = {JsonSerializer.Serialize(input)}");
            var transaction = _dbContext.Database.BeginTransaction();
            var eventAdd = _dbContext.Events
                                     .Add(new Event
                                     {
                                         SupplierId = currentUser,
                                         EventName = input.EventName,
                                         AdmissionPolicy = input.AdmissionPolicy,
                                         Status = EventStatuses.INIT,
                                         EventDescription = input.EventDescription,
                                         ExchangePolicy = input.ExchangePolicy,
                                         EventTypeId = input.EventTypeId,
                                         EventImage = input.EventImage,
                                     }).Entity;
            _dbContext.SaveChanges();
            if (input.EventDetails != null && input.EventDetails.Count() > 0)
            {
                foreach (var itemEvent in input.EventDetails)
                {
                    var eventDetailAdd = _dbContext.EventDetails
                                                .Add(new EventDetail
                                                {
                                                    VenueId = itemEvent.VenueId,
                                                    Status = EventStatuses.INIT,
                                                    StartSaleTicketDate = itemEvent.StartSaleTicketDate,
                                                    EndSaleTicketDate = itemEvent.EndSaleTicketDate,
                                                    OrganizationDay = itemEvent.OrganizationDay,
                                                    EventSeatMapImage = itemEvent.EventSeatMapImage,
                                                    EventId = eventAdd.Id,
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
                                                              Quantity = ticket.Quantity,                                                         
                                                          }).Entity;
                            _dbContext.SaveChanges();
                            for(int i = 1; i <= ticketEventAdd.Quantity; i++)
                            {
                                _dbContext.Tickets
                                    .Add(new Ticket
                                    {
                                        TicketEventId = ticketEventAdd.Id,
                                        SeatCode = "SEAT"+i,
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
            return _mapper.Map<EventDto>(eventAdd);
        }

        public PagingResult<EventDto> FindAll(FilterEventDto input)
        {
            _logger.LogInformation($"{nameof(FindAll)}: input = {JsonSerializer.Serialize(input)}");
            var result = new PagingResult<EventDto>();
            var query = _dbContext.Events.Where(p => !p.Deleted
                                                      && (input.EventName == null || p.EventName.ToLower().Contains(input.EventName.ToLower())));

            result.TotalItems = query.Count();
            query = query.OrderDynamic(input.Sort);

            if (input.PageSize != -1)
            {
                query = query.Skip(input.GetSkip()).Take(input.PageSize);
            }
            result.Items = _mapper.Map<IEnumerable<EventDto>>(query);
            return result;
        }

        public EventDetailDto GetEventById(int eventDetailId)
        {
            var result = new EventDetailDto();
            var eventDetail = _dbContext.EventDetails.FirstOrDefault(s => s.Id == eventDetailId && !s.Deleted)
                ?? throw new UserFriendlyException(ErrorCode.EventDetailNotFound);
            result.Id = eventDetailId;
            result.OrganizationDay = eventDetail.OrganizationDay;
            result.Status = eventDetail.Status;
            result.StartEventDate = eventDetail.StartSaleTicketDate;
            result.EndSaleTicketDate = eventDetail.EndSaleTicketDate;
            result.EventSeatMapImage = eventDetail.EventSeatMapImage;
            result.VenueId = eventDetail.VenueId;
            result.VenueName = _dbContext.Venues.Where(s => s.Id == result.VenueId).Select(s => s.Name).FirstOrDefault();
            result.EventId = eventDetail.EventId;

            var eventResult = _dbContext.Events.FirstOrDefault(s => s.Id == result.EventId && !s.Deleted)
                ?? throw new UserFriendlyException(ErrorCode.EventNotFound);
            result.EventName = eventResult.EventName;
            result.EventDescription = eventResult.EventDescription;
            result.EventName = eventResult.EventName;
            result.SupplierId = eventResult.SupplierId;
            result.SuppilerName = "tan";
            result.EventTypeId = eventResult.EventTypeId;
            result.EventTypeName = _dbContext.EventTypes.Where(s => s.Id == result.EventTypeId).Select(s => s.Name).FirstOrDefault();
            result.AdmissionPolicy = eventResult.AdmissionPolicy;
            result.ExchangePolicy = eventResult.ExchangePolicy;
            result.EventImage = eventResult.EventImage;


            var eventTickets = _dbContext.TicketEvents.Where(s => s.EventDetailId == result.Id && !s.Deleted);
            result.TicketEvents = eventTickets.ToList();
            return result;
        }
    }
}
