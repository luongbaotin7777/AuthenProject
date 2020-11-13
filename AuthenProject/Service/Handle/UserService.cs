using AuthenProject.Common;
using AuthenProject.Dtos;
using AuthenProject.Entities;
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
        //private readonly RoleManager<AppRole> _roleManager;
        private readonly ITokenService _tokenService;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IConfiguration _configuration;
        private readonly IPasswordHasher<AppUser> _passwordHasher;
        private readonly IRoleService _roleService;
        public UserService(UserManager<AppUser> userManager, /*RoleManager<AppRole> roleManager*/ SignInManager<AppUser> signInManager, IConfiguration configuration, IPasswordHasher<AppUser> passwordHasher, IRoleService roleService, ITokenService tokenService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _passwordHasher = passwordHasher;
            _roleService = roleService;
            _tokenService = tokenService;
            //_roleManager = roleManager;
        }

        public async Task<MessageReponse> DeleteUser(string UserId)
        {
            var user = await _userManager.FindByIdAsync(UserId.ToString());
            if (user == null) throw new Exception($"{UserId} not Found");
            var result = await _userManager.DeleteAsync(user);
            if (result.Succeeded)
            {
                return new MessageReponse()
                {
                    Message = "User Deleted",
                    IsSuccess = true
                };
            }
            return new MessageReponse()
            {
                Message = "Delete Failed",
                IsSuccess = false
            };
        }

        public async Task<List<GetAllUserReponse>> GetAllUser(string UserName,string Email)
        {
            if (!string.IsNullOrEmpty(UserName) || !string.IsNullOrEmpty(Email))
            {
                var users = _userManager.Users.Where(x => x.UserName.Contains(UserName) || x.Email.Contains(Email));

                var result = await users.Select(x => new GetAllUserReponse()
                {
                    Id = x.Id,
                    UserName = x.UserName,
                    Email = x.Email,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    Dob = x.Dob,
                    PhoneNumber = x.PhoneNumber
                }).ToListAsync();
                return result;
            }
            else
            {
                var users = await _userManager.Users.Select(x => new GetAllUserReponse()
                {
                    Id = x.Id,
                    UserName = x.UserName,
                    Email = x.Email,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    Dob = x.Dob,
                    PhoneNumber = x.PhoneNumber,
                    
                }).ToListAsync();
                
                return users;
            }
           
        }

        public async Task<GetUserByIdReponse> GetUserById(string UserId)
        {
            var users = await _userManager.FindByIdAsync(UserId.ToString());
            if (users == null) throw new Exception($"{UserId} not found");
            
            var claims = await _userManager.GetClaimsAsync(users);
            var roles = await _userManager.GetRolesAsync(users);
            var data = new GetUserByIdReponse()
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

        public async Task<MessageReponse> LoginUser(LoginUserModel model)
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
              return  await _tokenService.GenerateJWTToken(model.Username);
                //var userRoles = await _userManager.GetRolesAsync(userName);
                //var claim = new List<Claim>()
                //{
                //    new Claim(ClaimTypes.NameIdentifier,userName.Id.ToString()),
                //    new Claim(ClaimTypes.GivenName,userName.FirstName),
                //    new Claim(ClaimTypes.Surname,userName.LastName),
                //    new Claim(ClaimTypes.Email,userName.Email),
                //    new Claim(ClaimTypes.Role,string.Join(";",userRoles)),
                //};
                //var authSignKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwts:Key"]));
                //var token = new JwtSecurityToken(
                //        claims: claim,
                //        expires: DateTime.UtcNow.AddDays(7),
                //        signingCredentials: new SigningCredentials(authSignKey, SecurityAlgorithms.HmacSha256)
                //    );
                //string TokenAsString = new JwtSecurityTokenHandler().WriteToken(token);

                //return new MessageReponse()
                //{
                //    Message = TokenAsString,
                //    IsSuccess = true,
                //    ExpireDate = token.ValidTo
                //};

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
        public async Task<MessageReponse> RegisterUSer(RegisterUserModel model)
        {
            if (model.Password != model.ConfirmPassword)
            {
                return new MessageReponse()
                {
                    Message = "ConfirmPassword does not map Password ",
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
                        Message = "This user is not belong to any role !",
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

        public async Task<MessageReponse> UpdateUser(string UserId, UpdateUserModel model)
        {
            if (await _userManager.Users.AnyAsync(x => x.Email == model.Email && x.Id.ToString() != UserId))
            {
                return new MessageReponse()
                {
                    Message = "Email already exists !",
                    IsSuccess = false,
                };
            }
            var user = await _userManager.FindByIdAsync(UserId);
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

