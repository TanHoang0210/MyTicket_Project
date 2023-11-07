using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MYTICKET.WEB.SERVICE.VenueModule.Dtos
{
    /// <summary>
    /// chỉnh sửa svd
    /// </summary>
    public class UpdateVenueDto :CreateVenueDto
    {
        /// <summary>
        /// id svd
        /// </summary>
        public int Id { get; set; }
    }
}
