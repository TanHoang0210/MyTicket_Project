using Microsoft.AspNetCore.Http;
using MYTICKET.BASE.SERVICE.Common.Validations;

namespace MYTICKET.WEB.SERVICE.VenueModule.Dtos
{
    /// <summary>
    /// tạo mới svd
    /// </summary>
    public class CreateVenueDto
    {
        /// <summary>
        /// tên svd
        /// </summary>
        public required string Name { get; set; }
        /// <summary>
        /// địa chỉ svd
        /// </summary>
        public string Address { get; set; }
        /// <summary>
        /// sức chứa svd
        /// </summary>
        public int Capacity { get; set; }
        /// <summary>
        /// Mô tả
        /// </summary>
        public string? Description { get; set; }
        /// <summary>
        /// ảnh svd
        /// </summary>
        [CustomMaxLength(1024)]
        public string? Image { get; set; }
    }
}
