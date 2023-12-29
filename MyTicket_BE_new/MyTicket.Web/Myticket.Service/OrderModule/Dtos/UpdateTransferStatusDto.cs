using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MYTICKET.WEB.SERVICE.OrderModule.Dtos
{
    public class UpdateTransferStatusDto
    {
        public int TicketId { get; set; }

        public int TransferStatus { get; set; }

        public int CustomerTransferOwnerId { get; set; }

        public string? TransferTransactionNo { get; set; }
    }
}
