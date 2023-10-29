using Microsoft.EntityFrameworkCore;
using MYTICKET.BASE.InfrastructureBase.Entities;
using MYTICKET.UTILS.ConstantVaribale.Db;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MYTICKET.WEB.DOMAIN.Entities
{
    [Table(nameof(Role), Schema = DbSchemas.Default)]
    [Index(nameof(Deleted), nameof(Name), Name = $"IX_{nameof(Role)}")]
    public class Role : IRole
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(256)]
        public string Name { get; set; } = null!;
        /// <summary>
        /// <see cref="UserTypes"/>
        /// </summary>
        public int UserType { get; set; }

        [MaxLength(1024)]
        public string? Description { get; set; }

        public DateTime? CreatedDate { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? DeletedDate { get; set; }
        public bool Deleted { get; set; }
        public int? DeletedBy { get; set; }
    }
}
