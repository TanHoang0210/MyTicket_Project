using Microsoft.EntityFrameworkCore;
using MYTICKET.BASE.ENTITIES.Interfaces;
using MYTICKET.UTILS.ConstantVaribale.Db;

namespace MYTICKET.WEB.DOMAIN.Entities
{
    [Table(nameof(TicketEvent), Schema = DbSchemas.Default)]
    [Index(nameof(EventDetailId), nameof(Name), Name = $"IX_{nameof(TicketEvent)}")]
    public class TicketEvent : IFullAudited
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [MaxLength(264)]
        public required string Name { get; set; }

        public decimal Price { get; set; }

        public int EventDetailId { get; set; }

        public EventDetail? EventDetail { get; set; }

        public int Status { get; set; }

        public List<Ticket> Tickets { get; } = new();
        public DateTime? CreatedDate { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? DeletedDate { get; set; }
        public bool Deleted { get; set; }
        public int? DeletedBy { get; set; }
    }
}
