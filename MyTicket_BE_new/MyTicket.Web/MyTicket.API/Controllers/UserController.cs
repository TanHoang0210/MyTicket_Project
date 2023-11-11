using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MYTICKET.BASE.API.Controller;
using MYTICKET.BASE.SERVICE.Common;
using MYTICKET.UTILS;
using MYTICKET.WEB.DOMAIN.Entities;
using MYTICKET.WEB.SERVICE.AuthModule.Abstracts;
using MYTICKET.WEB.SERVICE.AuthModule.Dtos.UserDto;
using System.Net;

namespace MYTICKET.WEB.API.Controllers
{
    [Authorize]
    [Route("myticket/api/user")]
    [ApiController]
    public class UserController : ApiControllerBase
    {
        private readonly IUserService _userService;
        private readonly ICustomerService _customerService;
        private readonly ISupplierService _supplierService;

        public UserController(IUserService userService, ICustomerService customerService, ISupplierService supplierService)
        {
            _userService = userService;
            _customerService = customerService;
            _supplierService = supplierService;
        }
        /// <summary>
        /// Xem danh sách người dùng
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet("find-all")]
        //[PermissionFilter(PermissionKeys.UserTable)]
        public APIResponse<PagingResult<UserDto>> FindAll([FromQuery] FilterUserDto input)
            => new(_userService.FindAll(input));
        /// <summary>
        /// Tìm kiếm người dùng theo Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(APIResponse<User>), (int)HttpStatusCode.OK)]
        public APIResponse FindById(int id)
            => new(_userService.FindById(id));

        /// <summary>
        /// Thêm User
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("add")]
        //[PermissionFilter(PermissionKeys.UserCreate)]
        public APIResponse Add([FromBody] CreateUserDto input)
        {
            _userService.CreateUser(input);
            return new();
        }

        /// <summary>
        /// Thêm User Khach hang
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("add-by-customer")]

        public APIResponse AddByCustomer([FromBody] CreateUserDto input)
        {
            _customerService.CreateUser(input);
            return new();
        }

        /// <summary>
        /// Thêm User nha cung cap
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("add-by-suplier")]

        public APIResponse AddBySuplier([FromBody] CreateUserDto input)
        {
            _supplierService.CreateUser(input);
            return new();
        }

        /// <summary>
        /// Cập nhật User
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut("update")]
        //[PermissionFilter(PermissionKeys.UserUpdate)]
        public APIResponse Update([FromBody] UpdateUserDto input)
        {
            _userService.Update(input);
            return new();
        }

        /// <summary>
        /// Xóa User
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut("delete/{id}")]
        //[PermissionFilter(PermissionKeys.UserDelete)]
        public APIResponse Delete(int id)
        {
            _userService.Delete(id);
            return new();
        }

        /// <summary>
        /// Thay đổi trạng thái tai khoản
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut("change-status/{id}")]
        //[PermissionFilter(PermissionKeys.UserChangeStatus)]
        public APIResponse ChangeStatus(int id)
        {
            _userService.ChangeStatus(id);
            return new();
        }

        [HttpPut("set-password")]
        public APIResponse SetPassword(SetPasswordUserDto input)
        {
            _userService.SetPassword(input);
            return new();
        }
    }
}
