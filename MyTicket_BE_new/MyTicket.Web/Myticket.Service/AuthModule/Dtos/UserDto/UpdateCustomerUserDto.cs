using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MYTICKET.WEB.SERVICE.AuthModule.Dtos.UserDto
{
    public class UpdateCustomerUserDto
    {
        /// <summary>
        /// Tên
        /// </summary>
        private string? _lastName;
        public string? LastName
        {
            get => _lastName;
            set => _lastName = value?.Trim();
        }
        /// <summary>
        /// Họ
        /// </summary>
        private string? _firstName;
        public string? FirstName
        {
            get => _firstName;
            set => _firstName = value?.Trim();
        }

        /// <summary>
        /// Họ
        /// </summary>
        private string? _address;
        public string? Address
        {
            get => _address;
            set => _address = value?.Trim();
        }
        public DateTime? DateOfBirth { get; set; }
        /// <summary>
        /// Họ
        /// </summary>
        private string? _country;
        public string? Country
        {
            get => _country;
            set => _country = value?.Trim();
        }
        /// <summary>
        /// Họ
        /// </summary>
        private string? _nationality;
        public string? Nationality
        {
            get => _nationality;
            set => _nationality = value?.Trim();
        }
        public int? Gender { get; set; }
    }
}
