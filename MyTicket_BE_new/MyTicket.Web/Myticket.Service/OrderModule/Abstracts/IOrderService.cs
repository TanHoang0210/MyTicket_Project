using MYTICKET.BASE.SERVICE.Common;
using MYTICKET.WEB.SERVICE.OrderModule.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MYTICKET.WEB.SERVICE.OrderModule.Abstracts
{
    public interface IOrderService
    {
        /// <summary>
        /// Tạo đơn đặt vé
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        OrderDto CreateOrder(CreateOrderDto input);

        /// <summary>
        /// Danh sách vé của khách hàng
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        PagingResult<OrderDetailDto> FindAllOrderByCustomerId(FilterOrderCustomer input);
    }
}
