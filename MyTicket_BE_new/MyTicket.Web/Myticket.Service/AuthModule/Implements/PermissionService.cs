using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using MYTICKET.BASE.SERVICE.Common;
using MYTICKET.UTILS.RolePermission;
using MYTICKET.WEB.SERVICE.AuthModule.Abstracts;
using MYTICKET.WEB.SERVICE.AuthModule.Dtos.PermissionDto;
using MYTICKET.WEB.SERVICE.AuthModule.Dtos.RoleDto;
using MYTICKET.WEB.SERVICE.Common;

namespace MYTICKET.WEB.SERVICE.AuthModule.Implements
{
    public class PermissionService : ServiceBase, IPermissionService
    {
        public PermissionService(ILogger<PermissionService> logger, IHttpContextAccessor httpContext) : base(logger, httpContext)
        {
        }

        public List<string> GetPermission()
        {
            var result = new List<string>();
            var userId = CommonUtils.GetCurrentUserId(_httpContext);
            var userType = CommonUtils.GetCurrentUserType(_httpContext);
            var userRoles = _dbContext.UserRoles.Where(e => !e.Deleted);

            var query = from userRole in userRoles
                        join role in _dbContext.Roles.Where(e => !e.Deleted) on userRole.RoleId equals role.Id
                        join rolePermission in _dbContext.RolePermissions on role.Id equals rolePermission.RoleId
                        where role.UserType == userType && userRole.UserId == userId
                        select rolePermission.PermissionKey;

            result = query.Distinct().ToList();
            return result;
        }

        public List<PermissionDetailDto> FindAllPermission()
        {
            var result = new List<PermissionDetailDto>();
            foreach (var item in PermissionConfig.Configs)
            {
                result.Add(new PermissionDetailDto
                {
                    Key = item.Key,
                    ParentKey = item.Value.ParentKey,
                    Label = L(item.Value.LName),
                    Icon = item.Value.Icon
                });
            }
            return result;
        }

        public List<PermissionDetailDto> CreateRolePermission(CreateRolePermissionDto input)
        {
            throw new NotImplementedException();
        }
    }
}
