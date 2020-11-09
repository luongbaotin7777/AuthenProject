using AuthenProject.Common;
using AuthenProject.Dtos;
using AuthenProject.EFModel;
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
        Task<List<GetAllUserReponse>> GetAllUser(string UserName,string Email);
        Task<GetUserByIdReponse> GetUserById(string UserId);
        Task<MessageReponse> DeleteUser(string UserId);
        Task<MessageReponse> UpdateUser(string UserId, UpdateUserModel model);


    }
}
