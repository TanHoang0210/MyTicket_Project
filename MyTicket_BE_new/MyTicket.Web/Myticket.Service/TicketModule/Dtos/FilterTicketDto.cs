using Microsoft.AspNetCore.Mvc;
using MYTICKET.BASE.SERVICE.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MYTICKET.WEB.SERVICE.TicketModule.Dtos
{
    public class FilterTicketDto : PagingRequestBaseDto
    {
        [FromQuery(Name = "eventDetailId")]
        public int EventDetailId { get; set; }
    }
}
