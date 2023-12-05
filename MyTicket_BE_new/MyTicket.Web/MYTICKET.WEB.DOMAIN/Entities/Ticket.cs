using Microsoft.EntityFrameworkCore;
using MYTICKET.UTILS.ConstantVaribale.Db;
using MYTICKET.UTILS.ConstantVaribale.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MYTICKET.WEB.DOMAIN.Entities
{
    [Table(nameof(Ticket), Schema = DbSchemas.Default)]
    [Index(nameof(SeatCode), Name = $"IX_{nameof(Ticket)}")]
    public class Ticket
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }


        public string? TicketCode { get; set; }
        /// <summary>
        /// id loại sự kiện
        /// </summary>
        public int TicketEventId { get; set; }
        public TicketEvent? TicketEvent { get; set; }
        /// <summary>
        /// Mã chỗ ngồi
        /// </summary>
        [MaxLength(10)]
        public required string SeatCode { get; set; }

        /// <summary>
        /// Vé của khách hàng nào
        /// </summary>
        public List<OrderDetail> OrderDetails { get; } = new();
    }
}
