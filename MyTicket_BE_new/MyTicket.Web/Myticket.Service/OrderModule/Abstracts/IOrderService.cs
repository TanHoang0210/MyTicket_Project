using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
        Task<OrderDto> CreateOrder(CreateOrderDto input);

        /// <summary>
        /// Danh sách vé của khách hàng
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        PagingResult<OrderDetailDto> FindAllOrderByCustomerId(FilterOrderCustomer input);

        /// <summary>
        /// chi tiết vé khách hàng
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        OrderDetailDto FindOrderTicketById(int id);
        /// <summary>
        /// Xóa 1 vé khỏi đơn đặt vé
        /// </summary>
        /// <param name="id"></param>
        Task DeleteOrderDetail(int id);

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

        /// <summary>
        /// Chuyển nhượng vé
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task TransferTicket(TransferTicketDto input);


        /// <summary>
        /// trả vé
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task ExchangeTicket(TransferTicketDto input);
        string QrTest(string input);

        /// <summary>
        /// Danh sách vé chuyển nhượng của khách hàng
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        PagingResult<TicketTransferDto> FindAllTransferCustomerId(FilterOrderCustomer input);
        /// <summary>
        /// thoong tin ve chuyen nhuong
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        TicketTransferDto FindTransferTicketById(int id);
        /// <summary>
        /// Danh sách vé trả lại của khác hàng
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        PagingResult<TicketExchangeDto> FindAllExchangeCustomerId(FilterOrderCustomer input);
        /// <summary>
        /// Thong tin ve tra lai
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        TicketExchangeDto FindExchangeTicketById (int id);
        /// <summary>
        /// huy chuyen nhuong ve
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task CancelTransferTicket(TransferTicketDto input);

        /// <summary>
        /// huy tra ve
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task CancelExchangeTicket(TransferTicketDto input);

        /// <summary>
        /// Xác nhận yêu cầu khách hàng trả vé
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task ConfirmExchange(ConfirmExchangeTransferDto input);

        /// <summary>
        /// xác nhận yêu cầu chuyển nhượng vé
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task ConfirmTransfer(ConfirmExchangeTransferDto input);
        /// <summary>
        /// Cập nhật trạng thái chuyển nhượng
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task UpdateTransferStatus(UpdateTransferStatusDto input);


        /// <summary>
        /// Danh sách vé của khách hàng
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        PagingResult<OrderDetailDto> FindAllOrderByCustomerIdAdmin(FilterOrderCustomer input);

        /// <summary>
        /// Danh sách vé duoc chuyen nhuong cua kh
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        PagingResult<TicketTransferDto> FindAllOrderTransferByCustomerId(FilterOrderCustomer input);

        /// <summary>
        /// Danh sách vé chuyển nhượng của khách hàng
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        PagingResult<TicketTransferDto> FindAllTransferCustomerIdAdmin(FilterOrderCustomer input);

        /// <summary>
        /// Danh sách vé trả lại của khác hàng
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        PagingResult<TicketExchangeDto> FindAllExchangeCustomerIdAdmin(FilterOrderCustomer input);
    }
}
