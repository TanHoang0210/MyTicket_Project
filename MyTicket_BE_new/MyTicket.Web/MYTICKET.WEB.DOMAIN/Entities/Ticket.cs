using Microsoft.EntityFrameworkCore;
using MYTICKET.UTILS.ConstantVaribale.Db;
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
        public int Id { get; set; }

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
        /// trạng thái
        /// </summary>
        public int Status { get; set; }
    }
}
