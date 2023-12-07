using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MYTICKET.WEB.SERVICE.OrderModule.Dtos
{
    public class QRCodeDto
    {
        public int TicketId { get; set; }

        public string? TicketCode { get; set; }


        public string? SeatCode { get; set; }


        public int TicketEventId { get; set; }

        public string? TicketEventName { get; set; }

        public int OrderDetailId { get; set; }

        public int? Status { get; set; }
    }
}
