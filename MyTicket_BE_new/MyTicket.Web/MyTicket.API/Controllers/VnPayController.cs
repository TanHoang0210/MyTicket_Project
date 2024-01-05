using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MYTICKET.UTILS;
using MYTICKET.WEB.SERVICE.VnPayService.Abstracts;
using MYTICKET.WEB.SERVICE.VnPayService.Dtos;

namespace MYTICKET.WEB.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class VnpayController : ControllerBase
    {
        private readonly IVnpayService _service;

        public VnpayController(IVnpayService service)
        {
            _service = service;
        }
        [HttpPost("payment-vn-pay")]
        public IActionResult CreateUrl([FromBody] PaymentInformationDto input)
        {
            return Ok(_service.CreatePaymentUrl(input, HttpContext));
        }

        [HttpPost("transfer/payment-vn-pay")]
        public IActionResult CreateTransferUrl([FromBody] TransferPaymentDto input)
        {
            return Ok(_service.CreatePaymentTransferUrl(input, HttpContext));
        }

        /// <summary>
        /// hoàn tiền mua vé
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("refund/payment-vn-pay")]
        public async Task<APIResponse> CreateRefundUrl([FromBody] RefundOrderDto input)
        {
            await _service.CreateRefundUrl(input, HttpContext);
           return new();
        }
        /// <summary>
        /// hoàn tiền trả vé
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("exchange/payment-vn-pay")]
        public async Task<APIResponse> CreateExchangeUrl([FromBody] RefundOrderDto input)
        {
            await _service.CreateRefundExchangeDoneUrl(input, HttpContext);
            return new();
        }
        /// <summary>
        /// hoàn tiền mua vé chuyển nhượng
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("refund/transfer/payment-vn-pay")]
        public async Task<APIResponse> CreateRefundTransferUrl([FromBody] RefundOrderDto input)
        {
            await _service.CreateRefundTransferUrl(input, HttpContext);
            return new();
        }

        /// <summary>
        /// hoàn tiền mua vé chuyển nhượng
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("refund/transfer-order/payment-vn-pay")]
        public async Task<APIResponse> CreateRefundTransferOrderUrl([FromBody] RefundOrderDto input)
        {
            await _service.CreateRefundTransferOrderUrl(input, HttpContext);
            return new();
        }
    }
}
