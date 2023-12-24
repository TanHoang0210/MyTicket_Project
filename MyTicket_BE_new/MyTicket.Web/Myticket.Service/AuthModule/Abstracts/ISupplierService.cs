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
        /// Thêm user ncc
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        void CreateSupplierAccount(CreateSupplierAccountDto input);

        /// <summary>
        /// Danh sasch ncc
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        PagingResult<SupplierDto> GetAll(FilerSupplierDto input);

        /// <summary>
        /// Thông tin chi tiết ncc
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        SupplierDetailDto GetById(int Id);

        /// <summary>
        /// Thông tin chi tiết ncc
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        SupplierAccountDto GetAccountById(int Id);

        /// <summary>
        /// cap nhat thong tin ncc
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        void UpdateSupplierUser(UpdateSupplierDto input);

        /// <summary>
        /// Cap nhat tai khoan ncc
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        void UpdateSupplierAccount(UpdateSupplierAccountDto input);
    }
}
