using Hangfire;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MYTICKET.BASE.SERVICE.Common;
using MYTICKET.UTILS.ConstantVariables.Shared;
using MYTICKET.UTILS.ConstantVaribale.Shared;
using MYTICKET.UTILS.CustomException;
using MYTICKET.WEB.SERVICE.Common;
using MYTICKET.WEB.SERVICE.MailService.Abstracts;
using MYTICKET.WEB.SERVICE.MailService.Dtos;
using MYTICKET.WEB.SERVICE.SystemModule.Abstracts;
using MYTICKET.WEB.SERVICE.VnPayService.Abstracts;
using MYTICKET.WEB.SERVICE.VnPayService.Dtos;
using MYTICKET.WEB.SERVICE.VnPayService.Libraries;
using System.Text;
using System.Text.Json;

namespace MYTICKET.WEB.SERVICE.VnPayService.Implements
{
    public class VnpayService : ServiceBase, IVnpayService
    {
        private IHttpClientFactory _httpClientFactory;
        private readonly IEmailSenderService _mail;
        private readonly IOptions<VNPaySettings> _appSettings;
        public VnpayService(IOptions<VNPaySettings> appSettings, IEmailSenderService mail, IHttpClientFactory httpClientFactory, ILogger<VnpayService> logger, IHttpContextAccessor httpContext) : base(logger, httpContext)
        {
            _appSettings = appSettings;
            _httpClientFactory = httpClientFactory;
            _mail = mail;
        }

        public string CreatePaymentTransferUrl(TransferPaymentDto model, HttpContext context)
        {
            var tick = DateTime.Now.Ticks.ToString();
            var ticketTemp = (from ticket in _dbContext.Tickets
                              join ticketEvent in _dbContext.TicketEvents on ticket.TicketEventId equals ticketEvent.Id
                              join orderDetail in _dbContext.OrderDetails on ticket.Id equals orderDetail.TicketId
                              join od in _dbContext.Orders on orderDetail.OrderId equals od.Id into orders
                              from od in orders.DefaultIfEmpty()
                              where ticket.Id == model.TicketId && od.Status == OrderStatuses.SUCCESS && !od.Deleted
                              select new
                              {
                                  TicketId = ticket.Id,
                                  CustomerTransferOwnerId = od.CustomerId,
                                  TotalAmount = ticketEvent.Price,
                                  test = orderDetail
                              }).FirstOrDefault();
            var transfer = new
            {
                TicketId = ticketTemp.TicketId,
                CustomerTransferOwnerId = ticketTemp.CustomerTransferOwnerId,
                TotalAmount = ticketTemp.TotalAmount
            };
            var orderInfo = transfer.TicketId.ToString() + "_" + transfer.CustomerTransferOwnerId.ToString();
            var pay = new VnPayLibrary();
            pay.AddRequestData("vnp_Version", VnPayLibrary.VERSION);
            pay.AddRequestData("vnp_Command", _appSettings.Value.Vnp_Command);
            pay.AddRequestData("vnp_TmnCode", _appSettings.Value.Vnp_TmnCode);
            pay.AddRequestData("vnp_Amount", ((long)transfer.TotalAmount * 100).ToString());
            pay.AddRequestData("vnp_CreateDate", DateTime.Now.ToString("yyyyMMddHHmmss"));
            pay.AddRequestData("vnp_CurrCode", _appSettings.Value.Vnp_CurrCode);
            pay.AddRequestData("vnp_IpAddr", "127.0.0.1");
            pay.AddRequestData("vnp_Locale", _appSettings.Value.Vnp_Locale);
            pay.AddRequestData("vnp_OrderInfo", orderInfo.ToString());
            pay.AddRequestData("vnp_OrderType", transfer.CustomerTransferOwnerId.ToString());
            pay.AddRequestData("vnp_ExpireDate", DateTime.Now.AddMinutes(5).ToString("yyyyMMddHHmmss"));
            pay.AddRequestData("vnp_ReturnUrl", "http://localhost:8080/transfer/ticket/complete");
            pay.AddRequestData("vnp_TxnRef", tick);
            var paymentUrl =
            pay.CreateRequestUrl("https://sandbox.vnpayment.vn/paymentv2/vpcpay.html", _appSettings.Value.Vnp_SecureHash);

            return paymentUrl;
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
            pay.AddRequestData("vnp_ExpireDate", DateTime.Now.AddMinutes(5).ToString("yyyyMMddHHmmss"));
            pay.AddRequestData("vnp_TxnRef", tick);
            var paymentUrl =
            pay.CreateRequestUrl("https://sandbox.vnpayment.vn/paymentv2/vpcpay.html", _appSettings.Value.Vnp_SecureHash);

            return paymentUrl;
        }

        public async Task CreateRefundUrl(RefundOrderDto model, HttpContext context)
        {
            var user = CommonUtils.GetCurrentUserId(_httpContext);
            var tick = DateTime.Now.Ticks.ToString();

            var orderTemp = (from ord in _dbContext.Orders
                             join cus in _dbContext.Customers on ord.CustomerId equals cus.Id
                             join use in _dbContext.Users on cus.Id equals use.CustomerId
                             join orderDetail in _dbContext.OrderDetails on ord.Id equals orderDetail.OrderId
                             join eventDt in _dbContext.EventDetails on orderDetail.EventDetailId equals eventDt.Id
                             join evt in _dbContext.Events on eventDt.EventId equals evt.Id
                             join ticket in _dbContext.Tickets on orderDetail.TicketId equals ticket.Id
                             join ticketType in _dbContext.TicketEvents on ticket.TicketEventId equals ticketType.Id
                             where ord.CustomerId == model.CustomerId && orderDetail.Id == model.OrderDetail
                             select new
                             {
                                 Id = ord.Id,
                                 OrderType = orderDetail.EventDetailId,
                                 TotalAmount = ord.Total,
                                 OrderAmount = orderDetail.Price,
                                 OrderDescription = "Hoàn tiền đơn đặt vé My Ticket",
                                 TransactionNo = ord.TransactionNo,
                                 TransactionDate = ord.TransDate,
                                 CustomerMail = use.Email,
                                 EventName = evt.EventName,
                                 TicketType = ticketType.Name,
                                 TicketCode = ticket.TicketCode,
                                 SeatCode = ticket.SeatCode,
                                 OrganDay = eventDt.OrganizationDay
                             }).FirstOrDefault()
                             ?? throw new UserFriendlyException(ErrorCode.OrderNotFound);

            var order = new
            {
                Id = orderTemp.Id,
                OrderType = orderTemp.OrderType,
                TotalAmount = orderTemp.TotalAmount,
                OrderAmount = orderTemp.OrderAmount,
                OrderDescription = orderTemp.OrderDescription,
                TransactionNo = orderTemp.TransactionNo,
                TransactionDate = orderTemp.TransactionDate,
            };
            var vnp_RequestId = DateTime.Now.Ticks.ToString(); //Mã hệ thống merchant tự sinh ứng với mỗi yêu cầu hoàn tiền giao dịch. Mã này là duy nhất dùng để phân biệt các yêu cầu truy vấn giao dịch. Không được trùng lặp trong ngày.
            var vnp_Version = VnPayLibrary.VERSION; //2.1.0
            var vnp_Command = "refund";
            var vnp_TransactionType = order.TotalAmount == order.OrderAmount ? "02" : "03";
            var vnp_Amount = ((long)order.OrderAmount * 100).ToString();
            var vnp_TxnRef = order.TransactionNo; // Mã giao dịch thanh toán tham chiếu
            var vnp_OrderInfo = "Hoan tien giao dich:" + order.Id;
            var vnp_TransactionDate = order.TransactionDate;
            var vnp_CreateDate = DateTime.Now.ToString("yyyyMMddHHmmss");
            var vnp_CreateBy = user;
            var vnp_IpAddr = "127.0.0.1";

            var signData = vnp_RequestId + "|" + vnp_Version + "|" + vnp_Command + "|" + _appSettings.Value.Vnp_TmnCode + "|" + vnp_TransactionType + "|" + vnp_TxnRef + "|" + vnp_Amount + "|" + "|" + vnp_TransactionDate + "|" + vnp_CreateBy + "|" + vnp_CreateDate + "|" + vnp_IpAddr + "|" + vnp_OrderInfo;
            var vnp_SecureHash = Utils.HmacSHA512(_appSettings.Value.Vnp_SecureHash, signData);

            var rfData = new RefundData
            {
                vnp_RequestId = vnp_RequestId,
                vnp_Version = vnp_Version,
                vnp_Command = vnp_Command,
                vnp_TmnCode = _appSettings.Value.Vnp_TmnCode,
                vnp_TransactionType = vnp_TransactionType,
                vnp_TxnRef = vnp_TxnRef.ToString(),
                vnp_Amount = vnp_Amount,
                vnp_OrderInfo = vnp_OrderInfo,
                vnp_TransactionDate = vnp_TransactionDate,
                vnp_CreateBy = vnp_CreateBy.ToString(),
                vnp_CreateDate = vnp_CreateDate,
                vnp_IpAddr = vnp_IpAddr,
                vnp_SecureHash = vnp_SecureHash
            };

            var httpClient = _httpClientFactory.CreateClient();
            var apiUrl = "https://sandbox.vnpayment.vn/merchant_webapi/api/transaction";
            // Chuyển đối tượng thành chuỗi JSON
            var jsonData = JsonSerializer.Serialize(rfData);

            // Tạo nội dung của yêu cầu
            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

            // Thực hiện POST request
            var response = await httpClient.PostAsync(apiUrl, content);

            // Đọc nội dung phản hồi
            var responseContent = await response.Content.ReadAsStringAsync();
            var responseData = JsonSerializer.Deserialize<ResponseRefund>(responseContent);
            if (responseData.vnp_ResponseCode == "94")
            {
                var orderDetail = _dbContext.OrderDetails.FirstOrDefault(s => s.Id == model.OrderDetail && s.Status == OrderDetailStatuses.SUCCESS && !s.Deleted);
                orderDetail.RefundRequest = false;
                await _dbContext.SaveChangesAsync();
            }
            else if(responseData.vnp_ResponseCode == "00")
            {
                var orderDetail = _dbContext.OrderDetails.FirstOrDefault(s => s.Id == model.OrderDetail && s.Status == OrderDetailStatuses.SUCCESS && !s.Deleted);
                orderDetail.RefundRequest = false;
                BackgroundJob.Enqueue<ISystemService>
                    (x => x.ReceiveRefundNotification("Thông báo hoàn tiền thành công!", "Đơn đặt vé", orderTemp.CustomerMail, orderTemp.TicketType, orderTemp.TicketCode, orderTemp.SeatCode, orderTemp.EventName, orderTemp.OrganDay));
                await _dbContext.SaveChangesAsync();
            }
            else if (responseData.vnp_ResponseCode == "99")
            {
                throw new UserFriendlyException(ErrorCode.VNPayError);
            }
            else
            {
                throw new Exception(responseData.vnp_Message);

            }
        }

        public async Task CreateRefundTransferOrderUrl(RefundOrderDto model, HttpContext context)
        {
            var user = CommonUtils.GetCurrentUserId(_httpContext);
            var tick = DateTime.Now.Ticks.ToString();

            var orderTemp = (from ord in _dbContext.Orders
                             join cus in _dbContext.Customers on ord.CustomerId equals cus.Id
                             join use in _dbContext.Users on cus.Id equals use.CustomerId
                             join orderDetail in _dbContext.OrderDetails on ord.Id equals orderDetail.OrderId
                             join eventDt in _dbContext.EventDetails on orderDetail.EventDetailId equals eventDt.Id
                             join evt in _dbContext.Events on eventDt.EventId equals evt.Id
                             join ticket in _dbContext.Tickets on orderDetail.TicketId equals ticket.Id
                             join ticketType in _dbContext.TicketEvents on ticket.TicketEventId equals ticketType.Id
                             where ord.CustomerId == model.CustomerId && orderDetail.Id == model.OrderDetail
                             select new
                             {
                                 Id = ord.Id,
                                 OrderType = orderDetail.EventDetailId,
                                 TotalAmount = ord.Total,
                                 OrderAmount = orderDetail.Price,
                                 OrderDescription = "Hoàn tiền đơn chuyển nhượng vé thành công",
                                 TransactionNo = ord.TransactionNo,
                                 TransactionDate = ord.TransDate,
                                 CustomerMail = use.Email,
                                 EventName = evt.EventName,
                                 TicketType = ticketType.Name,
                                 TicketCode = ticket.TicketCode,
                                 SeatCode = ticket.SeatCode,
                                 OrganDay = eventDt.OrganizationDay
                             }).FirstOrDefault()
                             ?? throw new UserFriendlyException(ErrorCode.OrderNotFound);

            var order = new
            {
                Id = orderTemp.Id,
                OrderType = orderTemp.OrderType,
                TotalAmount = orderTemp.TotalAmount,
                OrderAmount = orderTemp.OrderAmount,
                OrderDescription = orderTemp.OrderDescription,
                TransactionNo = orderTemp.TransactionNo,
                TransactionDate = orderTemp.TransactionDate,
            };
            var vnp_RequestId = DateTime.Now.Ticks.ToString(); //Mã hệ thống merchant tự sinh ứng với mỗi yêu cầu hoàn tiền giao dịch. Mã này là duy nhất dùng để phân biệt các yêu cầu truy vấn giao dịch. Không được trùng lặp trong ngày.
            var vnp_Version = VnPayLibrary.VERSION; //2.1.0
            var vnp_Command = "refund";
            var vnp_TransactionType = order.TotalAmount == order.OrderAmount ? "02" : "03";
            var vnp_Amount = ((long)order.OrderAmount * 100).ToString();
            var vnp_TxnRef = order.TransactionNo; // Mã giao dịch thanh toán tham chiếu
            var vnp_OrderInfo = "Hoan tien giao dich:" + order.Id;
            var vnp_TransactionDate = order.TransactionDate;
            var vnp_CreateDate = DateTime.Now.ToString("yyyyMMddHHmmss");
            var vnp_CreateBy = user;
            var vnp_IpAddr = "127.0.0.1";

            var signData = vnp_RequestId + "|" + vnp_Version + "|" + vnp_Command + "|" + _appSettings.Value.Vnp_TmnCode + "|" + vnp_TransactionType + "|" + vnp_TxnRef + "|" + vnp_Amount + "|" + "|" + vnp_TransactionDate + "|" + vnp_CreateBy + "|" + vnp_CreateDate + "|" + vnp_IpAddr + "|" + vnp_OrderInfo;
            var vnp_SecureHash = Utils.HmacSHA512(_appSettings.Value.Vnp_SecureHash, signData);

            var rfData = new RefundData
            {
                vnp_RequestId = vnp_RequestId,
                vnp_Version = vnp_Version,
                vnp_Command = vnp_Command,
                vnp_TmnCode = _appSettings.Value.Vnp_TmnCode,
                vnp_TransactionType = vnp_TransactionType,
                vnp_TxnRef = vnp_TxnRef.ToString(),
                vnp_Amount = vnp_Amount,
                vnp_OrderInfo = vnp_OrderInfo,
                vnp_TransactionDate = vnp_TransactionDate,
                vnp_CreateBy = vnp_CreateBy.ToString(),
                vnp_CreateDate = vnp_CreateDate,
                vnp_IpAddr = vnp_IpAddr,
                vnp_SecureHash = vnp_SecureHash
            };

            var httpClient = _httpClientFactory.CreateClient();
            var apiUrl = "https://sandbox.vnpayment.vn/merchant_webapi/api/transaction";
            // Chuyển đối tượng thành chuỗi JSON
            var jsonData = JsonSerializer.Serialize(rfData);

            // Tạo nội dung của yêu cầu
            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

            // Thực hiện POST request
            var response = await httpClient.PostAsync(apiUrl, content);

            // Đọc nội dung phản hồi
            var responseContent = await response.Content.ReadAsStringAsync();
            var responseData = JsonSerializer.Deserialize<ResponseRefund>(responseContent);
            if (responseData.vnp_ResponseCode == "94")
            {
                var orderDetail = _dbContext.OrderDetails.FirstOrDefault(s => s.Id == model.OrderDetail && s.Status == OrderDetailStatuses.SUCCESS && !s.Deleted);
                orderDetail.TransferRefundRequest = false;
                orderDetail.TransferStatus = TransferStatuses.REFUNDED;
                await _dbContext.SaveChangesAsync();
            }
            else if (responseData.vnp_ResponseCode == "00")
            {
                var orderDetail = _dbContext.OrderDetails.FirstOrDefault(s => s.Id == model.OrderDetail && s.Status == OrderDetailStatuses.SUCCESS && !s.Deleted);
                orderDetail.TransferRefundRequest = false;
                orderDetail.TransferStatus = TransferStatuses.REFUNDED;
                BackgroundJob.Enqueue<ISystemService>
                    (x => x.ReceiveRefundNotification("Thông báo hoàn tiền chuyển nhượng vé thành công!", "Đơn chuyển nhượng vé", orderTemp.CustomerMail, orderTemp.TicketType, orderTemp.TicketCode, orderTemp.SeatCode, orderTemp.EventName, orderTemp.OrganDay));
                await _dbContext.SaveChangesAsync();
            }
            else if (responseData.vnp_ResponseCode == "99")
            {
                throw new UserFriendlyException(ErrorCode.VNPayError);
            }
            else
            {
                throw new Exception(responseData.vnp_Message);

            }
        }

        public async Task CreateRefundExchangeDoneUrl(RefundOrderDto model, HttpContext context)
        {
            var user = CommonUtils.GetCurrentUserId(_httpContext);
            var tick = DateTime.Now.Ticks.ToString();

            var orderTemp = (from ord in _dbContext.Orders
                             join cus in _dbContext.Customers on ord.CustomerId equals cus.Id
                             join use in _dbContext.Users on cus.Id equals use.CustomerId
                             join orderDetail in _dbContext.OrderDetails on ord.Id equals orderDetail.OrderId
                             join eventDt in _dbContext.EventDetails on orderDetail.EventDetailId equals eventDt.Id
                             join evt in _dbContext.Events on eventDt.EventId equals evt.Id
                             join ticket in _dbContext.Tickets on orderDetail.TicketId equals ticket.Id
                             join ticketType in _dbContext.TicketEvents on ticket.TicketEventId equals ticketType.Id
                             where ord.CustomerId == model.CustomerId && orderDetail.Id == model.OrderDetail
                             select new
                             {
                                 Id = ord.Id,
                                 OrderType = orderDetail.EventDetailId,
                                 TotalAmount = ord.Total,
                                 OrderAmount = orderDetail.Price,
                                 OrderDescription = "Hoàn tiền đơn Trả vé My Ticket",
                                 TransactionNo = ord.TransactionNo,
                                 TransactionDate = ord.TransDate,
                                 CustomerMail = use.Email,
                                 EventName = evt.EventName,
                                 TicketType = ticketType.Name,
                                 TicketCode = ticket.TicketCode,
                                 SeatCode = ticket.SeatCode,
                                 OrganDay = eventDt.OrganizationDay
                             }).FirstOrDefault()
                             ?? throw new UserFriendlyException(ErrorCode.OrderNotFound);

            var order = new
            {
                Id = orderTemp.Id,
                OrderType = orderTemp.OrderType,
                TotalAmount = orderTemp.TotalAmount,
                OrderAmount = orderTemp.OrderAmount,
                OrderDescription = orderTemp.OrderDescription,
                TransactionNo = orderTemp.TransactionNo,
                TransactionDate = orderTemp.TransactionDate,
            };
            var vnp_RequestId = DateTime.Now.Ticks.ToString(); //Mã hệ thống merchant tự sinh ứng với mỗi yêu cầu hoàn tiền giao dịch. Mã này là duy nhất dùng để phân biệt các yêu cầu truy vấn giao dịch. Không được trùng lặp trong ngày.
            var vnp_Version = VnPayLibrary.VERSION; //2.1.0
            var vnp_Command = "refund";
            var vnp_TransactionType = order.TotalAmount == order.OrderAmount ? "02" : "03";
            var vnp_Amount = ((long)order.OrderAmount * 100).ToString();
            var vnp_TxnRef = order.TransactionNo; // Mã giao dịch thanh toán tham chiếu
            var vnp_OrderInfo = "Hoan tien giao dich:" + order.Id;
            var vnp_TransactionDate = order.TransactionDate;
            var vnp_CreateDate = DateTime.Now.ToString("yyyyMMddHHmmss");
            var vnp_CreateBy = user;
            var vnp_IpAddr = "127.0.0.1";

            var signData = vnp_RequestId + "|" + vnp_Version + "|" + vnp_Command + "|" + _appSettings.Value.Vnp_TmnCode + "|" + vnp_TransactionType + "|" + vnp_TxnRef + "|" + vnp_Amount + "|" + "|" + vnp_TransactionDate + "|" + vnp_CreateBy + "|" + vnp_CreateDate + "|" + vnp_IpAddr + "|" + vnp_OrderInfo;
            var vnp_SecureHash = Utils.HmacSHA512(_appSettings.Value.Vnp_SecureHash, signData);

            var rfData = new RefundData
            {
                vnp_RequestId = vnp_RequestId,
                vnp_Version = vnp_Version,
                vnp_Command = vnp_Command,
                vnp_TmnCode = _appSettings.Value.Vnp_TmnCode,
                vnp_TransactionType = vnp_TransactionType,
                vnp_TxnRef = vnp_TxnRef.ToString(),
                vnp_Amount = vnp_Amount,
                vnp_OrderInfo = vnp_OrderInfo,
                vnp_TransactionDate = vnp_TransactionDate,
                vnp_CreateBy = vnp_CreateBy.ToString(),
                vnp_CreateDate = vnp_CreateDate,
                vnp_IpAddr = vnp_IpAddr,
                vnp_SecureHash = vnp_SecureHash
            };

            var httpClient = _httpClientFactory.CreateClient();
            var apiUrl = "https://sandbox.vnpayment.vn/merchant_webapi/api/transaction";
            // Chuyển đối tượng thành chuỗi JSON
            var jsonData = JsonSerializer.Serialize(rfData);

            // Tạo nội dung của yêu cầu
            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

            // Thực hiện POST request
            var response = await httpClient.PostAsync(apiUrl, content);

            // Đọc nội dung phản hồi
            var responseContent = await response.Content.ReadAsStringAsync();
            var responseData = JsonSerializer.Deserialize<ResponseRefund>(responseContent);
            if (responseData.vnp_ResponseCode == "94")
            {
                var orderDetail = _dbContext.OrderDetails.FirstOrDefault(s => s.Id == model.OrderDetail && s.Status == OrderDetailStatuses.SUCCESS && !s.Deleted);
                orderDetail.ExchangeRefundRequest = false;
                orderDetail.ExchangeStatus = ExchangeStatuses.SUCCESS;
                await _dbContext.SaveChangesAsync();
            }
            else if (responseData.vnp_ResponseCode == "00")
            {
                var orderDetail = _dbContext.OrderDetails.FirstOrDefault(s => s.Id == model.OrderDetail && s.Status == OrderDetailStatuses.SUCCESS && !s.Deleted);
                orderDetail.ExchangeRefundRequest = false;
                orderDetail.ExchangeStatus = ExchangeStatuses.SUCCESS;
                BackgroundJob.Enqueue<ISystemService>
                    (x => x.ReceiveRefundNotification("Thông báo hoàn tiền trả vé thành công!","Đơn trả vé",orderTemp.CustomerMail,orderTemp.TicketType,orderTemp.TicketCode, orderTemp.SeatCode,orderTemp.EventName,orderTemp.OrganDay));
                await _dbContext.SaveChangesAsync();
            }
            else if (responseData.vnp_ResponseCode == "99")
            {
                throw new UserFriendlyException(ErrorCode.VNPayError);
            }
            else
            {
                throw new Exception(responseData.vnp_Message);

            }
        }

        public async Task CreateRefundTransferUrl(RefundOrderDto model, HttpContext context)
        {
            var user = CommonUtils.GetCurrentUserId(_httpContext);
            var tick = DateTime.Now.Ticks.ToString();

            var orderTemp = (from ord in _dbContext.Orders
                             join orderDetail in _dbContext.OrderDetails on ord.Id equals orderDetail.OrderId
                             join cus in _dbContext.Customers on orderDetail.CustomerTransfer equals cus.Id
                             join use in _dbContext.Users on cus.Id equals use.CustomerId
                             join eventDt in _dbContext.EventDetails on orderDetail.EventDetailId equals eventDt.Id
                             join evt in _dbContext.Events on eventDt.EventId equals evt.Id
                             join ticket in _dbContext.Tickets on orderDetail.TicketId equals ticket.Id
                             join ticketType in _dbContext.TicketEvents on ticket.TicketEventId equals ticketType.Id
                             where orderDetail.CustomerTransfer == model.CustomerId && orderDetail.Id == model.OrderDetail
                             select new
                             {
                                 Id = ord.Id,
                                 OrderType = orderDetail.EventDetailId,
                                 TotalAmount = ord.Total,
                                 OrderAmount = orderDetail.Price,
                                 OrderDescription = "Hoàn tiền đơn đặt vé My Ticket",
                                 TransactionNo = orderDetail.TransferTransactionNo,
                                 TransactionDate = orderDetail.TransferTransDate,
                                 CustomerMail = use.Email,
                                 EventName = evt.EventName,
                                 TicketType = ticketType.Name,
                                 TicketCode = ticket.TicketCode,
                                 SeatCode = ticket.SeatCode,
                                 OrganDay = eventDt.OrganizationDay
                             }).FirstOrDefault()
                             ?? throw new UserFriendlyException(ErrorCode.OrderNotFound);

            var order = new
            {
                Id = orderTemp.Id,
                OrderType = orderTemp.OrderType,
                TotalAmount = orderTemp.TotalAmount,
                OrderAmount = orderTemp.OrderAmount,
                OrderDescription = orderTemp.OrderDescription,
                TransactionNo = orderTemp.TransactionNo,
                TransactionDate = orderTemp.TransactionDate,
            };
            var vnp_RequestId = DateTime.Now.Ticks.ToString(); //Mã hệ thống merchant tự sinh ứng với mỗi yêu cầu hoàn tiền giao dịch. Mã này là duy nhất dùng để phân biệt các yêu cầu truy vấn giao dịch. Không được trùng lặp trong ngày.
            var vnp_Version = VnPayLibrary.VERSION; //2.1.0
            var vnp_Command = "refund";
            var vnp_TransactionType = "02";
            var vnp_Amount = ((long)order.OrderAmount * 100).ToString();
            var vnp_TxnRef = order.TransactionNo; // Mã giao dịch thanh toán tham chiếu
            var vnp_OrderInfo = "Hoan tien giao dich:" + order.Id;
            var vnp_TransactionDate = order.TransactionDate;
            var vnp_CreateDate = DateTime.Now.ToString("yyyyMMddHHmmss");
            var vnp_CreateBy = user;
            var vnp_IpAddr = "127.0.0.1";

            var signData = vnp_RequestId + "|" + vnp_Version + "|" + vnp_Command + "|" + _appSettings.Value.Vnp_TmnCode + "|" + vnp_TransactionType + "|" + vnp_TxnRef + "|" + vnp_Amount + "|" + "|" + vnp_TransactionDate + "|" + vnp_CreateBy + "|" + vnp_CreateDate + "|" + vnp_IpAddr + "|" + vnp_OrderInfo;
            var vnp_SecureHash = Utils.HmacSHA512(_appSettings.Value.Vnp_SecureHash, signData);

            var rfData = new RefundData
            {
                vnp_RequestId = vnp_RequestId,
                vnp_Version = vnp_Version,
                vnp_Command = vnp_Command,
                vnp_TmnCode = _appSettings.Value.Vnp_TmnCode,
                vnp_TransactionType = vnp_TransactionType,
                vnp_TxnRef = vnp_TxnRef.ToString(),
                vnp_Amount = vnp_Amount,
                vnp_OrderInfo = vnp_OrderInfo,
                vnp_TransactionDate = vnp_TransactionDate,
                vnp_CreateBy = vnp_CreateBy.ToString(),
                vnp_CreateDate = vnp_CreateDate,
                vnp_IpAddr = vnp_IpAddr,
                vnp_SecureHash = vnp_SecureHash
            };

            var httpClient = _httpClientFactory.CreateClient();
            var apiUrl = "https://sandbox.vnpayment.vn/merchant_webapi/api/transaction";
            // Chuyển đối tượng thành chuỗi JSON
            var jsonData = JsonSerializer.Serialize(rfData);

            // Tạo nội dung của yêu cầu
            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

            // Thực hiện POST request
            var response = await httpClient.PostAsync(apiUrl, content);

            // Đọc nội dung phản hồi
            var responseContent = await response.Content.ReadAsStringAsync();
            var responseData = JsonSerializer.Deserialize<ResponseRefund>(responseContent);
            if (responseData.vnp_ResponseCode == "94")
            {
                var orderDetail = _dbContext.OrderDetails.FirstOrDefault(s => s.Id == model.OrderDetail && s.Status == OrderDetailStatuses.SUCCESS && !s.Deleted);
                orderDetail.RefundRequest = false;
                await _dbContext.SaveChangesAsync();
            }
            else if (responseData.vnp_ResponseCode == "00")
            {
                var orderDetail = _dbContext.OrderDetails.FirstOrDefault(s => s.Id == model.OrderDetail && s.Status == OrderDetailStatuses.SUCCESS && !s.Deleted);
                orderDetail.RefundRequest = false;
                BackgroundJob.Enqueue<ISystemService>
                    (x => x.ReceiveRefundNotification("Thông báo hoàn tiền đơn đặt vé thành công do hủy sự kiện!", "Đơn đặt vé", orderTemp.CustomerMail, orderTemp.TicketType, orderTemp.TicketCode, orderTemp.SeatCode, orderTemp.EventName, orderTemp.OrganDay));
                await _dbContext.SaveChangesAsync();
            }
            else if (responseData.vnp_ResponseCode == "99")
            {
                throw new UserFriendlyException(ErrorCode.VNPayError);
            }
            else
            {
                throw new Exception(responseData.vnp_Message);

            }
        }
    }
}
