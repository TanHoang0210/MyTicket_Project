﻿using Microsoft.AspNetCore.Http;
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

        public PagingResult<CurrentCustomerDto> GetAll(FilterCustomerDto input)
        {
            var result = new PagingResult<CurrentCustomerDto>();
            var query = (from user in _dbContext.Users
                         join customer in _dbContext.Customers on user.CustomerId equals customer.Id
                         where !user.Deleted && !customer.Deleted
                         && (input.Keyword == null || (customer.FirstName.Contains(input.Keyword)|| customer.LastName.Contains(input.Keyword)))
                         select new CurrentCustomerDto
                         {
                             Id = user.Id,
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

        public void UpdateCustomerUser(UpdateCustomerUserDto input)
        {
            var currentUserId = CommonUtils.GetCurrentUserId(_httpContext);
            _logger.LogInformation($"{nameof(UpdateCustomerUser)}: input = {JsonSerializer.Serialize(input)}, currentUserId = {currentUserId}");
            var currentUser = _dbContext.Users.FirstOrDefault(s => s.Id == currentUserId && !s.Deleted) ?? throw new UserFriendlyException(ErrorCode.UserNotFound);
            var currentCustomer = _dbContext.Customers.FirstOrDefault(s => s.Id == currentUser.CustomerId && !s.Deleted) ?? throw new UserFriendlyException(ErrorCode.CustomerNotFound);
            if(input.FirstName != null)
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
