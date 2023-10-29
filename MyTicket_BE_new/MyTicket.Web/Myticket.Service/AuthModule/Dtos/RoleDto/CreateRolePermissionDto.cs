using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MYTICKET.WEB.SERVICE.AuthModule.Dtos.RoleDto
{
    public class CreateRolePermissionDto
    {
        private string _name;
        public string Name
        {
            get => _name;
            set => _name = value?.Trim();
        }

        private string _description;
        public string Description
        {
            get => _description;
            set => _description = value?.Trim();
        }
        public int UserType { get; set; }
        public List<string> PermissionKeys { get; set; }
    }
}
