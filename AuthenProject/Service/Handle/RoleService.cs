using AuthenProject.Authorization;
using AuthenProject.Common;
using AuthenProject.Dtos;
using AuthenProject.EFModel;
using AuthenProject.Entities;
using AuthenProject.Service.Interface;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using static AuthenProject.Authorization.Permission;

namespace AuthenProject.Service.Handle
{
    public class RoleService : IRoleService
    {
        private readonly RoleManager<AppRole> _roleManager;
        private readonly UserManager<AppUser> _userManager;
      
        


        public RoleService(RoleManager<AppRole> roleManager, UserManager<AppUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;

        }

      

        public async Task<MessageReponse> AddClaimToRole(string RoleName)
        {
            var role = await _roleManager.FindByNameAsync(RoleName);
            if (role == null)
            {
                return new MessageReponse()
                {
                    Message = "RoleName not Found",
                    IsSuccess = false,
                };
            }
            if (role.Name == "ADMIN")
            {
                await _roleManager.AddClaimAsync(role, new Claim(CustomClaimTypes.Permission, Permission.Users.Create));
                await _roleManager.AddClaimAsync(role, new Claim(CustomClaimTypes.Permission, Permission.Users.View));
                await _roleManager.AddClaimAsync(role, new Claim(CustomClaimTypes.Permission, Permission.Users.Edit));
                await _roleManager.AddClaimAsync(role, new Claim(CustomClaimTypes.Permission, Permission.Users.Delete));
            }
            if (role.Name == "MOD")
            {
                await _roleManager.AddClaimAsync(role, new Claim(CustomClaimTypes.Permission, Permission.Users.View));
                await _roleManager.AddClaimAsync(role, new Claim(CustomClaimTypes.Permission, Permission.Users.Edit));
            }
            if (role.Name == "USER")
            {
                await _roleManager.AddClaimAsync(role, new Claim(CustomClaimTypes.Permission, Permission.Users.View));
            }
            return new MessageReponse()
            {
                Message = "Add Claim Successed",
                IsSuccess = true,
            };

        }

        public async Task<MessageReponse> AddUserToRole(AddToRoleModel model)
        {
            var user = await _userManager.FindByNameAsync(model.UserName);
            if (user == null)
            {
                return new MessageReponse()
                {
                    Message = "UserName not Found",
                    IsSuccess = false,
                };
            }
            else
            {
                var role = await _roleManager.FindByNameAsync(model.RoleName);
                if (role == null)
                {
                    return new MessageReponse()
                    {
                        Message = "RoleName not Found",
                        IsSuccess = false,
                    };
                }
                else
                {
                    var userRole = await _userManager.GetRolesAsync(user);

                    bool checkrole = userRole.ToList().Contains(model.RoleName);

                    if (checkrole)
                    {
                        return new MessageReponse()
                        {
                            Message = "User already belong to role",
                            IsSuccess = false,
                        };
                    }
                    else
                    {
                        var result = await _userManager.AddToRoleAsync(user, role.Name);
                        if (result.Succeeded)
                        {
                            return new MessageReponse()
                            {
                                Message = "Added successed",
                                IsSuccess = true,
                            };
                        }
                        return new MessageReponse()
                        {
                            Message = "Added Failed",
                            IsSuccess = true,
                        };
                    }
                }

            }



        }

        public async Task<MessageReponse> CreateRole(CreateRoleModel model)
        {
            var role = await _roleManager.FindByNameAsync(model.RoleName);
            if (role != null)
            {
                return new MessageReponse()
                {
                    Message = "Role already exists ",
                    IsSuccess = false,
                };
            }
            role = new AppRole()
            {
                Name = model.RoleName.ToUpper(),
                Description = model.Description

            };
            var result = await _roleManager.CreateAsync(role);
            if (result.Succeeded)
            {
                
                return new MessageReponse()
                {
                    Message = "Create Successed ",
                    IsSuccess = true,
                };
            }
            return new MessageReponse()
            {
                Message = "Create Failed ",
                IsSuccess = false,
            };

        }

        public async Task<MessageReponse> DeleteRole(Guid RoleId)
        {
            var role = await _roleManager.FindByIdAsync(RoleId.ToString());
            if (role == null) throw new Exception($"{RoleId} not found");

            var result = await _roleManager.DeleteAsync(role);
            if (result.Succeeded)
            {
                return new MessageReponse()
                {
                    Message = "Delete Successed",
                    IsSuccess = true
                };
            }
            return new MessageReponse()
            {
                Message = "Delete Failed",
                IsSuccess = true
            };
        }

        public async Task<List<GetAllRoleModel>> GetAllRole(string RoleName)
        {
            if (!string.IsNullOrEmpty(RoleName))
            {
                var roles = _roleManager.Roles.Where(x => x.Name.Contains(RoleName));
                var result = await roles.Select(x => new GetAllRoleModel()
                {
                    Id = x.Id,
                    RoleName = x.Name,
                    Description = x.Description
                }).ToListAsync();
                return result;
            }
            else
            {
                var roles = await _roleManager.Roles.Select(x => new GetAllRoleModel()
                {
                    Id = x.Id,
                    RoleName = x.Name,
                    Description = x.Description
                }).ToListAsync();
                return roles;
            }

        }

        public async Task<GetAllRoleModel> GetRoleById(string RoleId)
        {
            var role = await _roleManager.FindByIdAsync(RoleId.ToString());
            if (role == null) throw new Exception($"Cannot find RoleId: {RoleId}");
            var result = new GetAllRoleModel()
            {
                Id = role.Id,
                Description = role.Description,
                RoleName = role.Name
            };
            return result;



        }

        public async Task<MessageReponse> RemoveUserFromRole(AddToRoleModel model)
        {
            var user = await _userManager.FindByIdAsync(model.UserName);
            if (user == null)
            {
                return new MessageReponse()
                {
                    Message = "UserName not Found",
                    IsSuccess = false,
                };
            }
            else
            {
                var role = await _roleManager.FindByNameAsync(model.RoleName);
                if (role == null)
                {
                    return new MessageReponse()
                    {
                        Message = "RoleName not Found",
                        IsSuccess = false,
                    };
                }
                else
                {
                    var userRoles = await _userManager.GetRolesAsync(user);   
                    bool checkrole = userRoles.ToList().Contains(model.RoleName);
                    if (checkrole)
                    {
                        var result = await _userManager.RemoveFromRoleAsync(user, role.Name);
                        if (result.Succeeded)
                        {
                            return new MessageReponse()
                            {
                                Message = "Remove successed",
                                IsSuccess = true,
                            };
                        }
                        return new MessageReponse()
                        {
                            Message = "Remove Failed",
                            IsSuccess = false,
                        };
                    }
                    return new MessageReponse()
                    {
                        Message = "User not belong to Role",
                        IsSuccess = false,
                    };

                }

            }
        }

        public async Task<MessageReponse> UpdateRole(Guid RoleId, CreateRoleModel model)
        {
            if (await _roleManager.Roles.AnyAsync(x => x.Name == model.RoleName && x.Id != RoleId))
            {
                return new MessageReponse()
                {
                    Message = "RoleName already exists",
                    IsSuccess = false,
                };
            }
            var role = await _roleManager.FindByIdAsync(RoleId.ToString());
            if (role == null)
            {
                return new MessageReponse()
                {
                    Message = "RoldId Not Found",
                    IsSuccess = false,
                };
            }
            if (!string.IsNullOrEmpty(model.RoleName))
            {
                role.Name = model.RoleName;
            }
            if (!string.IsNullOrEmpty(model.Description))
            {
                role.Description = model.Description;
            }
            var result = await _roleManager.UpdateAsync(role);
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
