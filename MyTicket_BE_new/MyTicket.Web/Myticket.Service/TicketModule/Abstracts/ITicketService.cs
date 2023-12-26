using MYTICKET.BASE.SERVICE.Common;
using MYTICKET.WEB.SERVICE.TicketModule.Dtos;

namespace MYTICKET.WEB.SERVICE.TicketModule.Abstracts
{
    public interface ITicketService
    {
        /// <summary>
        /// Lấy danh sách vé 
        /// </summary>
        /// <param name="eventDetailId"></param>
        /// <returns></returns>
        List<TicketEventDto> GetAllTicket(int eventDetailId);

        /// <summary>
        /// Thêm mới vé sự kiện
        /// </summary>
        /// <param name="input"></param>
        void CreateTicket(CreateTicketDto input);

        /// <summary>
        /// Thêm mới loai ve su kien
        /// </summary>
        /// <param name="input"></param>
        void CreateTicketEvent(CreateTicketEventDto input);

        /// <summary>
        /// Lấy danh sách vé 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        PagingResult<TicketEventTransferDto> GetAllTicketTransfer(FilterTicketDto input);

        /// <summary>
        /// Lấy thong tin ve
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        TicketEventDto GetTicketById(int id);
    }
}
