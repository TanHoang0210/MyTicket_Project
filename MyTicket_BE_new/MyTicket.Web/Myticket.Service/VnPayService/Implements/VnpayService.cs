using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MYTICKET.BASE.SERVICE.Common;
using MYTICKET.UTILS.ConstantVariables.Shared;
using MYTICKET.UTILS.CustomException;
using MYTICKET.WEB.SERVICE.Common;
using MYTICKET.WEB.SERVICE.VnPayService.Abstracts;
using MYTICKET.WEB.SERVICE.VnPayService.Dtos;
using MYTICKET.WEB.SERVICE.VnPayService.Libraries;

namespace MYTICKET.WEB.SERVICE.VnPayService.Implements
{
    public class VnpayService : ServiceBase, IVnpayService
    {
        private readonly IOptions<VNPaySettings> _appSettings;
        public VnpayService(IOptions<VNPaySettings> appSettings, ILogger<VnpayService> logger, IHttpContextAccessor httpContext) : base(logger, httpContext)
        {
            _appSettings = appSettings;
        }

        public string CreatePaymentUrl(PaymentInformationDto model, HttpContext context)
        {
            var tick = DateTime.Now.Ticks.ToString();
            
            var userId = CommonUtils.GetCurrentUserId(_httpContext);
            var currentUser = _dbContext.Users.Where(s => s.Id == userId).FirstOrDefault();
            var currentUserName = _dbContext.Customers.Where(s => s.Id == currentUser.CustomerId).FirstOrDefault();
            var orderTemp = (from ord in _dbContext.Orders
                             join orderDetail in _dbContext.OrderDetails on ord.Id equals orderDetail.OrderId
                             join ticket in _dbContext.Tickets on orderDetail.TicketId equals ticket.Id
                             where ord.CustomerId == currentUser.CustomerId && ord.Id == model.OrderId
                             select new
                             {
                                 Id = ord.Id,
                                 OrderType = orderDetail.EventDetailId,
                                 TotalAmount = ord.Total,
                                 CustomerName = currentUserName.LastName + " " + currentUserName.FirstName,
                                 OrderDescription = "Thanh toán đơn đặt vé My Ticket",
                             }).FirstOrDefault()
                             ?? throw new UserFriendlyException(ErrorCode.OrderNotFound);
            var order = new
            {
                Id = orderTemp.Id,
                OrderType = orderTemp.OrderType,
                TotalAmount = orderTemp.TotalAmount,
                CustomerName = orderTemp.CustomerName,
                OrderDescription = orderTemp.OrderDescription,
            };
            var pay = new VnPayLibrary();
            pay.AddRequestData("vnp_Version", VnPayLibrary.VERSION);
            pay.AddRequestData("vnp_Command", _appSettings.Value.Vnp_Command);
            pay.AddRequestData("vnp_TmnCode", _appSettings.Value.Vnp_TmnCode);
            pay.AddRequestData("vnp_Amount", ((long)order.TotalAmount * 100).ToString());
            pay.AddRequestData("vnp_CreateDate", DateTime.Now.ToString("yyyyMMddHHmmss"));
            pay.AddRequestData("vnp_CurrCode", _appSettings.Value.Vnp_CurrCode);
            pay.AddRequestData("vnp_IpAddr", "127.0.0.1");
            pay.AddRequestData("vnp_Locale", _appSettings.Value.Vnp_Locale);
            pay.AddRequestData("vnp_OrderInfo", order.Id.ToString());
            pay.AddRequestData("vnp_OrderType", order.OrderType.ToString());
            pay.AddRequestData("vnp_ReturnUrl", "http://localhost:8080/ticket/complete");
            pay.AddRequestData("vnp_TxnRef", tick);
            var paymentUrl =
            pay.CreateRequestUrl("https://sandbox.vnpayment.vn/paymentv2/vpcpay.html", _appSettings.Value.Vnp_SecureHash);

            return paymentUrl;
        }
    }
}
