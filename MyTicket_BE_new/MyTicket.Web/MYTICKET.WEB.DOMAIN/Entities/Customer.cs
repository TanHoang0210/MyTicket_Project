using Microsoft.EntityFrameworkCore;
using MYTICKET.BASE.ENTITIES.Interfaces;
using MYTICKET.UTILS.ConstantVaribale.Db;

namespace MYTICKET.WEB.DOMAIN.Entities
{
    /// <summary>
    /// Customer
    /// </summary>
    [Table(nameof(Customer), Schema = DbSchemas.Default)]
    [Index(nameof(Deleted), nameof(LastName), Name = $"IX_{nameof(Customer)}")]
    public class Customer : IFullAudited
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        /// <summary>
        /// Tên đầy đủ
        /// </summary>
        [MaxLength(256)]
        public string FirstName { get; set; } = null!;

        /// <summary>
        /// Tên viết tắt
        /// </summary>
        [MaxLength(128)]
        public string LastName { get; set; } = null!;


        /// <summary>
        /// Quê quán
        /// </summary>
        [MaxLength(128)]
        [Unicode(false)]
        public int Country { get; set; }

        /// <summary>
        /// Quốc tịch
        /// </summary>
        [MaxLength(128)]
        [Unicode(false)]
        public int Nationality { get; set; }

        /// <summary>
        /// Địa chỉ
        /// </summary>
        [MaxLength(2024)]
        public string? Address { get; set; } = null!;

        /// <summary>
        /// Giới tính
        /// </summary>
        public int Gender { get; set; }

        /// <summary>
        /// Ngày sinh
        /// </summary>
        public DateTime? DateOfBirth { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? DeletedDate { get; set; }
        public bool Deleted { get; set; }
        public int? DeletedBy { get; set; }
    }
}
