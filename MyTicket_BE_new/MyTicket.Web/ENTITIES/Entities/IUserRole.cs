

using MYTICKET.BASE.ENTITIES.Base;
using MYTICKET.BASE.ENTITIES.Interfaces;

namespace MYTICKET.BASE.ENTITIES.Entities
{
    public interface IUserRole<TUserId, TRoleId> : IEntity<int>, IFullAudited
    {
        TUserId UserId { get; set; }
        TRoleId RoleId { get; set; }
    }
}
