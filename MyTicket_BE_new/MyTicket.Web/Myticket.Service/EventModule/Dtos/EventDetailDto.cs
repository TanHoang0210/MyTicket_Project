using MYTICKET.WEB.DOMAIN.Entities;
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
        /// <summary>
        /// id chi tiết sự kiện
        /// </summary>
        public int Id { get; set; }
        public int EventId { get; set; }
        /// <summary>
        /// Tên sự kiện
        /// </summary>
        public string? EventName { get; set; }

        /// <summary>
        /// id loại sự kiện
        /// </summary>
        public int? EventTypeId { get; set; }

        /// <summary>
        /// tên loại sự kiện
        /// </summary>
        public string? EventTypeName { get; set; }

        /// <summary>
        /// Id nhà cung cấp sự kiện
        /// </summary>
        public int? SupplierId { get; set; }

        public string? SuppilerName { get; set; }

        /// <summary>
        /// Mô tả
        /// </summary>
        public string? EventDescription { get; set; }
        /// <summary>
        /// chính sách đổi trả
        /// </summary>
        public string? ExchangePolicy { get; set; }
        /// <summary>
        /// Chính sách sự kiện
        /// </summary>
        public string? AdmissionPolicy { get; set; }
        /// <summary>
        /// ảnh sự kiện
        /// </summary>
        public string? EventImage { get; set; }

        /// <summary>
        /// trạng thái chi tiết sự kiện
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// Ngày sự kiện bắt đầu tổ chức
        /// </summary>
        public DateTime StartEventDate { get; set; }

        /// <summary>
        /// Id sân vận động, truyền vào nếu tổ chức ở svd
        /// </summary>
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
        public string? EventSeatMapImage { get; set; }
        public List<TicketEvent>? TicketEvents { get; set; }
    }
}
