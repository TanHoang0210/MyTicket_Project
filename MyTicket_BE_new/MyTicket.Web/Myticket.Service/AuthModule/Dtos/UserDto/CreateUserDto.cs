using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MYTICKET.WEB.SERVICE.AuthModule.Dtos.UserDto
{
    public class CreateUserDto
    {
        /// <summary>
        /// Tên đăng nhập
        /// </summary>
        private string _username;
        public string Username
        {
            get => _username;
            set => _username = value?.Trim();
        }

        /// <summary>
        /// Mật khẩu
        /// </summary>
        private string _password;
        public string Password
        {
            get => _password;
            set => _password = value?.Trim();
        }

        /// <summary>
        /// Email
        /// </summary>
        private string _email;
        public string Email
        {
            get => _email;
            set => _email = value?.Trim();
        }

        /// <summary>
        /// Số điện thoại
        /// </summary>
        private string _phone;
        public string Phone
        {
            get => _phone;
            set => _phone = value?.Trim();
        }


        /// <summary>
        /// Tên người dùng
        /// </summary>
        private string _fullName;
        public string FullName
        {
            get => _fullName;
            set => _fullName = value?.Trim();
        }


        /// <summary>
        /// Loại tài khoản
        /// </summary>
        public int? UserType { get; set; }
        public int? Status { get; set; }
        public int? CustomerId { get; set; }
        public int? SupplierId { get; set; }
        public List<int> RoleIds { get; set; }
    }
}
