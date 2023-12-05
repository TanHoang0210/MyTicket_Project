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
        /// Danh sách vé của khách hàng
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
    }
}
