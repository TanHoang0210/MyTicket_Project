using MYTICKET.WEB.DOMAIN.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MYTICKET.WEB.SERVICE.OrderModule.Dtos
{
    public class OrderDto
    {
        public int Id { get; set; }

        public required string OrderCode { get; set; }
        /// <summary>
        /// Id khách hàng đặt vé
        /// </summary>
        public int CustomerId { get; set; }

        /// <summary>
        /// trạng thái đơn đặt
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// ngày đặt
        /// </summary>
        public DateTime OrderDate { get; set; }

        public decimal? Total { get; set; }

        public bool RefundRequest { get; set; }

        public List<OrderDetailDto>? OrderDetails { get; set; }
    }
}
