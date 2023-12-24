using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using MYTICKET.BASE.SERVICE.Common;
using MYTICKET.UTILS;
using MYTICKET.WEB.SERVICE.OrderModule.Abstracts;
using MYTICKET.WEB.SERVICE.OrderModule.Dtos;
using MYTICKET.WEB.SERVICE.VenueModule.Abstracts;
using MYTICKET.WEB.SERVICE.VenueModule.Dtos;
using MYTICKET.WEB.SERVICE.VenueModule.Implements;

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
        public async Task<IActionResult> DeleteOrderTicketById(int id)
        {
            try
            {
                await _orderService.DeleteOrderDetail(id);
                return Ok("Success");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPut("update-order-status")]
        public async Task<IActionResult> UpdateStatus([FromBody] UpdateOrderStatusDto input)
        {
            try
            {
                await _orderService.UpdateOrderStatus(input);
                return Ok("Success");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPut("transfer-ticker")]
        public async Task<IActionResult> TransferTicket([FromBody] TransferTicketDto input)
        {
            try
            {
                await _orderService.TransferTicket(input);
                return Ok("Success");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);

            }
        }

        [HttpPut("exchange-ticker")]
        public async Task<IActionResult> ExchangeTicket([FromBody] TransferTicketDto input)
        {
            try
            {
                await _orderService.ExchangeTicket(input);
                return Ok("Success");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);

            }
        }
        [AllowAnonymous]
        [HttpGet("tess")]
        public APIResponse<string>geeee(string id)
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
        public async Task<IActionResult> CancelTransferTicket([FromBody] TransferTicketDto input)
        {
            try
            {
                await _orderService.CancelTransferTicket(input);
                return Ok("Success");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Huy tra lai ve
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut("cancel-exchange")]
        public async Task<IActionResult> CancelExchangeTicket([FromBody] TransferTicketDto input)
        {
            try
            {
                await _orderService.CancelExchangeTicket(input);
                return Ok("Success");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);

            }
        }

        /// <summary>
        /// Nhập mã xác nhận trả vé
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut("confirm-exchange")]
        public async Task<IActionResult> ConfirmExchangeTicket([FromBody] ConfirmExchangeTransferDto input)
        {
            try
            {
                await _orderService.ConfirmExchange(input);
                return Ok("Success");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);

            }
        }

        /// <summary>
        /// Nhập mã xác nhận chuyển nhượng vé
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut("confirm-transfer")]
        public async Task<IActionResult> ConfirmTransferTicket([FromBody] ConfirmExchangeTransferDto input)
        {
            try
            {
                await _orderService.ConfirmTransfer(input);
                return Ok("Success");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);

            }
        }

        /// <summary>
        /// Cập nhật trạng thái chuyển nhượng
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut("transfer/update-status")]
        public async Task<IActionResult> UpdateTransferStatus([FromBody] UpdateTransferStatusDto input)
        {
            try
            {
                await _orderService.UpdateTransferStatus(input);
                return Ok("Success");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);

            }
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
        public APIResponse<PagingResult<OrderDetailDto>> FindAllTicketTransferByCustomerIdAdmin([FromQuery] FilterOrderCustomer input)
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
