using System;

using BloggingSiteCMS.DAL.Domain;
//using BloggingSiteCMS.ViewModels;
using BloggingSiteCMS.DTOs;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BloggingSiteCMS.WebAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, ILogger<AccountController> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] RegisterDTO dto)
        {
            if (ModelState.IsValid)
            {
                AppUser user = new AppUser
                {
                    Email = dto.Email,
                    UserName = dto.UserName,
                    PhoneNumber = dto.PhoneNumber,
                    FirstName = dto.FirstName,
                    LastName = dto.LastName
                };

                var result = await _userManager.CreateAsync(user, dto.Password!);

                if (result.Succeeded)
                {
                    return Ok();
                }

                return BadRequest(result.Errors.Select(e => e.Description));
            }

            var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);

            return BadRequest(errors);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginDTO dto, [FromQuery] bool? useCookies, [FromQuery] bool? useSessionCookies)
        {
            if (ModelState.IsValid)
            {
                AppUser? user = await _userManager.FindByEmailAsync(dto.Email!);

                if (user == null)
                {
                    return BadRequest("An account registered to this email address could not be found");
                }

                var isPersistent = (useCookies == true) && (useSessionCookies != true);

                var result = await _signInManager.PasswordSignInAsync(user, dto.Password!, isPersistent, lockoutOnFailure: true);

                if (result.Succeeded)
                {
                    return Ok();
                }

                return BadRequest("Login Failed.");
            }

            var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);

            return BadRequest(errors);
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();

#if DEBUG
            return Ok();
#else
            return Redirect("/");
#endif
        }
    }
}