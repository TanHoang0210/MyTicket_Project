using MYTICKET.WEB.DOMAIN.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MYTICKET.WEB.SERVICE.OrderModule.Dtos
{
    public class CreateOrderDto
    {
        /// <summary>
        /// ngày đặt
        /// </summary>
        public DateTime OrderDate { get; set; }

        public List<CreateOrderDetailWithTicketTypeDto>? TicketTypes { get; set; }
        public List<CreateOrderDetailWithTicketDto>? Tickets { get; set; }
    }
}
