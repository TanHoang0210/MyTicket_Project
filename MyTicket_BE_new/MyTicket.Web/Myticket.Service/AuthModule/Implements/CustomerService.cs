using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
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
using MYTICKET.WEB.SERVICE.MailService.Abstracts;
using MYTICKET.WEB.SERVICE.MailService.Dtos;
using System.Text.Json;

namespace MYTICKET.WEB.SERVICE.AuthModule.Implements
{
    public class CustomerService : UserService, ICustomerService
    {
        private readonly IEmailSenderService _mail;
        public CustomerService(ILogger<UserService> logger, IEmailSenderService mail, IHttpContextAccessor httpContext) : base(logger, httpContext)
        {
            _mail = mail;
        }
        public void CreateCustomerUser(CreateCustomerUserDto input)
        {
            _logger.LogInformation($"{nameof(CreateCustomerUser)}: input = {JsonSerializer.Serialize(input)}");
            input.Password = CryptographyUtils.CreateMD5(input.Password);
            var transaction = _dbContext.Database.BeginTransaction();
            if (_dbContext.Users.Any(s => s.Email == input.Email && s.UserType == UserTypes.CUSTOMER && !s.Deleted))
            {
                throw new UserFriendlyException(ErrorCode.EmailUsed);
            }
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

        public PagingResult<CurrentCustomerDto> GetAll(FilterCustomerDto input)
        {
            var result = new PagingResult<CurrentCustomerDto>();
            var query = (from user in _dbContext.Users
                         join customer in _dbContext.Customers on user.CustomerId equals customer.Id
                         where !user.Deleted && !customer.Deleted
                         && (input.Keyword == null || (customer.FirstName.Contains(input.Keyword) || customer.LastName.Contains(input.Keyword)))
                         select new CurrentCustomerDto
                         {
                             Id = customer.Id,
                             Address = customer.Address,
                             Country = customer.Country,
                             DateOfBirth = customer.DateOfBirth,
                             Email = user.Email,
                             FirstName = customer.FirstName,
                             Gender = customer.Gender,
                             LastName = customer.LastName,
                             Nationality = customer.Nationality,
                             Password = user.Password,
                             Phone = user.Phone,
                             Username = user.Username
                         });
            result.TotalItems = query.Count();
            query = query.OrderDynamic(input.Sort);

            if (input.PageSize != -1)
            {
                query = query.Skip(input.GetSkip()).Take(input.PageSize);
            }
            result.Items = query;
            return result;
        }

        public CurrentCustomerDto GetById(int id)
        {
            var query = (from user in _dbContext.Users
                         join customer in _dbContext.Customers on user.CustomerId equals customer.Id
                         where !user.Deleted && !customer.Deleted
                         && customer.Id == id
                         select new CurrentCustomerDto
                         {
                             Id = customer.Id,
                             Address = customer.Address,
                             Country = customer.Country,
                             DateOfBirth = customer.DateOfBirth,
                             Email = user.Email,
                             FirstName = customer.FirstName,
                             Gender = customer.Gender,
                             LastName = customer.LastName,
                             Nationality = customer.Nationality,
                             Password = user.Password,
                             Phone = user.Phone,
                             Username = user.Username
                         }).FirstOrDefault() ?? throw new UserFriendlyException(ErrorCode.CustomerNotFound);
            return query;
        }

        public async Task SetCustomerPassword(SetPasswordCustomerDto input)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(e => e.Email == input.Email && !e.Deleted)
    ?? throw new UserFriendlyException(ErrorCode.UserNotFound);
            var password = CommonUtils.GenerateCode(8);
            user.Password = CryptographyUtils.CreateMD5(password);
            await _dbContext.SaveChangesAsync();
            // Lấy dịch vụ sendmailservice
            try
            {
                // Lấy dịch vụ sendmailservice
                MailContent content = new MailContent
                {
                    To = user.Email,
                    Subject = $"[Yêu cầu đổi mật khẩu thành công!]",
                    Body = $@"
                                <div  style=""background-color: rgb(226, 168, 140);
                                 width: 50%;flex-direction: column; margin: auto;
                                 "">
                                    <h1 style=""font-weight: bold; width: 100%;
                                    text-align: center;
                                    background-color:rgb(188, 101, 60) ; 
                                    color: white;
                                    padding: 10px 0;
                                    "">
                                    MyTicket - Ứng dụng đặt vé số 1 Việt Nam
                                    </h1>
                                 <div style="" display: flex; padding: 20px 0;"">
                                     <div>
                                         <img style=""width: 200px; height: 200px;"" src=""https://i.postimg.cc/jdzQ25TR/logo-pink-textcolor.png"" alt="""">
                                     </div>
                                     <div style=""margin: auto; flex-direction: column; text-align: center; color: #555; font-size:1.3rem;"">
                                          Mật khẩu mới của bạn là: <strong>{password}</strong>
                                     </div>
                                 </div>
                                </div>
                                "
                };
                await _mail.SendMail(content);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to send email: " + ex.Message);
            }
        }

        public void UpdateCustomerUser(UpdateCustomerUserDto input)
        {
            var currentUserId = CommonUtils.GetCurrentUserId(_httpContext);
            _logger.LogInformation($"{nameof(UpdateCustomerUser)}: input = {JsonSerializer.Serialize(input)}, currentUserId = {currentUserId}");
            var currentUser = _dbContext.Users.FirstOrDefault(s => s.Id == currentUserId && !s.Deleted) ?? throw new UserFriendlyException(ErrorCode.UserNotFound);
            var currentCustomer = _dbContext.Customers.FirstOrDefault(s => s.Id == currentUser.CustomerId && !s.Deleted) ?? throw new UserFriendlyException(ErrorCode.CustomerNotFound);
            if (input.FirstName != null)
            {
                currentCustomer.FirstName = input.FirstName;
            }
            if (input.LastName != null)
            {
                currentCustomer.LastName = input.LastName;
            }
            if (input.Address != null)
            {
                currentCustomer.Address = input.Address;
            }
            if (input.DateOfBirth != null)
            {
                currentCustomer.DateOfBirth = input.DateOfBirth;
            }
            if (input.Country != null)
            {
                currentCustomer.Country = input.Country;
            }
            if (input.Nationality != null)
            {
                currentCustomer.Nationality = input.Nationality;
            }
            if (input.Gender != 0)
            {
                currentCustomer.Gender = input.Gender.Value;
            }
            _dbContext.SaveChanges();
        }
    }
}
