using Microsoft.EntityFrameworkCore;
using MYTICKET.BASE.ENTITIES.Interfaces;
using MYTICKET.UTILS.ConstantVaribale.Db;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MYTICKET.WEB.DOMAIN.Entities
{
    /// <summary>
    /// Suppiler
    /// </summary>
    [Table(nameof(Suppiler), Schema = DbSchemas.Default)]
    [Index(nameof(Deleted), nameof(FullName), nameof(ShortName), Name = $"IX_{nameof(Suppiler)}")]
    public class Suppiler : IFullAudited
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


        [MaxLength(2024)]
        public string Address { get; set; } = null!;
        [MaxLength(18)]
        public string TaxCode { get; set; } = null!;

        public List<Event> Events { get; } = new();



        public DateTime? CreatedDate { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? DeletedDate { get; set; }
        public bool Deleted { get; set; }
        public int? DeletedBy { get; set; }
    }
}
