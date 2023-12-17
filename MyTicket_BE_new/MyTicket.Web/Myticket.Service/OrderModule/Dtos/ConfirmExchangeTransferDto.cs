using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MYTICKET.WEB.SERVICE.OrderModule.Dtos
{
    public class ConfirmExchangeTransferDto
    {
        public int Id { get; set; }

        public string ConfirmCode { get; set; } = null!;
    }
}
