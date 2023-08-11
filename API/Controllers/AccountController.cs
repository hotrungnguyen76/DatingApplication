using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks.Dataflow;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class AccountController : BaseApiController
    {
        private readonly DataContext _context;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;

        public AccountController(DataContext context, ITokenService tokenService, IMapper mapper)
        {
            _tokenService = tokenService;
            _context = context;
            this._mapper = mapper;
        }

        [HttpPost("Register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {

            if (await UserExists(registerDto)) return BadRequest("User already exists");

            var user = this._mapper.Map<AppUser>(registerDto);

            using var hmac = new HMACSHA512();
    
                user.UserName = registerDto.UserName.ToLower();
                user.HashPassword = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password));
                user.SaltPassword = hmac.Key;
            
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return new UserDto
            {
                UserName = user.UserName,
                Token = _tokenService.GenerateToken(user),
                KnownAs = user.KnownAs,
            };
        }

        private async Task<bool> UserExists(RegisterDto registerDto)
        {
            return await _context.Users.AnyAsync(user => user.UserName == registerDto.UserName.ToLower());
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            var user = await _context.Users
                .Include(p => p.Photos)
                .SingleOrDefaultAsync(x => x.UserName == loginDto.UserName.ToLower());
            if (user == null) return Unauthorized("User does not exist");

            var hmac = new HMACSHA512(user.SaltPassword);

            var computerHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));

            for (int i = 0; i < computerHash.Length; i++)
            {
                if (computerHash[i] != user.HashPassword[i]) return Unauthorized("Invalid password");
            }

            return new UserDto
            {
                UserName = user.UserName,
                Token = _tokenService.GenerateToken(user)
            };

        }
    }
}