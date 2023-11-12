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
    public class SupplierService : UserService, ISupplierService
    {
        public SupplierService(ILogger<UserService> logger, IHttpContextAccessor httpContext) : base(logger, httpContext)
        {
        }
        public void CreateSupplierUser(CreateSupplierDto input)
        {
            _logger.LogInformation($"{nameof(CreateSupplierUser)}: input = {JsonSerializer.Serialize(input)}");
            input.Password = CryptographyUtils.CreateMD5(input.Password);
            var transaction = _dbContext.Database.BeginTransaction();
            var addSupplier = _dbContext.Suppilers.Add(new Suppiler
            {
                FullName = input.FullName,
                ShortName = input.ShortName,
                Address = input.Address,
                TaxCode = input.TaxCode,     
            }).Entity;
            _dbContext.SaveChanges();
            var result = _dbContext.Users.Add(new User
            {
                SupplierId = addSupplier.Id,
                Phone = input.Phone,
                Email = input.Email,
                Status = UserStatus.ACTIVE,
                UserType = UserTypes.SUPPLIER,
                Password = input.Password,
                Username = input.Username,
            });
            _dbContext.SaveChanges();

            transaction.Commit();
        }
    }
}
