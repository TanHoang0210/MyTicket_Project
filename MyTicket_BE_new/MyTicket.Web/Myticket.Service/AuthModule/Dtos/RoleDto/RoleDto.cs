using MYTICKET.UTILS.ConstantVariables.User;
using MYTICKET.WEB.DOMAIN.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MYTICKET.WEB.SERVICE.AuthModule.Dtos.RoleDto
{
    public class RoleDto
    {
        /// <summary>
        /// Id
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Tên Role
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// <see cref="UserTypes"/>
        /// </summary>
        public int UserType { get; set; }
        /// <summary>
        /// Mô tả
        /// </summary>
        public string Description { get; set; }
        public List<RolePermission> PermissionDetails { get; set; }
    }
}
