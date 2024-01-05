using Microsoft.AspNetCore.Http;
using MYTICKET.WEB.SERVICE.VnPayService.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MYTICKET.WEB.SERVICE.VnPayService.Abstracts
{
    public interface IVnpayService
    {
        public string CreatePaymentUrl(PaymentInformationDto model, HttpContext context);

        public string CreatePaymentTransferUrl(TransferPaymentDto model, HttpContext context);

        /// <summary>
        /// Hoàn tiền mua vé
        /// </summary>
        /// <param name="model"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public Task CreateRefundUrl(RefundOrderDto model, HttpContext context);
        /// <summary>
        /// Hoanf tiền cho khách hàng mua vé chuyển nhượng
        /// </summary>
        /// <param name="model"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public Task CreateRefundTransferUrl(RefundOrderDto model, HttpContext context);

        /// <summary>
        /// Hoàn tiền trả vé thành công
        /// </summary>
        /// <param name="model"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public Task CreateRefundExchangeDoneUrl(RefundOrderDto model, HttpContext context);
        /// <summary>
        /// hoàn tiền cho khách chuyênr nhượng vé thành công
        /// </summary>
        /// <param name="model"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public Task CreateRefundTransferOrderUrl(RefundOrderDto model, HttpContext context);

    }
}
