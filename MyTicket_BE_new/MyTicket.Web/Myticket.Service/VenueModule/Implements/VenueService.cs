using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MYTICKET.BASE.SERVICE.Common;
using MYTICKET.UTILS.ConstantVariables.Shared;
using MYTICKET.UTILS.CustomException;
using MYTICKET.UTILS.Linq;
using MYTICKET.WEB.DOMAIN.Entities;
using MYTICKET.WEB.SERVICE.Common;
using MYTICKET.WEB.SERVICE.EventModule.Dtos;
using MYTICKET.WEB.SERVICE.VenueModule.Abstracts;
using MYTICKET.WEB.SERVICE.VenueModule.Dtos;
using System.Text.Json;

namespace MYTICKET.WEB.SERVICE.VenueModule.Implements
{
    public class VenueService : ServiceBase, IVenueService
    {
        public VenueService(ILogger<VenueService> logger, IHttpContextAccessor httpContext) : base(logger, httpContext)
        {
        }

        public VenueDto Create(CreateVenueDto input)
        {
            _logger.LogInformation($"{nameof(Create)}: input = {JsonSerializer.Serialize(input)}");
            var result = _dbContext.Add(_mapper.Map<Venue>(input)).Entity;
            _dbContext.SaveChanges();
            return _mapper.Map<VenueDto>(result);
        }

        public void Delete(int id)
        {
            _logger.LogInformation($"{nameof(Delete)}: id = {id}");
            var venue = _dbContext.Venues.FirstOrDefault(p => p.Id == id && !p.Deleted)
                        ?? throw new UserFriendlyException(ErrorCode.VenueNotFound);
            venue.Deleted = true;
            _dbContext.SaveChanges();
        }

        public PagingResult<VenueDto> GetAll(FilterVenueDto input)
        {
            _logger.LogInformation($"{nameof(GetAll)}: input = {JsonSerializer.Serialize(input)}");
            var result = new PagingResult<VenueDto>();
            var query = _dbContext.Venues.Where(p => !p.Deleted
                                                      && (input.Name == null || p.Name.ToLower().Contains(input.Name.ToLower())));

            result.TotalItems = query.Count();
            query = query.OrderDynamic(input.Sort);

            if (input.PageSize != -1)
            {
                query = query.Skip(input.GetSkip()).Take(input.PageSize);
            }
            result.Items = _mapper.Map<IEnumerable<VenueDto>>(query);
            return result;
        }

        public VenueDetailDto GetById(int venueId)
        {
            _logger.LogInformation($"{nameof(GetById)}: venueId = {venueId}");
            var result = (from venue in _dbContext.Venues
                         join venueEvent in _dbContext.EventDetails on venue.Id equals venueEvent.VenueId into venues
                         from venueEvent in venues.DefaultIfEmpty()
                         join eventt in _dbContext.Events on venueEvent.EventId equals eventt.Id into eventtEvents
                         from eventt in eventtEvents.DefaultIfEmpty()
                         select new VenueDetailDto
                         {
                             Name = venue.Name,
                             Id = venue.Id,
                             Address = venue.Address,
                             Capacity = venue.Capacity,
                             Description = venue.Description,
                             Image = venue.Image,
                             EventVenues = eventtEvents.Select(s => new EventDto 
                             {
                                 EventName = s.EventName,
                                 EventDescription = s.EventDescription,
                                 EventImage = s.EventImage,
                                 EventTypeId = s.EventTypeId,
                                 Id = s.Id,
                                 StartEventDate = s.StartEventDate,
                                 Status = s.Status
                             })
                             
                         }).FirstOrDefault() ?? throw new UserFriendlyException(ErrorCode.VenueNotFound);
            return result;
        }

        public VenueDto Update(UpdateVenueDto input)
        {
            _logger.LogInformation($"{nameof(Update)}: input = {JsonSerializer.Serialize(input)}");
            var venue = _dbContext.Venues.FirstOrDefault(p => p.Id == input.Id && !p.Deleted)
                          ?? throw new UserFriendlyException(ErrorCode.VenueNotFound);
            _mapper.Map(input, venue);
            _dbContext.SaveChanges();
            return _mapper.Map<VenueDto>(venue);
        }
    }
}
