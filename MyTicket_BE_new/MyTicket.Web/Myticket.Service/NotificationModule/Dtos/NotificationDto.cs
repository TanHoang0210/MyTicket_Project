using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MYTICKET.WEB.SERVICE.NotificationModule.Dtos
{
    public class NotificationDto
    {
        public int Id { get; set; }
        public string? Title { get; set; }

        public string? Description { get; set; }

        public bool IsSeen { get; set; }
        public DateTime CreateDate { get; set; }

        public int? CustomerId { get; set; }

        public int? EventDetailId { get; set; }
    }
}
