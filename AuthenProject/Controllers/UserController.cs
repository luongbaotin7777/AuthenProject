using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuthenProject.Authorization;
using AuthenProject.Dtos;
using AuthenProject.Service.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AuthenProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }
        //Post api/user/register
        [HttpPost("Register")]
        [AllowAnonymous]
        //[Authorize(Permission.Users.Create)]
        public async Task<IActionResult> RegisterUser(RegisterUserModel model)
        {
            var user = await _userService.RegisterUSer(model);
            if(user == null)
            {
                return BadRequest(user);
            }
            return Ok(user);
        }
        //Post api/user/login
        [HttpPost("Login")]
        [AllowAnonymous]
        public async Task<IActionResult> LoginUSer(LoginUserModel model)
        {
            var user = await _userService.LoginUser(model);
            if (user == null)
            {
                return BadRequest(user);
            }
            return Ok(user);
        }
        //Get api/user/getall
        [HttpGet("Getall")]
        //[Authorize(Permission.Users.View)]
        public async Task<IActionResult> GetAllUser()
        {
            var users = await _userService.GetAllUser();
            if (users == null)
            {
                return BadRequest(users);
            }
            return Ok(users);
        }
        //Get api/user/userid
        [HttpGet("{UserId}")]
        //[Authorize(Permission.Users.View)]
        public async Task<IActionResult> GetUserById (Guid UserId)
        {
            var user = await _userService.GetUserById(UserId);
            if (user == null)
            {
                return BadRequest(user);
            }
            return Ok(user);
        }
        //Delete api/user/userid
        [HttpDelete("{UserId}")]
        //[Authorize(Permission.Users.Delete)]
        public async Task<IActionResult> DeleteUser(Guid UserId)
        {
            var user = await _userService.DeleteUser(UserId);
            if (user == null)
            {
                return BadRequest(user);
            }
            return Ok(user);
        }
        //Update api/user/userid
        [HttpPut("{UserId}")]
        //[Authorize(Permission.Users.Edit)]
        public async Task<IActionResult> UpdateUser(Guid UserId,UpdateUserModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var user = await _userService.UpdateUser(UserId, model);
            if (user == null)
            {
                return BadRequest(user);
            }
            return Ok(user);
        }
        [HttpPut("ChangePassword")]
        public async Task<IActionResult> ChangePassword(string UserName,string currentPassword,string newPassword,string passwordConfirm)
        {
            var password = await _userService.ChangePassword(UserName, currentPassword, newPassword, passwordConfirm);
            if (password == null)
            {
                return BadRequest(password);
            }
            return Ok(password);

        }
    }
}
