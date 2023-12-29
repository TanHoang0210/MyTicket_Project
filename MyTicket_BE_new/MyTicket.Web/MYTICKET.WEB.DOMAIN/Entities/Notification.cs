using Microsoft.EntityFrameworkCore;
using MYTICKET.UTILS.ConstantVaribale.Db;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MYTICKET.WEB.DOMAIN.Entities
{
    [Table(nameof(Notification), Schema = DbSchemas.Default)]
    public class Notification
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string? Title { get; set; }

        public string? Description { get; set; }

        public bool IsSeen { get; set; }
        public DateTime CreateDate { get; set; }

        public int? CustomerId { get; set; }

        public int? EventDetailId { get; set; }

        public int? OrderId { get; set; }

        public int? OrderDetailId { get; set; }


    }
}
