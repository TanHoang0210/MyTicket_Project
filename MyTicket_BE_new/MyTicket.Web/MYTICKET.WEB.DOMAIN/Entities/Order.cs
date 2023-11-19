using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MYTICKET.BASE.ENTITIES.Interfaces;
using MYTICKET.UTILS.ConstantVaribale.Db;

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
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// ngày đặt
        /// </summary>
        public DateTime OrderDate {  get; set; } 

        public List<OrderDetail> OrderDetails { get; } = new();
        public string? QrCode { get; set; }
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
