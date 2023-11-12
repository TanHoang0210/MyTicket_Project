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
        public void CreateCustomerUser(CreateCustomerUserDto input)
        {
            _logger.LogInformation($"{nameof(CreateCustomerUser)}: input = {JsonSerializer.Serialize(input)}");
            input.Password = CryptographyUtils.CreateMD5(input.Password);
            var transaction = _dbContext.Database.BeginTransaction();
            var addCustomer = _dbContext.Customers.Add(new Customer
            {
                FirstName = input.FirstName,
                LastName = input.LastName,
            }).Entity;
            _dbContext.SaveChanges();
            var result = _dbContext.Users.Add(new User
            {
                CustomerId = addCustomer.Id,
                Phone = input.Phone,
                Email = input.Email,
                Status = UserStatus.ACTIVE,
                UserType = UserTypes.CUSTOMER,
                Password = input.Password,
                Username = input.Username,     
            });
            _dbContext.SaveChanges();

            transaction.Commit();
        }
    }
}
