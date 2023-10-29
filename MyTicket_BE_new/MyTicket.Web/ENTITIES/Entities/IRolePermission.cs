using MYTICKET.BASE.ENTITIES.Base;
using MYTICKET.BASE.ENTITIES.Interfaces;

namespace MYTICKET.BASE.ENTITIES.Entities
{
    public interface IRolePermission<TRoleId> : IEntity<int>, ICreatedBy
    {
        TRoleId RoleId { get; set; }
        string PermissionKey { get; set; }
    }
}
