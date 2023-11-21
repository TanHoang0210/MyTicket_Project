using MYTICKET.WEB.DOMAIN.Entities;
using MYTICKET.WEB.SERVICE.TicketModule.Dtos;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MYTICKET.WEB.SERVICE.EventModule.Dtos
{
    public class EventDetailDto
    {
        public int Id { get; set; }

        public int EventId { get; set; }
        public string? EventName { get; set; }
        public string? EventImage { get; set; }
        public int VenueId { get; set; }
        public string? VenueName { get; set; }

        /// <summary>
        /// Ngày diễn ra sự kiện
        /// </summary>
        public DateTime OrganizationDay { get; set; }

        /// <summary>
        /// Ngày bắt đầu bán vé
        /// </summary>
        public DateTime StartSaleTicketDate { get; set; }

        /// <summary>
        /// Ngày kết thúc bán vé
        /// </summary>
        public DateTime EndSaleTicketDate { get; set; }
        /// <summary>
        /// ảnh map sự kiện
        /// </summary>
        [MaxLength(1024)]
        public string? EventSeatMapImage { get; set; }

        public int SeatSelectType { get; set; }
        /// <summary>
        /// có sơ đồ để chọn vé không
        /// </summary>
        public bool HavingSeatMap { get; set; }
        public int Status { get; set; }
        public IEnumerable<TicketEventDto>? TicketEvents { get; set; }
    }
}
