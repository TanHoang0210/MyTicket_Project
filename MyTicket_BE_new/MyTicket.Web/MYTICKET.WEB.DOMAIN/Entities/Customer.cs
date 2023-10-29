using Microsoft.EntityFrameworkCore;
using MYTICKET.BASE.ENTITIES.Interfaces;
using MYTICKET.UTILS.ConstantVaribale.Db;

namespace MYTICKET.WEB.DOMAIN.Entities
{
    /// <summary>
    /// Customer
    /// </summary>
    [Table(nameof(Customer), Schema = DbSchemas.Default)]
    [Index(nameof(Deleted), nameof(FullName), nameof(ShortName), Name = $"IX_{nameof(Customer)}")]
    public class Customer : IFullAudited
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        /// <summary>
        /// Tên đầy đủ
        /// </summary>
        [MaxLength(256)]
        public string FullName { get; set; } = null!;

        /// <summary>
        /// Tên viết tắt
        /// </summary>
        [MaxLength(128)]
        public string? ShortName { get; set; }


        /// <summary>
        /// Số điện thoại
        /// </summary>
        [MaxLength(128)]
        [Unicode(false)]
        public string? Phone { get; set; }

        /// <summary>
        /// Địa chỉ email
        /// </summary>
        [MaxLength(128)]
        [Unicode(false)]
        public string? Email { get; set; }


        [MaxLength(2024)]
        public string Address { get; set; } = null!;
        [MaxLength(18)]
        public string TaxCode { get; set; } = null!;
        [MaxLength(18)]
        public string Language { get; set; } = null!;

        public DateTime? CreatedDate { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? DeletedDate { get; set; }
        public bool Deleted { get; set; }
        public int? DeletedBy { get; set; }
    }
}
