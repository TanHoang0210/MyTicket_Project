using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MYTICKET.WEB.SERVICE.AuthModule.Dtos.UserDto
{
    /// <summary>
    /// Tài khoản thực hiện việc nào đó
    /// </summary>
    public class UserByDto
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string FullName { get; set; }
        public int UserType { get; set; }
    }
}
