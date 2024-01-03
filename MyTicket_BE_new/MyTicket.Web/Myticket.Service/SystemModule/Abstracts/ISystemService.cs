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

        Task NotiSuccessTransfer(string buyTicketUserEmail, int ownerUserId);
    }
}
