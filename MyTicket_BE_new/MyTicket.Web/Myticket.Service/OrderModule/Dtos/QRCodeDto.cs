using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MYTICKET.WEB.SERVICE.OrderModule.Dtos
{
    public class QRCodeDto
    {
        public int CustomerId { get; set; }
        public int TicketId { get; set; }

        public int OrderDetailId { get; set; }

        public int? Status { get; set; }
    }
}
