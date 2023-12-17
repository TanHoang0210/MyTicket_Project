using MYTICKET.WEB.SERVICE.AuthModule.Dtos.UserDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MYTICKET.WEB.SERVICE.AuthModule.Abstracts
{
    public interface ICustomerService : IUserService
    {
        /// <summary>
        /// Thêm user kh
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        void CreateCustomerUser(CreateCustomerUserDto input);

        /// <summary>
        /// Thêm user kh
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        void UpdateCustomerUser(UpdateCustomerUserDto input);


    }
}