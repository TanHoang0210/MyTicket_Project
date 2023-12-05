using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MYTICKET.WEB.SERVICE.OrderModule.Dtos
{
    public class TransferTicketDto
    {
        public int OrderDetailId { get; set; }

        public int CustomerTransferId { get; set; }
    }
}
