using BCrypt.Net;
using EPM_Blogger.Application.DTOs.Authentication;
using EPM_Blogger.Application.DTOs.Users;
using EPM_Blogger.Application.Interfaces;
using EPM_Blogger.Domain.Interfaces;
using EPM_Blogger.Domains.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace EPM_Blogger.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IAuthRepository _userRepository;

        private readonly IConfiguration _config;

        public AuthService(IAuthRepository userRepository, IConfiguration config)
        {
            _userRepository = userRepository;
            _config = config;
        }


        public async Task<UserDto> CreateUserAsync(RegisterUserDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Username))
                throw new ArgumentException("Username is required.");
            if (string.IsNullOrWhiteSpace(dto.Email) || !IsValidEmail(dto.Email))
                throw new ArgumentException("Invalid email format.");
            if (string.IsNullOrWhiteSpace(dto.Password) || dto.Password.Length < 6)
                throw new ArgumentException("Password must be at least 6 characters long.");

            if (await _userRepository.GetByEmailAsync(dto.Email) != null)
                throw new InvalidOperationException("Email already exists.");
            if (await _userRepository.GetByUsernameAsync(dto.Username) != null)
                throw new InvalidOperationException("Username already exists.");

            var passwordHash = HashPassword(dto.Password);
            // Username is not case-sensitive
            var user = new User
            {
                Username = dto.Username.Trim().ToLower(),
                Email = dto.Email.Trim(),
                PasswordHash = passwordHash
            };

            var createdUser = await _userRepository.AddAsync(user);

            return new UserDto
            {
                UserId = createdUser.UserId,
                Username = createdUser.Username,
                Email = createdUser.Email,
                CreatedAt = createdUser.CreatedAt.GetValueOrDefault(),
            };
        }

        private string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        public async Task<AuthResponseDto> LoginAsync(LoginDto dto)
        {
            var user = await _userRepository.GetByEmailAsync(dto.UsernameOrEmail)
                       ?? await _userRepository.GetByUsernameAsync(dto.UsernameOrEmail);

            if (user == null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
                throw new UnauthorizedAccessException("Invalid credentials.");

            var userDto = new UserDto
            {
                UserId = user.UserId,
                Username = user.Username,
                Email = user.Email,
                CreatedAt = user.CreatedAt.GetValueOrDefault(),
            };

            var token = GenerateJwtToken(userDto);
            var refreshToken = GenerateRefreshToken();

            return new AuthResponseDto
            {
                User = userDto,
                AccessToken = token,
                RefreshToken = refreshToken,
                ExpiresAt = DateTime.UtcNow.AddMinutes(30)
            };
        }

        private string GenerateJwtToken(UserDto user)
        {
            var claims = new[]
            {
            new Claim(JwtRegisteredClaimNames.Sub, user.UserId.ToString()),
            new Claim(JwtRegisteredClaimNames.UniqueName, user.Username),
            new Claim(JwtRegisteredClaimNames.Email, user.Email)
        };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]??""));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(30),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private string GenerateRefreshToken()
        {
            return Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
        }
    }
}
