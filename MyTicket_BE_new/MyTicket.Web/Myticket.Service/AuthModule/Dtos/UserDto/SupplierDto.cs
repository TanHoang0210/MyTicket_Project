using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MYTICKET.WEB.SERVICE.AuthModule.Dtos.UserDto
{
    public class SupplierDto
    {
        public int Id { get; set; }

        /// <summary>
        /// Tên đầy đủ
        /// </summary>
        public string FullName { get; set; } = null!;
        /// <summary>
        /// Tên viết tắt
        /// </summary>
        public string? ShortName { get; set; }

        public string Address { get; set; } = null!;
        public string TaxCode { get; set; } = null!;
    }
}
