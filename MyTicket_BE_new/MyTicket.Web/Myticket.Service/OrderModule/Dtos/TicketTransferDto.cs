using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MYTICKET.WEB.SERVICE.OrderModule.Dtos
{
    public class TicketTransferDto
    {
        public int Id { get; set; }

        public int OrderId { get; set; }

        public string? OrderCode { get; set; }

        public DateTime? OrderDate { get; set; }

        public int EventDetailId { get; set; }
        public string? EventName { get; set; }

        public DateTime? OrganizationDay { get; set; }
        public string? VenueName { get; set; }

        public string? VenueAddress { get; set; }

        public int TicketId { get; set; }
        public string? TicketEventName { get; set; }

        public string? TicketCode { get; set; }

        public string? SeatCode { get; set; }

        public decimal Price { get; set; }

        public int? Status { get; set; }

        public int? TransferStatus { get; set; }

        public DateTime? TransferDate { get; set; }
        public DateTime? TransferDoneDate { get; set; }
        public DateTime? TransferCancelDate { get; set; }
        public bool TransferRefundRequest { get; set; }
        public bool RefundRequest { get; set; }
    }
}
