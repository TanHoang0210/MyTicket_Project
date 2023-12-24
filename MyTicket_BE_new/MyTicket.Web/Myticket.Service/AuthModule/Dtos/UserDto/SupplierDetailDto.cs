using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MYTICKET.WEB.SERVICE.AuthModule.Dtos.UserDto
{
    public class SupplierDetailDto
    {
        public int Id { get; set; }

        /// <summary>
        /// Tên đầy đủ
        /// </summary>
        public string? FullName { get; set; }
        /// <summary>
        /// Tên viết tắt
        /// </summary>
        public string? ShortName { get; set; }

        public string? Address { get; set; }
        public string? TaxCode { get; set; } 

        public List<SupplierAccountDto>? Accounts { get; set; }

    }
}
