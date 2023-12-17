using MYTICKET.BASE.SERVICE.Common;
using MYTICKET.WEB.SERVICE.AuthModule.Dtos.UserDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MYTICKET.WEB.SERVICE.AuthModule.Abstracts
{
    public interface ISupplierService : IUserService
    {
        /// <summary>
        /// Thêm user ncc
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        void CreateSupplierUser(CreateSupplierDto input);

        /// <summary>
        /// Danh sasch ncc
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        PagingResult<SupplierDto> GetAll(FilerSupplierDto input);
    }
}
