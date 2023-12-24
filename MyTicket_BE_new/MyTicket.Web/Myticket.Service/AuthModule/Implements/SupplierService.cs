using DocumentFormat.OpenXml.Office2010.Excel;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using MYTICKET.BASE.SERVICE.Common;
using MYTICKET.UTILS.ConstantVariables.Shared;
using MYTICKET.UTILS.ConstantVariables.User;
using MYTICKET.UTILS.CustomException;
using MYTICKET.UTILS.Linq;
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

        public void CreateSupplierAccount(CreateSupplierAccountDto input)
        {
            var supplier = _dbContext.Suppilers.FirstOrDefault(u => u.Id == input.SupplierId && !u.Deleted)?? throw new UserFriendlyException(ErrorCode.CustomerNotFound);
            input.Password = CryptographyUtils.CreateMD5(input.Password);
            _dbContext.Users.Add(new User
            {
                SupplierId = input.SupplierId,
                Phone = input.Phone,
                Email = input.Email,
                Status = UserStatus.ACTIVE,
                UserType = UserTypes.SUPPLIER,
                Password = input.Password,
                Username = input.Username,
            });
            _dbContext.SaveChanges();
        }

        public void CreateSupplierUser(CreateSupplierDto input)
        {
            _logger.LogInformation($"{nameof(CreateSupplierUser)}: input = {JsonSerializer.Serialize(input)}");

            var transaction = _dbContext.Database.BeginTransaction();
            var addSupplier = _dbContext.Suppilers.Add(new Suppiler
            {
                FullName = input.FullName,
                ShortName = input.ShortName,
                Address = input.Address,
                TaxCode = input.TaxCode,     
            }).Entity;
            _dbContext.SaveChanges();
            transaction.Commit();
        }

        public SupplierAccountDto GetAccountById(int Id)
        {
            var accounts = _dbContext.Users.Where(s => s.Id == Id && s.UserType == UserTypes.SUPPLIER).Select(s => new SupplierAccountDto
            {
                Id = s.Id,
                Email = s.Email,
                Phone = s.Phone,
                Username = s.Username,
                Password = s.Password
            }).FirstOrDefault() ?? throw new UserFriendlyException(ErrorCode.NotFound);
            return accounts;
        }

        public PagingResult<SupplierDto> GetAll(FilerSupplierDto input)
        {
            _logger.LogInformation($"{nameof(GetAll)}: input = {JsonSerializer.Serialize(input)}");
            var result = new PagingResult<SupplierDto>();
            var supplliers = _dbContext.Suppilers.Where(u => !u.Deleted && (input.Keyword == null || (u.FullName.Contains(input.Keyword) || (u.ShortName.Contains(input.Keyword)))));
            // đếm tổng trước khi phân trang
            result.TotalItems = supplliers.Count();
            supplliers = supplliers.OrderDynamic(input.Sort);

            if (input.PageSize != -1)
            {
                supplliers = supplliers.Skip(input.GetSkip()).Take(input.PageSize);
            }
            result.Items = _mapper.Map<List<SupplierDto>>(supplliers);
            return result;
        }

        public SupplierDetailDto GetById(int Id)
        {
            _logger.LogInformation($"{nameof(GetAll)}: Id = {Id}");

            var supplier = _dbContext.Suppilers.Where(u => u.Id == Id && !u.Deleted)
                .Select(s => new SupplierDetailDto
                {
                    Id= s.Id,
                    FullName = s.FullName,
                    ShortName = s.ShortName,
                    Address = s.Address,
                    TaxCode = s.TaxCode,
                })
                .FirstOrDefault() ?? throw new UserFriendlyException(ErrorCode.CustomerNotFound);

            var accounts = _dbContext.Users.Where(s => s.SupplierId == Id && s.UserType == UserTypes.SUPPLIER).Select(s => new SupplierAccountDto 
            {
                Id = s.Id,
                Email = s.Email,
                Phone = s.Phone,
                Username = s.Username,
                Password = s.Password
            }).ToList();
            supplier.Accounts = accounts;
            return supplier;

        }

        public void UpdateSupplierAccount(UpdateSupplierAccountDto input)
        {
            var account = _dbContext.Users.FirstOrDefault(u => u.Id == input.Id && u.UserType == UserTypes.SUPPLIER && !u.Deleted) ?? throw new UserFriendlyException(ErrorCode.UserNotFound);
            account.Username = input.Username;
            account.Email = input.Email;
            account.Phone = input.Phone;
            _dbContext.SaveChanges();
        }

        public void UpdateSupplierUser(UpdateSupplierDto input)
        {
            var supplier = _dbContext.Suppilers.FirstOrDefault(u => u.Id == input.Id && !u.Deleted) ?? throw new UserFriendlyException(ErrorCode.CustomerNotFound);
            supplier.FullName = input.FullName;
            supplier.ShortName = input.ShortName;
            supplier.Address = input.Address;
            supplier.TaxCode = input.TaxCode;
            _dbContext.SaveChanges();
        }
    }
}
