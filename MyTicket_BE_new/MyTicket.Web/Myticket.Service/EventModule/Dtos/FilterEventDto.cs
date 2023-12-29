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
        [FromQuery(Name = "supplierId")]
        public int? SupplierId { get; set; }
        [FromQuery(Name = "eventTypeId")]
        public int? EventTypeId { get; set; }
        [FromQuery(Name = "startDate")]
        public DateTime? StartDate { get; set; }

        [FromQuery(Name = "endDate")]
        public DateTime? EndDate { get; set; }

        [FromQuery(Name = "status")]
        public int? Status {  get; set; }
    }
}
