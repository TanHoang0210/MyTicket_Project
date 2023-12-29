using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MYTICKET.BASE.ENTITIES.Interfaces;
using MYTICKET.UTILS.ConstantVaribale.Db;
using MYTICKET.UTILS.ConstantVaribale.Shared;

namespace MYTICKET.WEB.DOMAIN.Entities
{
    [Table(nameof(Order), Schema = DbSchemas.Default)]
    [Index(nameof(CustomerId), Name = $"IX_{nameof(Order)}")]
    public class Order : IFullAudited
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        /// <summary>
        /// id đơn đặt vé
        /// </summary>
        public int Id { get; set; }

        public required string OrderCode { get; set; }
        /// <summary>
        /// Id khách hàng đặt vé
        /// </summary>
        public int CustomerId { get; set; }

        public Customer Customer { get; set; } = null!;

        /// <summary>
        /// trạng thái đơn đặt
        /// <see cref="OrderStatuses"/>
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// ngày đặt
        /// </summary>
        public DateTime OrderDate {  get; set; }

        /// <summary>
        /// Hết hạn thanh toán
        /// </summary>
        public DateTime ExpireDate { get; set; }

        public decimal Total { get; set; }

        public string? TransactionNo { get; set; }

        public string? BackgroundJobId { get; set; }

        public List<OrderDetail> OrderDetails { get; } = new();
        #region fullaudit
        public DateTime? CreatedDate { get ; set ; }
        public int? CreatedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? DeletedDate { get; set; }
        public bool Deleted { get; set; }
        public int? DeletedBy { get; set; }
        #endregion
    }
}
