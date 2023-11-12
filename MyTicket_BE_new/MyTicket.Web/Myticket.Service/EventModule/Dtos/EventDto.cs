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
        public int? EventTypeName { get; set; }

        /// <summary>
        /// Mô tả
        /// </summary>
        public string? EventDescription { get; set; }

        /// <summary>
        /// ảnh sự kiện
        /// </summary>
        public string? EventImage { get; set; }

        /// <summary>
        /// trạng thái sự kiện
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// Ngày bắt đầu sự kiện
        /// </summary>
        public DateTime StartEventDate { get; set; }
    }
}
