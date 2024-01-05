using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MYTICKET.WEB.SERVICE.TicketModule.Dtos
{
    public class TicketDto
    {
        public int Id { get; set; }

        public string? TicketCode { get; set; }
        public string? SeatCode { get; set; }

        public int Status {  get; set; }
    }
}
