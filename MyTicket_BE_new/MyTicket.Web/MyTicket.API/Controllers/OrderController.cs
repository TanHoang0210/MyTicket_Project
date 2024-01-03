using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MYTICKET.BASE.SERVICE.Common;
using MYTICKET.UTILS;
using MYTICKET.WEB.SERVICE.OrderModule.Abstracts;
using MYTICKET.WEB.SERVICE.OrderModule.Dtos;

namespace MYTICKET.WEB.API.Controllers
{
    [Authorize]
    [Route("myticket/api/order")]
    [ApiController]

    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }
        /// <summary>
        /// Tạo mới đơn hàng
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("create")]
        public async Task<APIResponse<OrderDto>> CreateOrder(CreateOrderDto input)
               => new(await _orderService.CreateOrder(input));

        /// <summary>
        /// Danh sách vé của khách hàng
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet("find-all")]
        public APIResponse<PagingResult<OrderDetailDto>> FindAllOrderTicket([FromQuery] FilterOrderCustomer input)
               => new(_orderService.FindAllOrderByCustomerId(input));

        /// <summary>
        /// Thong tin ve khach hang
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("find-by-id")]
        public APIResponse<OrderDetailDto> FindOrderTicketById(int id)
               => new(_orderService.FindOrderTicketById(id));
        /// <summary>
        /// Đơn hàng khách hàng
        /// </summary>
        /// <returns></returns>
        [HttpGet("find-order/for-pay")]
        public APIResponse<OrderDto> FindOrderById()
               => new(_orderService.GetOrderReadyToPayByCustomer());

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("ticket/delete/{id}")]
        public async Task<APIResponse> DeleteOrderTicketById(int id)
        {
            await _orderService.DeleteOrderDetail(id);
            return new();
        }
        [HttpPut("update-order-status")]
        public async Task<APIResponse> UpdateStatus([FromBody] UpdateOrderStatusDto input)
        {

            await _orderService.UpdateOrderStatus(input);
            return new();

        }
        [HttpPut("transfer-ticker")]
        public async Task<APIResponse> TransferTicket([FromBody] TransferTicketDto input)
        {
            await _orderService.TransferTicket(input);
            return new();
        }

        [HttpPut("exchange-ticker")]
        public async Task<APIResponse> ExchangeTicket([FromBody] TransferTicketDto input)
        {
            await _orderService.ExchangeTicket(input);
            return new();
        }
        [AllowAnonymous]
        [HttpGet("tess")]
        public APIResponse<string> geeee(string id)
       => new(_orderService.QrTest(id));

        /// <summary>
        /// Danh sách vé chuyen nhuong của khách hàng
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet("transfer/find-all")]
        public APIResponse<PagingResult<TicketTransferDto>> FindAllTransferTicket([FromQuery] FilterOrderCustomer input)
               => new(_orderService.FindAllTransferCustomerId(input));

        /// <summary>
        /// Danh sách vé của khách hàng
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet("exchange/find-all")]
        public APIResponse<PagingResult<TicketExchangeDto>> FindAllExchangeTicket([FromQuery] FilterOrderCustomer input)
               => new(_orderService.FindAllExchangeCustomerId(input));

        /// <summary>
        /// Thong tin ve chuyen nhuong khach hang
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("transfer/find-by-id")]
        public APIResponse<TicketTransferDto> FindTransferTicketById(int id)
               => new(_orderService.FindTransferTicketById(id));
        /// <summary>
        /// Thong tin ve tra lai khach hang
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("exchange/find-by-id")]
        public APIResponse<TicketExchangeDto> FindExchangeTicketById(int id)
               => new(_orderService.FindExchangeTicketById(id));

        /// <summary>
        /// Huy chuyne nhuong ve
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut("cancel-tranfer")]
        public async Task<APIResponse> CancelTransferTicket([FromBody] TransferTicketDto input)
        {
            await _orderService.CancelTransferTicket(input);
            return new();
        }

        /// <summary>
        /// Huy tra lai ve
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut("cancel-exchange")]
        public async Task<APIResponse> CancelExchangeTicket([FromBody] TransferTicketDto input)
        {

            await _orderService.CancelExchangeTicket(input);
            return new();
        }

        /// <summary>
        /// Nhập mã xác nhận trả vé
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut("confirm-exchange")]
        public async Task<APIResponse> ConfirmExchangeTicket([FromBody] ConfirmExchangeTransferDto input)
        {
            await _orderService.ConfirmExchange(input);
            return new();

        }

        /// <summary>
        /// Nhập mã xác nhận chuyển nhượng vé
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut("confirm-transfer")]
        public async Task<APIResponse> ConfirmTransferTicket([FromBody] ConfirmExchangeTransferDto input)
        {
            await _orderService.ConfirmTransfer(input);
            return new();
        }

        /// <summary>
        /// Cập nhật trạng thái chuyển nhượng
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut("transfer/update-status")]
        public async Task<APIResponse> UpdateTransferStatus([FromBody] UpdateTransferStatusDto input)
        {
            await _orderService.UpdateTransferStatus(input);
            return new();
        }

        /// <summary>
        /// Danh sách vé của khách hàng
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet("admin/order/find-all")]
        public APIResponse<PagingResult<OrderDetailDto>> FindAllTicketByCustomerIdAdmin([FromQuery] FilterOrderCustomer input)
               => new(_orderService.FindAllOrderByCustomerIdAdmin(input));

        /// <summary>
        /// Danh sách vé của khách hàng
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet("admin/order-transfer/find-all")]
        public APIResponse<PagingResult<TicketTransferDto>> FindAllTicketTransferByCustomerIdAdmin([FromQuery] FilterOrderCustomer input)
               => new(_orderService.FindAllOrderTransferByCustomerId(input));

        /// <summary>
        /// Danh sách vé của khách hàng
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet("admin/transfer/find-all")]
        public APIResponse<PagingResult<TicketTransferDto>> FindAllTransferByCustomerIdAdmin([FromQuery] FilterOrderCustomer input)
               => new(_orderService.FindAllTransferCustomerIdAdmin(input));

        /// <summary>
        /// Danh sách vé của khách hàng
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet("admin/exchange/find-all")]
        public APIResponse<PagingResult<TicketExchangeDto>> FindAllExchangeByCustomerIdAdmin([FromQuery] FilterOrderCustomer input)
               => new(_orderService.FindAllExchangeCustomerIdAdmin(input));
    }
}
