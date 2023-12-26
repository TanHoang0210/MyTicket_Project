using MYTICKET.WEB.DOMAIN.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MYTICKET.WEB.SERVICE.TicketModule.Dtos
{
    public class TicketEventDto
    {
        public int Id { get; set; }
        public required string Name { get; set; }

        public decimal? Price { get; set; }

        public int? EventDetailId { get; set; }

        public int? Status { get; set; }

        public int? Quantity { get; set; }

        public int? IntQuantity { get; set; }
    }
}
