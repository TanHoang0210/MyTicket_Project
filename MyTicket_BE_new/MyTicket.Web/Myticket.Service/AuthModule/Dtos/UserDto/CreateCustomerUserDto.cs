using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MYTICKET.WEB.SERVICE.AuthModule.Dtos.UserDto
{
    public class CreateCustomerUserDto
    {
        /// <summary>
        /// Tên đăng nhập
        /// </summary>
        private string _username = null!;
        public string Username
        {
            get => _username;
            set => _username = value.Trim();
        }

        /// <summary>
        /// Mật khẩu
        /// </summary>
        private string _password = null!;
        public string Password
        {
            get => _password;
            set => _password = value.Trim();
        }

        /// <summary>
        /// Email
        /// </summary>
        private string _email = null!;
        public string Email
        {
            get => _email;
            set => _email = value.Trim();
        }

        /// <summary>
        /// Số điện thoại
        /// </summary>
        private string _phone = null!;
        public string Phone
        {
            get => _phone;
            set => _phone = value.Trim();
        }


        /// <summary>
        /// Tên
        /// </summary>
        private string _lastName = null!;
        public string LastName
        {
            get => _lastName;
            set => _lastName = value.Trim();
        }
        /// <summary>
        /// Họ
        /// </summary>
        private string _firstName = null!;
        public string FirstName
        {
            get => _firstName;
            set => _firstName = value.Trim();
        }
    }
}
