using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MYTICKET.WEB.SERVICE.EventTypeModule.Dtos
{
    public class EventTypeDto
    {
        public int Id { get; set; }

        public required string Name { get; set; }

        public string? Description { get; set; }
        public string? EventTypeImage{ get; set; }
    }
}
