using DocumentFormat.OpenXml.Office2010.Excel;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using MYTICKET.BASE.SERVICE.Common;
using MYTICKET.UTILS.ConstantVariables.Shared;
using MYTICKET.UTILS.CustomException;
using MYTICKET.UTILS.Linq;
using MYTICKET.WEB.DOMAIN.Entities;
using MYTICKET.WEB.SERVICE.Common;
using MYTICKET.WEB.SERVICE.EventModule.Dtos;
using MYTICKET.WEB.SERVICE.EventTypeModule.Abstracts;
using MYTICKET.WEB.SERVICE.EventTypeModule.Dtos;
using MYTICKET.WEB.SERVICE.VenueModule.Dtos;
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
                Description = input.Description,
                EventTypeImage = input.Image
            }).Entity;
            _dbContext.SaveChanges();
            return _mapper.Map<EventTypeDto>(add);
        }

        public PagingResult<EventTypeDto> FindAll(FitlerEventTypeDto input)
        {
            var result = new PagingResult<EventTypeDto>();
            _logger.LogInformation($"{nameof(FindAll)}");

            var query = _dbContext.EventTypes.Select(s => new EventTypeDto
            {
                Name = s.Name,
                Description= s.Description,
                Id = s.Id,
                Image = s.EventTypeImage
            });
            result.TotalItems = query.Count();
            query = query.OrderDynamic(input.Sort);

            if (input.PageSize != -1)
            {
                query = query.Skip(input.GetSkip()).Take(input.PageSize);
            }
            result.Items =query;
            return result;

        }

        public EventTypeDto FindById(int id)
        {
            var query = _dbContext.EventTypes.Where(s => s.Id == id && !s.Deleted).Select(s => new EventTypeDto
            {
                Name = s.Name,
                Description = s.Description,
                Id = s.Id,
                Image = s.EventTypeImage
            }).FirstOrDefault();
            return query;
        }

        public void Update(UpdateEventTypeDto input)
        {
            var query = _dbContext.EventTypes.Where(s => s.Id == input.Id && !s.Deleted).FirstOrDefault() ?? throw new UserFriendlyException(ErrorCode.EventNotFound);
            query.Name = input.Name;
            query.Description = input.Description;
            query.EventTypeImage = input.Image;
            _dbContext.SaveChanges();
        }
    }
}
