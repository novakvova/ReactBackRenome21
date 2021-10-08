using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.Shop.Data.Entities.Identity;
using Web.Shop.Models;
using Web.Shop.Services;

namespace Web.Shop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IJwtTokenService _jwtTokenService;
        private readonly SignInManager<AppUser> _signInManager;

        public AccountController(UserManager<AppUser> userManager,
            IJwtTokenService jwtTokenService,
            SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _jwtTokenService = jwtTokenService;
            _signInManager = signInManager;
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody]RegisterViewModel model)
        {
            ///Зберігаємо фото
            var user = new AppUser
            {
                Email=model.Email,
                UserName=model.UserName,
                Photo="ssfsdf.jpg"
            };
            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
                return BadRequest(new { message = result.Errors});

            await _signInManager.SignInAsync(user, isPersistent: false);

            return Ok(new
            {
                token = _jwtTokenService.CreateToken(user)
            });
        }

        [HttpPost]
        [Route("login")]
        //[Consumes("multipart/form-data")]
        public async Task<IActionResult> Login([FromForm]LoginViewModel model)
        {
            var result = await _signInManager
                .PasswordSignInAsync(model.Email, model.Password, false, false);

            if (!result.Succeeded)
            {
                return BadRequest(new
                {
                    invalid = "Не правильно введені дані!"
                });
            }
            var user = await _userManager.FindByEmailAsync(model.Email);

            return Ok(new
            {
                token = _jwtTokenService.CreateToken(user)
            });
        }
    }
}
