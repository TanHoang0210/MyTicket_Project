using MYTICKET.BASE.ENTITIES.Base;
using MYTICKET.BASE.ENTITIES.Interfaces;

namespace MYTICKET.BASE.InfrastructureBase.Entities
{
    public interface IRole : IEntity<int>, IFullAudited
    {
        string Name { get; set; }
        string Description { get; set; }
    }
}
