using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MYTICKET.WEB.SERVICE.VnPayService.Dtos
{
    public class RefundExchangeDto
    {
        public int OrderDetailId { get; set; }
        public int CustomerId { get; set; }
    }
}
