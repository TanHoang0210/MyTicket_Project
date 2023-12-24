using Microsoft.AspNetCore.Mvc;
using MYTICKET.BASE.SERVICE.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MYTICKET.WEB.SERVICE.OrderModule.Dtos
{
    public class FilterOrderCustomer : PagingRequestBaseDto
    {
        [FromQuery(Name = "customerId")]
        public int CustomerId { get; set; }

        [FromQuery(Name = "status")]
        public int? Status { get; set; }
    }
}
