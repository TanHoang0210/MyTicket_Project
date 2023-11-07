using MYTICKET.BASE.SERVICE.Common;
using MYTICKET.WEB.SERVICE.VenueModule.Dtos;

namespace MYTICKET.WEB.SERVICE.VenueModule.Abstracts
{
    public interface IVenueService
    {
        /// <summary>
        /// Thêm mới svd
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        VenueDto Create(CreateVenueDto input);

        /// <summary>
        /// cập nhật thông tin sân vận động
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        VenueDto Update(UpdateVenueDto input);

        /// <summary>
        /// Danh sách sân vận động
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        PagingResult<VenueDto> GetAll(FilterVenueDto input);

        /// <summary>
        /// Thông tin chi tiết sân vận động
        /// </summary>
        /// <param name="venueId"></param>
        /// <returns></returns>
        VenueDetailDto GetById(int venueId);

        /// <summary>
        /// xóa sân vận động
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        void Delete(int id);
    }
}
