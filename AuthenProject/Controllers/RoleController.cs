using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuthenProject.Dtos;
using AuthenProject.Service.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AuthenProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize(Roles = "ADMIN")]
    public class RoleController : ControllerBase
    {
        private readonly IRoleService _roleService;
        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }
        //Post api/role/create
        [HttpPost("Create")]

        public async Task<IActionResult> CreateRole(CreateRoleModel model)
        {
            var role = await _roleService.CreateRole(model);
            if(role == null)
            {
                return BadRequest(role);
            }
            return Ok(role);
        }
        //Get api/role/getall
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAllRole()
        {
            var roles = await _roleService.GetAllRole();
            if(roles == null)
            {
                return BadRequest(roles);
            }
            return Ok(roles);
        }
        //Get api/role/roleId
        [HttpGet("{RoleId}")]
        public async Task<IActionResult> GetRoleById(Guid RoleId)
        {
            var roles = await _roleService.GetRoleById(RoleId);
            if (roles == null)
            {
                return BadRequest(roles);
            }
            return Ok(roles);
        }
        //Post api/role/addusertorole
        [HttpPost("AddUserToRole")]
        public async Task<IActionResult> AddUserToRole(AddToRoleModel model)
        {
            var userToRole = await _roleService.AddUserToRole(model);
            if(userToRole == null)
            {
                return BadRequest(userToRole);
            }
            return Ok(userToRole);
        }
        //Post api/role/removeuserfromrole
        [HttpPost("RemoveUserFromRole")]
        public async Task<IActionResult> RemoveUserFromRole(AddToRoleModel model)
        {
            var remove = await _roleService.RemoveUserFromRole(model);
            if (remove == null)
            {
                return BadRequest(remove);
            }
            return Ok(remove);
        }
        //Put api/role/roleId
        [HttpPut("{RoleId}")]
        public async Task<IActionResult> UpdateRole(Guid RoleId,CreateRoleModel model)
        {
            var role = await _roleService.UpdateRole(RoleId, model);
            if (role == null)
            {
                return BadRequest(role);
            }
            return Ok(role);
        }
        //Delete api/role/roleId
        [HttpDelete("{RoleId}")]
        public async Task<IActionResult> DeleteRole(Guid RoleId)
        {
            var role = await _roleService.DeleteRole(RoleId);
            if (role == null)
            {
                return BadRequest(role);
            }
            return Ok(role);
        }
        //Post api/role/RoleName
        [HttpPost("{RoleName}")]
        public async Task<IActionResult> AddClaimToRole(string RoleName)
        {
            var roleClaim = await _roleService.AddClaimToRole(RoleName);
            if (roleClaim == null)
            {
                return BadRequest(roleClaim);
            }
            return Ok(roleClaim);
        }
        //Post api/role/addpermission
        //[HttpPost("AddPermission")]
        //public async Task<IActionResult> AddPermission(AddPermission model)
        //{
        //    var permission = await _roleService.AddPermission(model);
        //    if (permission == null)
        //    {
        //        return BadRequest(permission);
        //    }
        //    return Ok(permission);
        //}
    }
}
