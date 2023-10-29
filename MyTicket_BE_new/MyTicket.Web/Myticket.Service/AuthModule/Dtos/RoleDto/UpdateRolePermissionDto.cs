using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MYTICKET.WEB.SERVICE.AuthModule.Dtos.RoleDto
{
    public class UpdateRolePermissionDto : CreateRolePermissionDto
    {
        public int Id { get; set; }
    }
}
