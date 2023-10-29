using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MYTICKET.WEB.SERVICE.AuthModule.Dtos.PermissionDto
{
    public class PermissionDetailDto
    {
        /// <summary>
        /// PermissionKey
        /// </summary>
        public string Key { get; set; }
        public string ParentKey { get; set; }
        public string Label { get; set; }
        public string Icon { get; set; }
    }
}
