using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MYTICKET.WEB.SERVICE.AuthModule.Dtos.UserDto
{
    public class SetPasswordUserDto
    {
        public int Id { get; set; }
        private string? _password;
        public string? Password 
        { 
            get => _password;
            set => _password = value?.Trim(); 
        }
    }
}
