using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.Shop.Data.Entities.Identity;
using Web.Shop.Models;

namespace Web.Shop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;

        public AccountController(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody]RegisterViewModel model)
        {
            var user = new AppUser
            {
                Email=model.Email,
                UserName=model.UserName
            };
            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
                return BadRequest(new { message = result.Errors});

            return Ok(user.Id);
        }
    }
}
