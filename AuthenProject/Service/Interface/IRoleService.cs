using AuthenProject.Common;
using AuthenProject.Dtos;
using AuthenProject.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthenProject.Service.Interface
{
   
    public interface IRoleService
    {
        Task<MessageReponse> CreateRole(CreateRoleModel model);
        Task<List<GetAllRoleModel>> GetAllRole(string RoleName);

        Task<GetAllRoleModel> GetRoleById(string RoleId);
        Task<MessageReponse> UpdateRole(Guid RoleId, CreateRoleModel model);
        Task<MessageReponse> DeleteRole(Guid RoleId);

        Task<MessageReponse> AddUserToRole(AddToRoleModel model);
        Task<MessageReponse> RemoveUserFromRole(AddToRoleModel model);
        Task<MessageReponse> AddClaimToRole(string RoleName);
    }
}
