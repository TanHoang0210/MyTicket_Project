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
using Org.BouncyCastle.Asn1.Cmp;
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
            _dbContext.Events.Add(new Event
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
            });
            _dbContext.SaveChanges();
            transaction.Commit();
        }

        public void CreateEventDetail(CreateEventDetailDto input)
        {
            _logger.LogInformation($"{nameof(CreateEvent)}: input = {JsonSerializer.Serialize(input)}");
            var transaction = _dbContext.Database.BeginTransaction();
            var eventInfo = _dbContext.Events.FirstOrDefault(s => s.Id == input.EventId && s.Deleted) ?? throw new UserFriendlyException(ErrorCode.EventNotFound);
            var eventDetailAdd = _dbContext.EventDetails.Add(new EventDetail
            {
                EventId = input.EventId,
                VenueId = input.VenueId,
                Status = EventStatuses.INIT,
                StartSaleTicketDate = input.StartSaleTicketDate,
                EndSaleTicketDate = input.EndSaleTicketDate,
                OrganizationDay = input.OrganizationDay,
                EventSeatMapImage = input.EventSeatMapImage,
                HavingSeatMap = input.HavingSeatMap,
                SeatSelectType = input.SelectSeatType,
            }).Entity;
            _dbContext.SaveChanges();
            var startDateEvent = _dbContext.EventDetails.Select(s => s.OrganizationDay).Min();
            eventInfo.StartEventDate = startDateEvent;
            _dbContext.SaveChanges();
            transaction.Commit();
        }

        public PagingResult<EventDto> FindAll(FilterEventDto input)
        {
            _logger.LogInformation($"{nameof(FindAll)}: input = {JsonSerializer.Serialize(input)}");
            var result = new PagingResult<EventDto>();

            var query = (from ev in _dbContext.Events
                        join evType in _dbContext.EventTypes on ev.EventTypeId equals evType.Id
                        join evDetail in _dbContext.EventDetails on ev.Id equals evDetail.EventId into evDetails
                        where (!ev.Deleted
                               && (input.Keyword == null || ev.EventName.ToLower().Contains(input.Keyword.ToLower()))
                               && (input.EventTypeId == null || evType.Id == input.EventTypeId)
                               && ((input.StartDate == null || evDetails.Any(ed => ed.OrganizationDay.Date >= input.StartDate.Value.Date))
                               && (input.EndDate == null || evDetails.Any(ed => ed.OrganizationDay.Date <= input.EndDate.Value.Date))))
                        select new EventDto
                        {
                            Id = ev.Id,
                            EventName = ev.EventName,
                            EventDescription = ev.EventDescription,
                            EventImage = ev.EventImage,
                            EventTypeId = ev.EventTypeId,
                            EventTypeName = evType.Name,
                            FirstEventDate = evDetails.OrderBy(o => o.OrganizationDay).Select(s => s.OrganizationDay.Date).FirstOrDefault(),
                            LastEventDate = evDetails.OrderByDescending(o => o.OrganizationDay).Select(s => s.OrganizationDay.Date).FirstOrDefault(),
                            Status = ev.Status, // Assuming Status is a property of EventDetail,
                            Supllier = _dbContext.Suppilers.Where(s => s.Id == ev.SupplierId).Select(s => s.FullName).FirstOrDefault(),
                            SupplierId = ev.SupplierId
                        });
            result.TotalItems = query.Count();
            query = query.OrderByDescending(s => s.FirstEventDate);

            if (input.PageSize != -1)
            {
                query = query.Skip(input.GetSkip()).Take(input.PageSize);
            }

            result.Items = query;
            return result;
        }

        public List<EventDto> FindAllNewEvent()
        {
            var result = (from ev in _dbContext.Events
                        join evType in _dbContext.EventTypes on ev.EventTypeId equals evType.Id
                        join evDetail in _dbContext.EventDetails on ev.Id equals evDetail.EventId into evDetails
                        select new EventDto
                        {
                            Id = ev.Id,
                            EventName = ev.EventName,
                            EventDescription = ev.EventDescription,
                            EventImage = ev.EventImage,
                            EventTypeId = ev.EventTypeId,
                            EventTypeName = evType.Name,
                            FirstEventDate = evDetails.OrderBy(o => o.OrganizationDay).Select(s => s.OrganizationDay.Date).FirstOrDefault(),
                            LastEventDate = evDetails.OrderByDescending(o => o.OrganizationDay).Select(s => s.OrganizationDay.Date).FirstOrDefault(),
                            Status = ev.Status, // Assuming Status is a property of EventDetail,
                            Supllier = _dbContext.Suppilers.Where(s => s.Id == ev.SupplierId).Select(s => s.FullName).FirstOrDefault(),
                            SupplierId = ev.SupplierId
                        }).OrderByDescending(s => s.FirstEventDate).Take(9).ToList();
            return result;
        }

        public List<EventDto> FindAllOutStandingEvent()
        {
            var result = (from ev in _dbContext.Events
                          join evType in _dbContext.EventTypes on ev.EventTypeId equals evType.Id
                          join evDetail in _dbContext.EventDetails on ev.Id equals evDetail.EventId into evDetails
                          where (new int[] { EventStatuses.INIT, EventStatuses.ONSALE }.Contains(ev.Status) && !ev.Deleted && ev.IsOutStanding)
                          select new EventDto
                          {
                              Id = ev.Id,
                              EventName = ev.EventName,
                              EventDescription = ev.EventDescription,
                              EventImage = ev.EventImage,
                              EventTypeId = ev.EventTypeId,
                              EventTypeName = evType.Name,
                              FirstEventDate = evDetails.OrderBy(o => o.OrganizationDay).Select(s => s.OrganizationDay.Date).FirstOrDefault(),
                              LastEventDate = evDetails.OrderByDescending(o => o.OrganizationDay).Select(s => s.OrganizationDay.Date).FirstOrDefault(),
                              Status = ev.Status, // Assuming Status is a property of EventDetail,
                              Supllier = _dbContext.Suppilers.Where(s => s.Id == ev.SupplierId).Select(s => s.FullName).FirstOrDefault(),
                              SupplierId = ev.SupplierId
                          }).OrderByDescending(s => s.FirstEventDate).Take(9).ToList();
            return result;
        }

        public List<EventDto> FindAllTopSaleEvent()
        {
            var result = (from ev in _dbContext.Events
                          join evType in _dbContext.EventTypes on ev.EventTypeId equals evType.Id
                          join evDetail in _dbContext.EventDetails on ev.Id equals evDetail.EventId into evDetails
                          from evDetail in evDetails.DefaultIfEmpty()
                          join odDetail in _dbContext.OrderDetails on evDetail.Id equals odDetail.EventDetailId into odDetails
                          from odDetail in odDetails.DefaultIfEmpty()
                          where( new int[] { EventStatuses.INIT, EventStatuses.ONSALE}.Contains(ev.Status)  && !ev.Deleted)
                          select new EventDto
                          {
                              Id = ev.Id,
                              EventName = ev.EventName,
                              EventDescription = ev.EventDescription,
                              EventImage = ev.EventImage,
                              EventTypeId = ev.EventTypeId,
                              EventTypeName = evType.Name,
                              FirstEventDate = evDetails.OrderBy(o => o.OrganizationDay).Select(s => s.OrganizationDay.Date).FirstOrDefault(),
                              LastEventDate = evDetails.OrderByDescending(o => o.OrganizationDay).Select(s => s.OrganizationDay.Date).FirstOrDefault(),
                              Status = ev.Status, // Assuming Status is a property of EventDetail,
                              Supllier = _dbContext.Suppilers.Where(s => s.Id == ev.SupplierId).Select(s => s.FullName).FirstOrDefault(),
                              SupplierId = ev.SupplierId,
                              PercentSaleTicket = odDetails.Count()/(_dbContext.Tickets.Include(s => s.TicketEvent).Where(s => s.TicketEvent.EventDetailId == evDetail.Id).Count())
                          }).OrderByDescending(s => s.PercentSaleTicket).ThenByDescending(s => s.PercentSaleTicket).Take(9).ToList();
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
                    AdmissionPolicy = s.AdmissionPolicy,
                    Supllier = _dbContext.Suppilers.Where(x => x.Id == s.SupplierId).Select(s => s.FullName).FirstOrDefault(),
                    SupplierId = s.SupplierId
                })
                .FirstOrDefault()
               ?? throw new UserFriendlyException(ErrorCode.EventNotFound);

            var eventDetails = _dbContext.EventDetails
                .Include(s => s.Event)
                .Include(s => s.TicketEvents.Where(s => !s.Deleted))
                .ThenInclude(x => x.Tickets)
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
                    VenueAddress = _dbContext.Venues.Where(x => x.Id == s.VenueId).Select(s => s.Address).FirstOrDefault(),
                    TicketEvents = s.TicketEvents.Select(x => new TicketEventDto
                    {
                        Id = x.Id,
                        Name = x.Name,
                        EventDetailId = x.EventDetailId,
                        Price = x.Price,
                        Quantity = x.Tickets.Where(s => !_dbContext.OrderDetails.Include(t => t.Order)
                    .Any(o => o.TicketId == s.Id && o.Order.Status != OrderStatuses.CANCEL && !o.Deleted)).Take(10).Count(),
                        Status = (x.Tickets
        .Where(ticket => !_dbContext.OrderDetails
            .Include(orderDetail => orderDetail.Order)
            .Any(order => order.TicketId == ticket.Id && order.Order.Status != OrderStatuses.CANCEL && !order.Deleted))
        .Take(10)
        .Count() < 1) ? 3 : x.Status
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
                .ThenInclude(x => x.Tickets.Where(s => (!_dbContext.OrderDetails.Any(o => o.TicketId == s.Id))))
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
                        Quantity = x.Tickets.Where(s => !_dbContext.OrderDetails.Include(t => t.Order)
                    .Any(o => o.TicketId == s.Id && o.Order.Status != OrderStatuses.CANCEL && !o.Deleted)).Take(10).Count(),
                        Status = (x.Tickets
        .Where(ticket => !_dbContext.OrderDetails
            .Include(orderDetail => orderDetail.Order)
            .Any(order => order.TicketId == ticket.Id && order.Order.Status != OrderStatuses.CANCEL && !order.Deleted))
        .Take(10)
        .Count() < 1) ? 3 : x.Status
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
