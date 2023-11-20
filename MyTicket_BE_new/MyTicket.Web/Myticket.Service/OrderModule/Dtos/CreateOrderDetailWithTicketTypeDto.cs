using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MYTICKET.WEB.SERVICE.OrderModule.Dtos
{
    public class CreateOrderDetailWithTicketTypeDto
    {
        public int EventDetailId { get; set; }

        public int TicketEventId { get; set; }

        public int Quantity { get; set; }
    }
}
