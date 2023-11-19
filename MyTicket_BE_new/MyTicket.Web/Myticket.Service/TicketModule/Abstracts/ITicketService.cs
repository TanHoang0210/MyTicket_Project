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
    }
}
