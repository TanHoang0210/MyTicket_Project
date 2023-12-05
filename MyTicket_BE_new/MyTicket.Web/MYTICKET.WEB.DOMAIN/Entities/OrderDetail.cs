using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MYTICKET.BASE.ENTITIES.Interfaces;
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
    [Table(nameof(OrderDetail), Schema = DbSchemas.Default)]
    [Index(nameof(EventDetailId), Name = $"IX_{nameof(OrderDetail)}")]
    public class OrderDetail :IFullAudited
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int OrderId { get; set; }
        public Order Order { get; set; } = null!;
        public int EventDetailId { get; set; }

        public EventDetail EventDetail { get; set; } = null!;

        public int TicketId { get; set;}

        public Ticket Ticket { get; set; } = null!;
        /// <summary>
        /// chuyển nhượng vé
        /// </summary>
        public int? IsTransfer {  get; set; }
        /// <summary>
        /// <see cref="TransferStatuses"/>
        /// </summary>
        public int? TransferStatus { get; set; }
        /// <summary>
        /// mã xác nhận chuyển 
        /// </summary>
        public string? TransferCode { get; set; }
        /// <summary>
        /// người được chuyển nhượng
        /// </summary>
        public int? CustomerTransfer {  get; set; }

        public int? IsExchange { get; set; }

        public int? ExchangeStatus { get; set; }
        public string? ExchangeCode { get; set; }
        public string? QrCode { get; set; }

        #region fulldaudit
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
