using MYTICKET.BASE.SERVICE.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MYTICKET.WEB.SERVICE.VenueModule.Dtos
{
    public class FilterVenueDto : PagingRequestBaseDto
    {
        /// <summary>
        /// tên svd
        /// </summary>
        public string? Name { get; set; }
    }
}
