using Microsoft.AspNetCore.Mvc;
using MYTICKET.BASE.SERVICE.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MYTICKET.WEB.SERVICE.AuthModule.Dtos.RoleDto
{
    public class FilterRoleDto : PagingRequestBaseDto
    {
        [FromQuery(Name = "userType")]
        public int? UserType { get; set; }
    }
}
