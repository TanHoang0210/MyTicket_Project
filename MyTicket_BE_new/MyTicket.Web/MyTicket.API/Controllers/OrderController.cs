using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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
        public APIResponse<OrderDto> CreateOrder(CreateOrderDto input)
               => new(_orderService.CreateOrder(input));

        /// <summary>
        /// Danh sách vé của khách hàng
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet("find/{id}")]
        public APIResponse<PagingResult<OrderDetailDto>> FindVenueById([FromQuery]FilterOrderCustomer input)
               => new(_orderService.FindAllOrderByCustomerId(input));
    }
}
