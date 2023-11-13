using MYTICKET.WEB.DOMAIN.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MYTICKET.WEB.SERVICE.TicketModule.Dtos
{
    public class CreateTicketEventDto
    {
        private string _name = null!;
        public string Name
        {
            get => _name;
            set => _name = value;
        }

        public decimal Price { get; set; }

        public int Quantity { get; set; }
    }
}
