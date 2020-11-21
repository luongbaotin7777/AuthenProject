using AuthenProject.Common;
using AuthenProject.Dtos;
using AuthenProject.Entities;
using AuthenProject.Repository;
using AuthenProject.Service.Interface;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace AuthenProject.Service.Handle
{
    public class UserService : IUserService
    {
        private readonly UserManager<AppUser> _userManager;
       
        private readonly ITokenService _tokenService;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IConfiguration _configuration;
        private readonly IPasswordHasher<AppUser> _passwordHasher;
        private readonly IRoleService _roleService;
        private IUnitOfWork _unitofwork;
        public UserService(IUnitOfWork unitofwork, UserManager<AppUser> userManager,SignInManager<AppUser> signInManager, IConfiguration configuration, IPasswordHasher<AppUser> passwordHasher, IRoleService roleService, ITokenService tokenService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _passwordHasher = passwordHasher;
            _roleService = roleService;
            _tokenService = tokenService;
            _unitofwork = unitofwork;
        }

        public async Task<MessageReponse> ChangePassword(string UserName,string currentPassword,string passwordConfirm ,string newPassword)
        {
            var user = await _userManager.FindByNameAsync(UserName);
            if(user != null)
            {
             if(newPassword == passwordConfirm)
                {
                    var result =  await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);
                    if (result.Succeeded)
                    {
                        return new MessageReponse()
                        {
                            Message = "Change Password Successfully",
                            IsSuccess = false
                        };
                    }
                    return new MessageReponse()
                    {
                        Message = "Incorrect Current Password",
                        IsSuccess = false
                    };
                }
               
                return new MessageReponse()
                {
                    Message = "The confirmation password is not the same as the password",
                    IsSuccess = false
                };
            }
            return new MessageReponse()
            {
                Message = "Incorrect UserName",
                IsSuccess = false
            };
        }

        public async Task<MessageReponse> DeleteUser(Guid UserId)
        {
            var user = await _unitofwork.User.FindByIdAsync(UserId);
            if (user == null) throw new Exception($"{UserId} not Found");
             _unitofwork.User.Delete(user);
            await _unitofwork.SaveAsync();
            return new MessageReponse()
            {
                Message = "Deleted successfully",
                IsSuccess = false
            };
        }

        public async Task<List<UserDtos>> GetAllUser()
        {
            var users = await _unitofwork.User.GetAllAsync();    
            var listUser = new List<UserDtos>();
            foreach(var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                var claims = await _userManager.GetClaimsAsync(user);
                var data = new UserDtos()
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    PhoneNumber = user.PhoneNumber,
                    Dob = user.Dob,
                    Roles = roles,
                    Claims = claims
                    
                };
                listUser.Add(data);
            }
            return listUser;
           
        }

        public async Task<UserDtos> GetUserById(Guid UserId)
        {
            var users = await _unitofwork.User.FindByIdAsync(UserId);
            if (users == null) throw new Exception($"{UserId} not found");
            
            var claims = await _userManager.GetClaimsAsync(users);
            var roles = await _userManager.GetRolesAsync(users);
            var data = new UserDtos()
            {
                Id = users.Id,
                UserName = users.UserName,
                FirstName = users.FirstName,
                LastName = users.LastName,
                Dob = users.Dob,
                Email = users.Email,
                PhoneNumber = users.PhoneNumber,
                Roles = roles,
                Claims = claims

            };
            return data;
        }

        public async Task<MessageReponse> LoginUser(LoginDtos model)
        {
            var userName = await _userManager.FindByNameAsync(model.Username);
            if (userName == null)
            {
                return new MessageReponse()
                {
                    Message = "UserName not found! ",
                    IsSuccess = false,
                };
            }
            var result = await _signInManager.PasswordSignInAsync(userName, model.Password, true, false);
            if (result.Succeeded)
            {
              return  await _tokenService.GenerateJWTToken(model.Username,1);
               

            }
            else
            {
                return new MessageReponse()
                {
                    Message = "Login Failed",
                    IsSuccess = true,

                };
            };

        }
        public async Task<MessageReponse> RegisterUSer(RegisterRequestDto model)
        {
            if (model.Password != model.ConfirmPassword)
            {
                return new MessageReponse()
                {
                    Message = "ConfirmPassword  is not the same Password ",
                    IsSuccess = true,
                };
            }
            var userName = await _userManager.FindByNameAsync(model.UserName);
            if (userName != null)
            {
                return new MessageReponse()
                {
                    Message = "UserName already exists !",
                    IsSuccess = false,
                };
            }
            var userEmail = await _userManager.FindByEmailAsync(model.Email);
            if (userEmail != null)
            {
                return new MessageReponse()
                {
                    Message = "UserEmail already exists !",
                    IsSuccess = false,
                };
            }
            if (!model.Dob.HasValue)
            {
                return new MessageReponse()
                {
                    Message = "Dob is Required !",
                    IsSuccess = false,
                };
            }
           
            var user = new AppUser()
            {
                UserName = model.UserName.ToUpper(),
                Email = model.Email.ToUpper(),
                FirstName = model.FirstName.ToUpper(),
                LastName = model.LastName.ToUpper(),
                PhoneNumber = model.PhoneNumber,
                Dob = model.Dob,
            };

           

            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                var claims = new List<Claim>()
                {

                    new Claim(ClaimTypes.GivenName,model.FirstName),
                    new Claim(ClaimTypes.Surname,model.LastName),
                    new Claim(ClaimTypes.Email,model.Email),
                };
                    await _userManager.AddClaimsAsync(user, claims);

                var listRoleModel = new ArrayList();
                if(model.ListRoleName == null)
                {
                    return new MessageReponse()
                    {
                        Message = "Register Successed",
                        IsSuccess = true,
                    };
                }
                foreach(var role in model.ListRoleName)
                {
                    var roleModel = new AddToRoleModel()
                    {
                        UserName = model.UserName,
                        RoleName = role
                    };
                    listRoleModel.Add(roleModel);
                }

                if(listRoleModel == null)
                {
                    return new MessageReponse()
                    {
                        Message = "Add role failed !",
                        IsSuccess = false,
                    };
                }
                foreach(AddToRoleModel roleModel in listRoleModel)
                {

                    await _roleService.AddUserToRole(roleModel);
                }
                return new MessageReponse()
                {
                    Message = "Register Successed !",
                    IsSuccess = true,
                };
            }
            return new MessageReponse()
            {
                Message = "Register Failed !",
                IsSuccess = false,
                
            };
        }

        public async Task<MessageReponse> UpdateUser(Guid UserId, UpdateUserModel model)
        {
            if (await _unitofwork.User.GetByAnyConditionAsync(x => x.Email == model.Email && x.Id != UserId))
            {
                return new MessageReponse()
                {
                    Message = "Email already exists !",
                    IsSuccess = false,
                };
            }
            var user = await _unitofwork.User.FindByIdAsync(UserId);
            if(user == null)
            {
                return new MessageReponse()
                {
                    Message = "User Id not found !",
                    IsSuccess = false,
                };
            }
            if (!string.IsNullOrEmpty(model.FirstName))
            {
                user.FirstName = model.FirstName;
            }
            else
            {
                user.FirstName = user.FirstName;
            }
            if (!string.IsNullOrEmpty(model.LastName))
            {
                user.LastName = model.LastName;
            }
            else
            {
                user.LastName = model.LastName;
            }
            if (!string.IsNullOrEmpty(model.PhoneNumber))
            {
                user.PhoneNumber = model.PhoneNumber;
            }
            else
            {
                user.PhoneNumber = model.PhoneNumber;
            }
            if (!string.IsNullOrEmpty(model.Email))
            {
                user.Email = model.Email;
            }
            else
            {
                user.Email = model.Email;
            }
            if (model.Dob.HasValue)
            {
                user.Dob = (DateTime)model.Dob;
            }
            else
            {
                return new MessageReponse()
                {
                    Message = "Date of birth is required !",
                    IsSuccess = false,
                };
            }
            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                return new MessageReponse()
                {
                    Message = "Update Successed",
                    IsSuccess = true
                };
            }
            return new MessageReponse()
            {
                Message = "Update Failed",
                IsSuccess = false
            };

        }
    }
}

