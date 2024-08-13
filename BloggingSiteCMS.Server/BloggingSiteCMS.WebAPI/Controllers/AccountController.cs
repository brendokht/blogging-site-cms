using System;
using System.Text;

using BloggingSiteCMS.DAL.Domain;
using BloggingSiteCMS.ViewModels;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;

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
        public async Task<IActionResult> Register([FromBody] RegisterViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                AppUser user = new AppUser
                {
                    Email = viewModel.Email,
                    UserName = viewModel.UserName,
                    PhoneNumber = viewModel.PhoneNumber,
                    FirstName = viewModel.FirstName,
                    LastName = viewModel.LastName
                };

                var result = await _userManager.CreateAsync(user, viewModel.Password!);

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
        public async Task<IActionResult> Login([FromBody] LoginViewModel viewModel, [FromQuery] bool? useCookies, [FromQuery] bool? useSessionCookies)
        {
            if (ModelState.IsValid)
            {
                AppUser? user = await _userManager.FindByEmailAsync(viewModel.Email!);

                if (user == null)
                {
                    return BadRequest("An account registered to this email address could not be found");
                }

                var isPersistent = (useCookies == true) && (useSessionCookies != true);

                var result = await _signInManager.PasswordSignInAsync(user, viewModel.Password!, isPersistent, lockoutOnFailure: true);

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