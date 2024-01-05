namespace MYTICKET.WEB.SERVICE.SystemModule.Abstracts
{
    public interface ISystemService
    {
        /// <summary>
        /// Thông báo sinh nhật khách hàng
        /// </summary>
        /// <returns></returns>
        Task HappyBirthDayCustomerNotification();

        /// <summary>
        /// Thông báo hủy sự kiện
        /// </summary>
        /// <returns></returns>
        Task CancelEventNotification(int eventDetailId);

        /// <summary>
        /// Thông báo chưa hoàn tiền cho khách hàng
        /// </summary>
        /// <returns></returns>
        Task NotRefundNotificationForAdmin();

        /// <summary>
        /// Thông báo chưa hoàn tiền cho khách hàng
        /// </summary>
        /// <returns></returns>
        Task NotRefundTransferNotificationForAdmin();

        /// <summary>
        /// Thông báo chưa hoàn tiền cho khách hàng
        /// </summary>
        /// <returns></returns>
        Task NotRefundExchangeNotificationForAdmin();

        Task CancelOrderExpired(int orderId);
        /// <summary>
        /// Thoong baso chuyen nhuong thanh cong
        /// </summary>
        /// <param name="buyTicketUserEmail"></param>
        /// <param name="ownerUserId"></param>
        /// <returns></returns>
        Task NotiSuccessTransfer(string buyTicketUserEmail, int ownerUserId);

        /// <summary>
        /// Thông báo nhận vé chuyển nhuonwjg
        /// </summary>
        /// <param name="title"></param>
        /// <param name="type"></param>
        /// <param name="CustomerMail"></param>
        /// <param name="TicketType"></param>
        /// <param name="TicketCode"></param>
        /// <param name="SeatCode"></param>
        /// <param name="EventName"></param>
        /// <param name="OrganDay"></param>
        /// <returns></returns>
        Task ReceiveRefundNotification(string title, string type, string CustomerMail, string TicketType, string TicketCode, string SeatCode, string EventName, DateTime OrganDay);

        /// <summary>
        /// tự động hủy đơn chuyển nhượng vé
        /// </summary>
        /// <returns></returns>
        Task CancelAllTransfer();

        /// <summary>
        /// Tự động hủy đơn trả vé
        /// </summary>
        /// <returns></returns>
        Task CancelAllExchange();


    }
}
