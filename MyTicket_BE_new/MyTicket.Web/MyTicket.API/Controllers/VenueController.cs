using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MYTICKET.BASE.SERVICE.Common;
using MYTICKET.UTILS;
using MYTICKET.WEB.SERVICE.VenueModule.Abstracts;
using MYTICKET.WEB.SERVICE.VenueModule.Dtos;

namespace MYTICKET.WEB.API.Controllers
{
    [Route("myticket/api/venue")]
    [ApiController]
    public class VenueController : ControllerBase
    {
        private readonly IVenueService _venueService;

        public VenueController(IVenueService venueService) 
        {
            _venueService = venueService;
        }
        /// <summary>
        /// thêm mới svd
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("create")]
        public APIResponse<VenueDto> CreateVenue(CreateVenueDto input)
               => new(_venueService.Create(input));

        /// <summary>
        /// cập nhập svd
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut("update")]
        public APIResponse<VenueDto> UpdateVenue(UpdateVenueDto input)
               => new(_venueService.Update(input));

        /// <summary>
        /// lấy thông tin svd theo id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("find-by-id")]
        public APIResponse<VenueDetailDto> FindVenueById([FromQuery]int id)
               => new(_venueService.GetById(id));

        /// <summary>
        /// lây danh sách svd
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet("find")]
        public APIResponse<PagingResult<VenueDto>> FindProduct([FromQuery] FilterVenueDto input)
               => new(_venueService.GetAll(input));

        /// <summary>
        /// Xóa venue
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("delete/{id}")]
        public APIResponse DeleteVenue(int id)
        {
            _venueService.Delete(id);
            return new();
        }
    }
}
