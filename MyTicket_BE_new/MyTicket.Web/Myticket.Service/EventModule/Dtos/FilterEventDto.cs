using Microsoft.AspNetCore.Mvc;
using MYTICKET.BASE.SERVICE.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MYTICKET.WEB.SERVICE.EventModule.Dtos
{
    public class FilterEventDto : PagingRequestBaseDto
    {
        private string? _eventName;
        [FromQuery(Name = "eventName")]
        public  string? EventName 
        {
            get => _eventName;
            set => _eventName = value?.Trim();
        }
    }
}
