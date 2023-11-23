using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
    }
}
