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

        OrderDto FindAllOrderByCustomerId(FilterOrderCustomer input);
    }
}
