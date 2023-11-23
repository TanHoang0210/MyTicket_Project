using MYTICKET.BASE.SERVICE.Common;
using MYTICKET.WEB.SERVICE.OrderModule.Dtos;

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

        /// <summary>
        /// Xóa 1 vé khỏi đơn đặt vé
        /// </summary>
        /// <param name="id"></param>
        void DeleteOrderDetail(int id);

        void DeleteOrderExpired();

        /// <summary>
        /// lấy đơn hàng đang thanh toán
        /// </summary>
        /// <returns></returns>
        OrderDto GetOrderReadyToPayByCustomer();

        /// <summary>
        /// Cập nhật trạng thái đơn hàng
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task UpdateOrderStatus(UpdateOrderStatusDto input);
    }
}
