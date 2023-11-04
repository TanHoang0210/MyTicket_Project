using Microsoft.EntityFrameworkCore;
using MYTICKET.BASE.ENTITIES.Interfaces;
using MYTICKET.UTILS.ConstantVaribale.Db;

namespace MYTICKET.WEB.DOMAIN.Entities
{
    [Table(nameof(EventType), Schema = DbSchemas.Default)]
    [Index(nameof(Name), Name = $"IX_{nameof(EventType)}")]
    public class EventType : IFullAudited
    {
        public int Id { get; set; }

        /// <summary>
        /// Tên Loaị sự kiện
        /// </summary>
        [MaxLength(264)]
        public required string Name { get; set; }
        /// <summary>
        /// Mô tả
        /// </summary>
        [MaxLength(1024)]
        public string? Description { get; set; }

        public List<Event> Events { get; } = new();
        #region FullAudit
        public DateTime? CreatedDate { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? DeletedDate { get; set; }
        public bool Deleted { get; set; }
        public int? DeletedBy { get; set; }
        #endregion
    }
}
