using Microsoft.AspNetCore.Mvc;
using MYTICKET.BASE.SERVICE.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MYTICKET.WEB.SERVICE.AuthModule.Dtos.UserDto
{
    public class FilterUserDto : PagingRequestBaseDto
    {
        [FromQuery(Name = "username")]
        public string Username { get; set; }
        [FromQuery(Name = "fullname")]
        public string FullName { get; set; }
        [FromQuery(Name = "status")]
        public int? Status { get; set; }
        [FromQuery(Name = "customerId")]
        public int? CustomerId { get; set; }
        [FromQuery(Name = "supplierId")]
        public int? SupplierId { get; set; }
    }
}
