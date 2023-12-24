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
