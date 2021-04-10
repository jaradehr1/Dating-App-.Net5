using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class AccountController : BaseAPIController
    {
        private readonly DataContext context;
        private readonly ITokenService tokenService;

        public AccountController(DataContext context, ITokenService tokenService)
        {
            this.tokenService = tokenService;
            this.context = context;
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {
            if (await UserExists(registerDto.Username))
                return BadRequest("Useranme is taken");

            using var hmac = new HMACSHA512();
            var user = new AppUser
            {
                UserName = registerDto.Username.ToLower(),
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password)),
                PasswordSalt = hmac.Key
            };

            this.context.Users.Add(user);
            await this.context.SaveChangesAsync();

            return new UserDto {
                Username = user.UserName,
                Token = this.tokenService.CreateToken(user)
            };
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            var user = await this.context.Users
                .Include(p => p.Photos)
                .FirstOrDefaultAsync(u => u.UserName == loginDto.Username.ToLower());
            if (user == null)
                return Unauthorized("Invalid username");

            using var hmac = new HMACSHA512(user.PasswordSalt);

            var computeHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));

            for (int i = 0; i < computeHash.Length; i++)
            {
                if (computeHash[i] != user.PasswordHash[i])
                    return Unauthorized("Invalid password");
            }

            return new UserDto {
                Username = user.UserName,
                Token = this.tokenService.CreateToken(user),
                photoUrl = user.Photos.FirstOrDefault(x => x.IsMain)?.Url
            };
        }

        private async Task<bool> UserExists(string username)
        {
            return await this.context.Users.AnyAsync(u => u.UserName == username.ToLower());
        }
    }
}