﻿using MYTICKET.WEB.DOMAIN.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MYTICKET.WEB.SERVICE.OrderModule.Dtos
{
    public class OrderDetailDto
    {
        public int Id { get; set; }

        public int OrderId { get; set; }

        public int EventDetailId { get; set; }
        public string? EventName { get; set; }

        public DateTime? OrganizationDay { get; set; }
        public string? VenueName { get; set; }

        public int TicketId { get; set; }
        public string? TicketEventName { get; set; }

        public string? TicketCode { get; set; }

        public string? SeatCode { get; set; }

        public decimal Price { get; set; }
    }
}