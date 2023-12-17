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
    }
}
