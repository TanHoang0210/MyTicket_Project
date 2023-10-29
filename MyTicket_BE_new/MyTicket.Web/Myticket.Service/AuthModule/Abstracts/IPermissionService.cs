using MYTICKET.WEB.SERVICE.AuthModule.Dtos.PermissionDto;

namespace MYTICKET.WEB.SERVICE.AuthModule.Abstracts
{
    public interface IPermissionService
    {
        List<string> GetPermission();
        List<PermissionDetailDto> FindAllPermission();
    }
}
