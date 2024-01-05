using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MYTICKET.WEB.SERVICE.TicketModule.Dtos
{
    public class UpdateTicketStatusDto
    {
        public int Id {  get; set; }

        public int Status { get; set; }
    }
}
