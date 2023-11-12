using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MYTICKET.WEB.SERVICE.EventTypeModule.Dtos
{
    public class UpdateEventTypeDto : CreateEventTypeDto
    {
        public int Id { get; set; }
    }
}
