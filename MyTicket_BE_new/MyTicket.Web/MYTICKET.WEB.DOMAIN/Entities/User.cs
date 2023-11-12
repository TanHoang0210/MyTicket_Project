using Microsoft.EntityFrameworkCore;
using MYTICKET.BASE.ENTITIES.Entities;
using MYTICKET.UTILS.ConstantVariables.User;
using MYTICKET.UTILS.ConstantVaribale.Db;

namespace MYTICKET.WEB.DOMAIN.Entities
{
    /// <summary>
    /// User
    /// </summary>
    [Table(nameof(User), Schema = DbSchemas.Default)]
    [Index(nameof(Deleted), nameof(Username), Name = $"IX_{nameof(User)}")]
    public class User : IUser
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(128)]
        [Unicode(false)]
        public string Username { get; set; } = null!;

        [Required]
        [MaxLength(128)]
        [Unicode(false)]
        public string Password { get; set; } = null!;

        [MaxLength(128)]
        [Unicode(false)]
        public string? Email { get; set; }

        [MaxLength(128)]
        [Unicode(false)]
        public string? Phone { get; set; }

        /// <summary>
        /// Loại user <see cref="UserTypes"/>
        /// </summary>
        public int UserType { get; set; }
        public int? CustomerId { get; set; }
        public int? SupplierId { get; set; }

        /// <summary>
        /// Trạng thái user <see cref="UserStatus"/>
        /// </summary>
        [Column("Status")]
        public int Status { get; set; }

        #region audit
        public DateTime? CreatedDate { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? DeletedDate { get; set; }
        public int? DeletedBy { get; set; }
        public bool Deleted { get; set; }
        #endregion
    }
}
