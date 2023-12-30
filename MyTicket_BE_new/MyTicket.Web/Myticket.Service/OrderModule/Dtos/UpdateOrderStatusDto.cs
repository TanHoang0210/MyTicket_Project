using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MYTICKET.WEB.SERVICE.OrderModule.Dtos
{
    public class UpdateOrderStatusDto
    {
        public int Id { get; set; }

        public int Status {  get; set; }

        public string? TransactionNo { get; set; }

        public string? TransDate { get; set;}
    }
}
