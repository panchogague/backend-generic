using AutoMapper;
using Chanchas.DTOs;
using Chanchas.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Chanchas.Controllers
{
    public class AccountController : BaseApiController
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IMapper _mapper;
        private readonly ITokenService _tokenService;

        public AccountController(UserManager<IdentityUser> userManager, IMapper mapper, ITokenService tokenService)
        {
            _userManager = userManager;
            _mapper = mapper;
            _tokenService = tokenService;
        }

        //[HttpPost("login")]
        //public async Task<ActionResult> Login()
        //{

        //}

        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {
            //if (await ExistUser(registerDto.Email)) return BadRequest("Email already exist");

            var user = _mapper.Map<IdentityUser>(registerDto);

            var result = await _userManager.CreateAsync(user,registerDto.Password);

            if (!result.Succeeded) return BadRequest(result.Errors);

            var roleResult = await _userManager.AddToRoleAsync(user, "Member");

            if (!roleResult.Succeeded) return BadRequest(roleResult.Errors);

            return Ok(new UserDto 
            { 
                Email = registerDto.Email, 
                UserName = registerDto.UserName, 
                Token = await _tokenService.CreateToken(user)
            });

        }

        private async Task<bool> ExistUser(string email)
        {
            return await _userManager.Users.AnyAsync(u=> u.NormalizedEmail == email.ToUpper());
        }
    }
}
