using Microsoft.IdentityModel.Tokens;
using api_zombies.Models;
using api_zombies.Services.Users;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace api_zombies.Services.Authentication
{
    public class AuthService : IAuthService
    {
        private readonly IUsersService _dataAccess;
        private readonly IConfiguration _configuration;

        public AuthService(IUsersService dataAccess, IConfiguration configuration)
        {
            _dataAccess = dataAccess;
            _configuration = configuration;
        }

        public async Task<Usuari?> RegisterAsync(UserDto userDto, bool instructor)
        {
            Usuari user = new Usuari();

            // TODO: Use Automapper
            user.Nom = userDto.Nom;
            user.Email = userDto.Email;
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(userDto.Password);

            Usuari? newUser = await _dataAccess.InsertUserAsync(user);
            if (newUser != null)
            {
                return newUser;
            }
            return null;
        }

        public async Task<Usuari?> LoginAsync(UserDto userDto)
        {
            //Usuari? user = _dataContext.Usuaris.FirstOrDefault(u => u.Email == userDto.Email);
            Usuari? user = await _dataAccess.GetUserAsync(userDto.Email);
            if (user != null)
            {
                string token = CreateToken(user);

                user.Token = token;

                return user;
            }
            return null;

        }

        private string CreateToken(Usuari user)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Nom)
                , new Claim(ClaimTypes.Sid, user.Id.ToString())
                , new Claim(ClaimTypes.Email, user.Email)
                , new Claim(ClaimTypes.Role, "Admin")
            };

            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:Token").Value));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.AddDays(1),
                signingCredentials: credentials
                );
            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
            return jwt;
        }

        public Task LogoutAsync(int userId)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> VerifyPasswordHashAsync(UserDto userDto)
        {
            bool result = await ExistsUserAsync(userDto);
            if (!result)
            {
                return false;
            }

            Usuari user = await _dataAccess.GetUserAsync(userDto.Email);
  
            if (!BCrypt.Net.BCrypt.Verify(userDto.Password, user.PasswordHash))
            {
                return false;
            }

            return true;
        }

        public async Task<bool> ExistsUserAsync(UserDto userDto)
        {
            Usuari? user = await _dataAccess.GetUserAsync(userDto.Email);
            if (user != null)
            {
                return true;
            }
            return false;
        }

    }
}
