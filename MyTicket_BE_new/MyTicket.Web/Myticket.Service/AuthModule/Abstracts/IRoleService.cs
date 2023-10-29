using MYTICKET.BASE.SERVICE.Common;
using MYTICKET.WEB.SERVICE.AuthModule.Dtos.RoleDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MYTICKET.WEB.SERVICE.AuthModule.Abstracts
{
    public interface IRoleService
    {
        RoleDto Add(CreateRolePermissionDto input);
        RoleDto Update(UpdateRolePermissionDto input);
        RoleDto FindById(int id);
        void Delete(int id);
        PagingResult<RoleDto> FindAll(FilterRoleDto input);
    }
}
