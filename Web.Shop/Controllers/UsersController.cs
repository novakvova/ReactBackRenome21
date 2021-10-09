using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.Shop.Data;
using Web.Shop.Models;

namespace Web.Shop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly AppEFContext _context;
        private readonly IMapper _mapper;
        public UsersController(AppEFContext context,
            IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var users = await _context.Users
                .Select(x=>_mapper.Map<UserItemViewModel>(x))
                .ToListAsync();
            return Ok(users);
        }
    }
}
