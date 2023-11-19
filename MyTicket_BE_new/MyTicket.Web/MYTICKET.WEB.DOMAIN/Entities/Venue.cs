using Microsoft.EntityFrameworkCore;
using MYTICKET.BASE.ENTITIES.Interfaces;
using MYTICKET.UTILS.ConstantVaribale.Db;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MYTICKET.WEB.DOMAIN.Entities
{
    [Table(nameof(Venue), Schema = DbSchemas.Default)]
    [Index(nameof(Deleted), nameof(Name),Name = $"IX_{nameof(Venue)}")]
    public class Venue : IFullAudited
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        /// <summary>
        /// Tên sân vận động
        /// </summary>
        [MaxLength(264)]
        public required string Name { get; set; }
        [MaxLength(264)]

        public string? Address { get; set; }
        public int Capacity { get; set; }
        [MaxLength(1024)]
        public string? Description { get; set; }

        public string? Image {  get; set; }
        #region FullAudited
        public DateTime? CreatedDate { get; set; }
        public int? CreatedBy { get ; set; }
        public DateTime? ModifiedDate { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? DeletedDate { get; set; }
        public bool Deleted { get; set; }
        public int? DeletedBy { get; set; }
        #endregion
    }
}
