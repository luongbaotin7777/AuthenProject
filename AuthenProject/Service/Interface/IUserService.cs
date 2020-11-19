using AuthenProject.Common;
using AuthenProject.Dtos;

using AuthenProject.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthenProject.Service.Interface
{
    public interface IUserService
    {
        Task<MessageReponse> RegisterUSer(RegisterUserModel model);
        Task<MessageReponse> LoginUser(LoginUserModel model);
        Task<List<GetAllUserReponse>> GetAllUser();
        Task<GetUserByIdReponse> GetUserById(Guid UserId);
        Task<MessageReponse> DeleteUser(Guid UserId);
        Task<MessageReponse> UpdateUser(Guid UserId, UpdateUserModel model);
        Task<MessageReponse> ChangePassword(string UserName,string currentPassword,string newPassword, string passwordConfirm);

    }
}
