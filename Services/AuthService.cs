
using EmployeeManagement.Api.DTOs.Auth;
using EmployeeManagement.Api.Entities;
using EmployeeManagement.Api.Repositories;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace EmployeeManagement.Api.Services
{
    public class JwtSettings
    {
        public string Secret { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public int ExpiryMinutes { get; set; }
    }

    public interface IAuthService
    {
        Task<string> RegisterAsync(RegisterDto dto);
        Task<string> LoginAsync(LoginDto dto);
    }

    public class AuthService : IAuthService
    {
        private readonly IUnitOfWork _uow;
        private readonly JwtSettings _jwtSettings;
        public AuthService(IUnitOfWork uow, IOptions<JwtSettings> opts)
        {
            _uow = uow;
            _jwtSettings = opts.Value;
        }

        public async Task<string> RegisterAsync(RegisterDto dto)
        {
            var exists = (await _uow.Users.FindAsync(u => u.Username == dto.Username || u.Email == dto.Email)).Any();
            if (exists) throw new Exception("User exists");
            var user = new User
            {
                Username = dto.Username,
                Email = dto.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                Role = string.IsNullOrWhiteSpace(dto.Role) ? "User" : dto.Role
            };
            await _uow.Users.AddAsync(user);
            await _uow.SaveChangesAsync();
            return GenerateJwt(user);
        }

        public async Task<string> LoginAsync(LoginDto dto)
        {
            var user = (await _uow.Users.FindAsync(u => u.Username == dto.Username)).FirstOrDefault();
            if (user == null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash)) throw new Exception("Invalid credentials");
            return GenerateJwt(user);
        }

        private string GenerateJwt(User user)
        {
            var claims = new[] { new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()), new Claim(ClaimTypes.Name, user.Username), new Claim(ClaimTypes.Role, user.Role ?? "User") };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Secret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(_jwtSettings.Issuer, _jwtSettings.Audience, claims, expires: DateTime.UtcNow.AddMinutes(_jwtSettings.ExpiryMinutes), signingCredentials: creds);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
