using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using MYTICKET.WEB.DOMAIN.Entities;
using MYTICKET.WEB.SERVICE.Common;
using MYTICKET.WEB.SERVICE.EventTypeModule.Abstracts;
using MYTICKET.WEB.SERVICE.EventTypeModule.Dtos;
using MYTICKET.WEB.SERVICE.VenueModule.Implements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MYTICKET.WEB.SERVICE.EventTypeModule.Implements
{
    public class EventTypeService : ServiceBase, IEventTypeService
    {
        public EventTypeService(ILogger<EventTypeService> logger, IHttpContextAccessor httpContext) : base(logger, httpContext)
        {
        }

        public EventTypeDto Create(CreateEventTypeDto input)
        {
            _logger.LogInformation($"{nameof(Create)}: input = {JsonSerializer.Serialize(input)}");
            var add = _dbContext.EventTypes.Add(new EventType
            {
                Name = input.Name,
                Description = input.Description
            }).Entity;
            _dbContext.SaveChanges();
            return _mapper.Map<EventTypeDto>(add);
        }

        public List<EventTypeDto> FindAll()
        {
            _logger.LogInformation($"{nameof(FindAll)}");
            var result = _dbContext.EventTypes.Select(s => new EventTypeDto
            {
                Name = s.Name,
                Description= s.Description,
                Id = s.Id
            }).ToList();
            return result;
        }
    }
}
