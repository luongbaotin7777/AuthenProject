using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AuthenProject.Common;
using AuthenProject.Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AuthenProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SocialController : ControllerBase
    {
        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly ITokenService _tokenService;

        public SocialController(SignInManager<AppUser> signInManager, UserManager<AppUser> userManager, ITokenService tokenService)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _tokenService = tokenService;
       
        }
        
        [AllowAnonymous]
        [HttpGet]
        [Route("/api/google-login")]
        public IActionResult GoogleLogin()
        {
            string redirectUrl = Url.Action("ExternalLoginCallback", "Social");
            var properties = _signInManager.ConfigureExternalAuthenticationProperties("Google", redirectUrl);
            properties.AllowRefresh = true;
            return new ChallengeResult("Google", properties);
        }
        [AllowAnonymous]
        [HttpGet]
        [Route("/api/signin-google")]
        
        public async Task<IActionResult> ExternalLoginCallback()
        {
            ExternalLoginInfo info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null) throw new Exception("Failed get infor");
            //var result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false, bypassTwoFactor: true);
            string[] userInfo = { info.Principal.FindFirst(ClaimTypes.Name).Value, info.Principal.FindFirst(ClaimTypes.Email).Value };
            var token = info.AuthenticationTokens.Single(x => x.Name == "access_token").Value;
            
            var userEmailExists = await _userManager.FindByEmailAsync(info.Principal.FindFirst(ClaimTypes.Email).Value);
            if(userEmailExists == null)
            {
                AppUser appUser = new AppUser()
                {
                    Email = info.Principal.FindFirst(ClaimTypes.Email).Value,
                    UserName = info.Principal.FindFirst(ClaimTypes.Email).Value,
                    FirstName = info.Principal.FindFirst(ClaimTypes.GivenName).Value,
                    LastName = info.Principal.FindFirst(ClaimTypes.Surname).Value,
                    EmailConfirmed = true,
                };
                IdentityResult identityResult = await _userManager.CreateAsync(appUser);
                if (identityResult.Succeeded)
                {
                    identityResult = await _userManager.AddLoginAsync(appUser, info);
                    if (identityResult.Succeeded)
                    {
                        await _signInManager.SignInAsync(appUser, false);
                        return Ok();
                    }
                }
                return BadRequest();
            }
            else
            {
                userEmailExists.Email = info.Principal.FindFirst(ClaimTypes.Email).Value;
                userEmailExists.FirstName = info.Principal.FindFirst(ClaimTypes.GivenName).Value;
                userEmailExists.LastName = info.Principal.FindFirst(ClaimTypes.Surname).Value;
                userEmailExists.UserName = info.Principal.FindFirst(ClaimTypes.Email).Value;
                userEmailExists.EmailConfirmed = true;
                IdentityResult identityResult = await _userManager.UpdateAsync(userEmailExists);
                if (identityResult.Succeeded)
                {
                    identityResult = await _userManager.AddLoginAsync(userEmailExists, info);
                    if (identityResult.Succeeded)
                    {
                        await _signInManager.SignInAsync(userEmailExists, false);
                        return Ok();
                    }
                }
                return BadRequest();

            }
           
        }
        [HttpPost("{UserName}")]
        public async Task<IActionResult> GenToken(string UserName)
        {
            var result =  await _tokenService.GenerateJWTToken(UserName);
            if(result == null)
            {
                return BadRequest();
            }
            return Ok(result);
        }

    }
}
