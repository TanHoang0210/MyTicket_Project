using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using MYTICKET.BASE.SERVICE.Common;
using MYTICKET.UTILS.ConstantVariables.Shared;
using MYTICKET.UTILS.ConstantVariables.User;
using MYTICKET.UTILS.CustomException;
using MYTICKET.UTILS.Security;
using MYTICKET.WEB.DOMAIN.Entities;
using MYTICKET.WEB.SERVICE.AuthModule.Abstracts;
using MYTICKET.WEB.SERVICE.AuthModule.Dtos.UserDto;
using System.Text.Json;

namespace MYTICKET.WEB.SERVICE.AuthModule.Implements
{
    public class CustomerService : UserService, ICustomerService
    {
        public CustomerService(ILogger<UserService> logger, IHttpContextAccessor httpContext) : base(logger, httpContext)
        {
        }
        public override void CreateUser(CreateUserDto input)
        {
            _logger.LogInformation($"{nameof(CreateUser)}: input = {JsonSerializer.Serialize(input)}");
            var userId = CommonUtils.GetCurrentUserId(_httpContext);
            input.Password = CryptographyUtils.CreateMD5(input.Password);
            var transaction = _dbContext.Database.BeginTransaction();
            var user = _mapper.Map<User>(input);


            if (input.Status == null)
            {
                user.Status = UserStatus.ACTIVE;
            }
            user.UserType = UserTypes.CUSTOMER;

            var result = _dbContext.Users.Add(user);


            _dbContext.SaveChanges();

            //Thêm role
            if (input.RoleIds != null)
            {
                foreach (var item in input.RoleIds)
                {
                    var role = _dbContext.Roles.FirstOrDefault(e => e.Id == item) ?? throw new UserFriendlyException(ErrorCode.RoleNotFound);
                    _dbContext.UserRoles.Add(new UserRole
                    {
                        UserId = result.Entity.Id,
                        RoleId = item
                    });
                }
                _dbContext.SaveChanges();
            }
            transaction.Commit();
        }
    }
}
