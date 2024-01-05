using MYTICKET.WEB.DOMAIN.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MYTICKET.WEB.SERVICE.EventModule.Dtos
{
    public class EventDto
    {
        public int Id { get; set; }

        /// <summary>
        /// Tên sự kiện
        /// </summary>
        public required string EventName { get; set; }

        /// <summary>
        /// id loại sự kiện
        /// </summary>
        public int? EventTypeId { get; set; }

        /// <summary>
        /// id loại sự kiện
        /// </summary>
        public string? EventTypeName { get; set; }

        /// <summary>
        /// Mô tả
        /// </summary>
        public string? EventDescription { get; set; }

        /// <summary>
        /// ảnh sự kiện
        /// </summary>
        public string? EventImage { get; set; }


        public string? AdmissionPolicy { get; set; }

        public string? ExchangePolicy { get; set; }


        /// <summary>
        /// trạng thái sự kiện
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// Ngày bắt đầu sự kiện
        /// </summary>
        public DateTime? FirstEventDate { get; set; }

        /// <summary>
        /// Ngày bắt đầu sự kiện
        /// </summary>
        public DateTime? LastEventDate { get; set; }
        public int? SupplierId { get; set; }
        public string? Supllier { get; set; }

        public string? Address { get; set; }

        public IEnumerable<EventDetailDto>? EventDetails { get; set; }

        public double? PercentSaleTicket {get;set;}

        public bool IsExchange { get; set; }

        public bool IsOutStanding { get; set; }
    }
}
