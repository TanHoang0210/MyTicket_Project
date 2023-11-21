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
    public class CreateEventDetailDto
    {
        /// <summary>
        /// Id sân vận động, truyền vào nếu tổ chức ở svd
        /// </summary>
        public int VenueId { get; set; }

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
        private string? _eventSeatMapImage;
        [MaxLength(1024)]
        public string? EventSeatMapImage
        {
            get => _eventSeatMapImage;
            set => _eventSeatMapImage = value?.Trim();
        }

        public bool HavingSeatMap {  get; set; }

        public int SelectSeatType { get; set; }
        public List<CreateTicketEventDto>? TicketEvents { get; set; }
    }
}
