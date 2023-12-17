using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MYTICKET.WEB.SERVICE.TicketModule.Dtos
{
    public class TicketEventTransferDto
    {
        public int Id { get; set; }

        public int EventDetailId { get; set; }

        public string? TicketName { get; set; }

        public decimal Price { get; set; }

        public string? SeatCode { get; set; }

        public int CustomerTransferId { get; set; }
    }
}
