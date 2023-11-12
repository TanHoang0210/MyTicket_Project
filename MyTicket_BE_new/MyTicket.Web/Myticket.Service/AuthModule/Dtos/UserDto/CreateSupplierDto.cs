using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MYTICKET.WEB.SERVICE.AuthModule.Dtos.UserDto
{
    public class CreateSupplierDto
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
        private string _fullName = null!;
        public string FullName
        {
            get => _fullName;
            set => _fullName = value.Trim();
        }
        /// <summary>
        /// Họ
        /// </summary>
        private string _shortName = null!;
        public string ShortName
        {
            get => _shortName;
            set => _shortName = value.Trim();
        }
        private string _address = null!;
        public string Address 
        {
            get => _address;
            set => _address = value.Trim();
        }
        private string _taxCode = null!;
        public string TaxCode
        {
            get => _taxCode;
            set => _taxCode = value.Trim();
        }
    }
}
