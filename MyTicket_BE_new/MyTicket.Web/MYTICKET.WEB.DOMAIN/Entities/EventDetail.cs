using Microsoft.EntityFrameworkCore;
using MYTICKET.BASE.ENTITIES.Interfaces;
using MYTICKET.UTILS.ConstantVaribale.Db;
using System.Xml.Linq;

namespace MYTICKET.WEB.DOMAIN.Entities
{
    [Table(nameof(EventDetail), Schema = DbSchemas.Default)]
    [Index(nameof(EventId), nameof(OrganizationDay), Name = $"IX_{nameof(EventDetail)}")]
    public class EventDetail : IFullAudited
    {
        public int Id { get; set; }

        public int EventId { get; set; }
        public Event? Event { get; set; }
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
        [MaxLength(1024)]
        public string? EventSeatMapImage { get; set; }
        public int Status { get; set; }
        public List<TicketEvent> TicketEvents { get; } = new();

        public DateTime? CreatedDate { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? DeletedDate { get; set; }
        public bool Deleted { get; set; }
        public int? DeletedBy { get; set; }
    }
}
