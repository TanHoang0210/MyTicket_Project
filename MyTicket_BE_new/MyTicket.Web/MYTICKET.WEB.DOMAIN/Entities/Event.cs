using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MYTICKET.BASE.ENTITIES.Interfaces;
using MYTICKET.UTILS.ConstantVaribale.Db;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MYTICKET.WEB.DOMAIN.Entities
{
    [Table(nameof(Event), Schema = DbSchemas.Default)]
    [Index(nameof(EventName), nameof(EventTypeId), Name = $"IX_{nameof(Event)}")]
    public class Event : IFullAudited
    {
        public int Id { get; set; }

        /// <summary>
        /// Tên sự kiện
        /// </summary>
        [MaxLength(264)]
        public required string EventName { get; set; }

        /// <summary>
        /// id loại sự kiện
        /// </summary>
        public int? EventTypeId { get; set; }

        public EventType? EventType { get; set; }

        /// <summary>
        /// Id nhà cung cấp sự kiện
        /// </summary>
        public int? SupplierId { get; set; }

        public Suppiler? Suppiler { get; set; }

        /// <summary>
        /// Mô tả
        /// </summary>
        [MaxLength(1024)]
        public string? EventDescription { get; set; }
        /// <summary>
        /// chính sách đổi trả
        /// </summary>
        [MaxLength(1024)]
        public string? ExchangePolicy { get; set; }
        /// <summary>
        /// Chính sách sự kiện
        /// </summary>
        [MaxLength(1024)]
        public string? AdmissionPolicy { get; set; }
        /// <summary>
        /// ảnh sự kiện
        /// </summary>
        [MaxLength(1024)]
        public string? EventImage { get; set; }

        /// <summary>
        /// trạng thái sự kiện
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// Ngày sự kiện bắt đầu tổ chức
        /// </summary>
        public DateTime StartEventDate { get; set; }
        public List<EventDetail> EventDetails { get;} = new();
        public DateTime? CreatedDate { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? DeletedDate { get; set; }
        public bool Deleted { get; set; }
        public int? DeletedBy { get; set; }
    }
}
