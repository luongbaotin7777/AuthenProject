using AuthenProject.Authorization;
using AuthenProject.Common;
using AuthenProject.Dtos;

using AuthenProject.Entities;
using AuthenProject.Repository;
using AuthenProject.Repository.RepositoryBase;
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
        private readonly ApplicationDbContext _context;
        private readonly IRepositoryWrapper _repositoryWrapper;



        public RoleService(RoleManager<AppRole> roleManager, UserManager<AppUser> userManager, ApplicationDbContext context, IRepositoryWrapper repositoryWrapper)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _context = context;
            _repositoryWrapper = repositoryWrapper;

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

        public MessageReponse AddPermission(AddPermission model)
        {
            //var role = await _roleManager.FindByNameAsync(model.RoleName);
            //if (role != null)
            //{


            //    var roleId = role.Id.ToString();
            //    var queryLinq = from rc in _context.RoleClaims
            //                    select new
            //                    {
            //                        V = rc.RoleId.ToString() == roleId,
            //                        V1 = rc.ClaimType == model.ClaimType,
            //                        V2 = rc.ClaimValue == model.ClaimValue
            //                    };

            //    var addPermission = await _context.RoleClaims.AddAsync(queryLinq.First());

            //    if (addPermission != null)
            //    {
            //        await _context.SaveChangesAsync();
            //        return new MessageReponse()
            //        {
            //            Message = "Add Claim Successed",
            //            IsSuccess = true,
            //        };
            //    }
            //    return new MessageReponse()
            //    {
            //        Message = "Add Claim Failed",
            //        IsSuccess = false,
            //    };
            //}
            //return new MessageReponse()
            //{
            //    Message = "Role not found",
            //    IsSuccess = false,
            //};
            return new MessageReponse()
            {
                Message = "UserName not Found",
                IsSuccess = false,
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
            await _repositoryWrapper.Role.CreateAsync(role);
            await _repositoryWrapper.SaveAsync();
            return new MessageReponse()
            {
                Message = "Created Successfully ",
                IsSuccess = false,
            };

        }

        public async Task<MessageReponse> DeleteRole(Guid RoleId)
        {
            var role = await _repositoryWrapper.Role.FindByIdAsync(RoleId);
            if (role == null) throw new Exception($"{RoleId} not found");
            _repositoryWrapper.Role.Delete(role);
            await _repositoryWrapper.SaveAsync();
            return new MessageReponse()
            {
                Message = "Delete Successed",
                IsSuccess = true
            };
        }

        public async Task<List<GetAllRoleModel>> FindRole(string Name)
        {
            var product = _repositoryWrapper.Role.GetbyWhereCondition(x => x.Name.Contains(Name));
            var result = await product.Select(x => new GetAllRoleModel()
            {
                Id = x.Id,
                RoleName = x.Name,
                Description = x.Description
            }).ToListAsync();
            return result;
        }

        public async Task<IEnumerable<AppRole>> GetAllRole()
        {

            var roles = await _repositoryWrapper.Role.GetAllAsync();
            return roles;

        }

        public async Task<GetAllRoleModel> GetRoleById(Guid RoleId)
        {
            var role = await _repositoryWrapper.Role.FindByIdAsync(RoleId);
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

                    if (!checkrole)
                    {
                        return new MessageReponse()
                        {
                            Message = "User is not in role",
                            IsSuccess = false,
                        };
                    }
                    else
                    {
                        var queryLinq = from ur in _context.UserRoles
                                        join au in _context.AppUsers on ur.UserId equals au.Id
                                        join ar in _context.AppRoles on ur.RoleId equals ar.Id
                                        where au.UserName == model.UserName && ar.Name == model.RoleName
                                        select ur;
                        var remove = _context.UserRoles.Remove(queryLinq.First());

                        if (remove != null)
                        {
                            await _context.SaveChangesAsync();
                            return new MessageReponse()
                            {
                                Message = "Delete role successed",
                                IsSuccess = false,
                            };
                        }
                        return new MessageReponse()
                        {
                            Message = "Delete role failed",
                            IsSuccess = false,
                        };
                    }
                }
            }
        }

        public async Task<MessageReponse> UpdateRole(Guid RoleId, CreateRoleModel model)
        {
            var role = await _repositoryWrapper.Role.FindByIdAsync(RoleId);
            if (role == null)
            {
                return new MessageReponse()
                {
                    Message = "RoleId Not Found",
                    IsSuccess = false,
                };
            }

            if (!await _repositoryWrapper.Role.GetByAnyConditionAsync(x => x.Name == model.RoleName && x.Id != RoleId)) 
            {
                if (!string.IsNullOrEmpty(model.RoleName))
                {
                    role.Name = model.RoleName;
                }
                else
                {
                    role.Name = role.Name;
                }
                if (!string.IsNullOrEmpty(model.Description))
                {
                    role.Description = model.Description;
                }
                else
                {
                    role.Description = role.Description;
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

            }
            return new MessageReponse()
            {
                Message = "RoleName already exists",
                IsSuccess = false,
            };
        }
    }
}
