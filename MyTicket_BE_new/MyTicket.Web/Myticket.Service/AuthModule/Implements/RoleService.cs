using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using MYTICKET.BASE.SERVICE.Common;
using MYTICKET.UTILS.ConstantVariables.Shared;
using MYTICKET.UTILS.CustomException;
using MYTICKET.WEB.DOMAIN.Entities;
using MYTICKET.WEB.SERVICE.AuthModule.Abstracts;
using MYTICKET.WEB.SERVICE.AuthModule.Dtos.RoleDto;
using MYTICKET.WEB.SERVICE.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MYTICKET.WEB.SERVICE.AuthModule.Implements
{
    public class RoleService : ServiceBase, IRoleService
    {
        public RoleService(ILogger<RoleService> logger, IHttpContextAccessor httpContext) : base(logger, httpContext)
        {
        }
        public RoleDto Add(CreateRolePermissionDto input)
        {
            _logger.LogInformation($"{nameof(Add)}: input = {JsonSerializer.Serialize(input)}");
            var roleInsert = _mapper.Map<Role>(input);
            _dbContext.Add(roleInsert);
            _dbContext.SaveChanges();

            foreach (var item in input.PermissionKeys)
            {
                _dbContext.RolePermissions.Add(new RolePermission
                {
                    RoleId = roleInsert.Id,
                    PermissionKey = item
                });
            }
            _dbContext.SaveChanges();
            return _mapper.Map<RoleDto>(roleInsert);
        }

        public void Delete(int id)
        {
            _logger.LogInformation($"{nameof(Delete)}: id = {id}");
            var role = _dbContext.Roles.FirstOrDefault(e => e.Id == id && !e.Deleted) ?? throw new UserFriendlyException(ErrorCode.RoleNotFound);
            role.Deleted = true;
            var rolePermission = _dbContext.RolePermissions.Where(e => e.RoleId == id);
            foreach (var item in rolePermission)
            {
                _dbContext.Remove(item);
            }
            _dbContext.SaveChanges();
        }

        public PagingResult<RoleDto> FindAll(FilterRoleDto input)
        {

            _logger.LogInformation($"{nameof(FindAll)}:");
            var rolePermissions = _dbContext.Roles.Where(e => !e.Deleted && (input.UserType == null || e.UserType == input.UserType));
            var result = new PagingResult<RoleDto>();
            result.TotalItems = rolePermissions.Count();
            result.Items = _mapper.Map<List<RoleDto>>(rolePermissions).ToList();
            return result;
        }

        public RoleDto FindById(int id)
        {
            _logger.LogInformation($"{nameof(FindById)}: id = {id}");
            var role = _dbContext.Roles.FirstOrDefault(e => e.Id == id && !e.Deleted) ?? throw new UserFriendlyException(ErrorCode.RoleNotFound);

            var result = _mapper.Map<RoleDto>(role);
            var rolePermission = _dbContext.RolePermissions.Where(e => e.RoleId == id);
            result.PermissionDetails = rolePermission.ToList();
            return result;
        }

        public RoleDto Update(UpdateRolePermissionDto input)
        {
            _logger.LogInformation($"{nameof(Update)}: input = {JsonSerializer.Serialize(input)}");
            var role = _dbContext.Roles.FirstOrDefault(e => e.Id == input.Id && !e.Deleted);
            if (role == null)
            {
                throw new UserFriendlyException(ErrorCode.RoleNotFound);
            }
            role.Name = input.Name;
            role.Description = input.Description;

            var removeListPermission = new List<string>();
            var insertListPermission = new List<RolePermission>();

            //List permission input
            var inputListPermission = new List<RolePermission>();

            //List permission có trong db
            var currentListPermission = _dbContext.RolePermissions.Where(e => e.RoleId == input.Id).Select(e => e.PermissionKey).ToList();

            //List Rolepermission bị xóa
            removeListPermission = currentListPermission.Except(input.PermissionKeys).ToList();
            foreach (var item in removeListPermission)
            {
                var removeItem = _dbContext.RolePermissions.FirstOrDefault(e => e.PermissionKey == item && e.RoleId == input.Id);
                _dbContext.RolePermissions.Remove(removeItem);
            }

            foreach (var item in input.PermissionKeys)
            {
                //Thêm các rolepermission với các permission key từ input vào List permission input 
                var rolePermission = _dbContext.RolePermissions.FirstOrDefault(e => e.RoleId == input.Id && e.PermissionKey == item);
                if (rolePermission == null)
                {
                    _dbContext.RolePermissions.Add(new RolePermission
                    {
                        RoleId = input.Id,
                        PermissionKey = item,
                    });
                }
            }
            _dbContext.SaveChanges();
            return _mapper.Map<RoleDto>(role);
        }
    }
}
