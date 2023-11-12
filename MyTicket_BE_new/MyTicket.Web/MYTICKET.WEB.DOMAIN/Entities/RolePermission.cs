using Microsoft.EntityFrameworkCore;
using MYTICKET.BASE.ENTITIES.Entities;
using MYTICKET.UTILS.ConstantVaribale.Db;
using MYTICKET.UTILS.RolePermission;

namespace MYTICKET.WEB.DOMAIN.Entities
{
    [Table(nameof(RolePermission), Schema = DbSchemas.Default)]
    [Index(nameof(RoleId), nameof(PermissionKey), Name = $"IX_{nameof(RolePermission)}")]
    public class RolePermission : IRolePermission<int>
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int RoleId { get; set; }
        public Role Role { get; set; } = null!;

        /// <summary>
        /// Tên permission <br/>
        /// <see cref="PermissionConfig"/>
        /// </summary>
        [Required]
        [MaxLength(128)]
        [Unicode(false)]
        public string PermissionKey { get; set; } = null!;

        public DateTime? CreatedDate { get; set; }
        public int? CreatedBy { get; set; }
    }
}
