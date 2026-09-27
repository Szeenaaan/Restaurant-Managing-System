using RMS.Application.Authentication.DTOs;
using RMS.Application.Helpers;
using RMS.Persistence;
using RMS.Repositories.AuthRepository;
namespace RMS.Services.AuthService
{
    public class AuthService
    {
        private readonly AuthRepository _authRepository;
        private readonly JwtHelper _jwtHelper;

        public AuthService(AuthRepository authRepository, JwtHelper jwtHelper)
        {
            _authRepository = authRepository;
            _jwtHelper = jwtHelper;
        }
        public async Task<LoginResponseDto> LoginAsync(LoginDto dto)
        {
            if (dto.Email == null)
            {
                return new LoginResponseDto { Success = false, StatusCode = 400, Message = "Email is required.", };
            }
            if (dto.Password == null)
            {
                return new LoginResponseDto { Success = false, StatusCode = 400, Message = "Password is required.", };
            }
            var user = await _authRepository.GetByEmailAsync(dto.Email);

            if (user == null)
            {
                return new LoginResponseDto { Success = false, StatusCode = 401, Message = "Invalid email or password" };
            }

            if (!user.IsActive)
            {
                return new LoginResponseDto { Success = false, StatusCode = 403, Message = "This account is does not exist. Please contact support." };
            }
            if (user.IsLocked)
            {
                return new LoginResponseDto { Success = false, StatusCode = 403, Message = "This account is currently blocked. Please contact support." };
            }

            if (!BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
            {
                return new LoginResponseDto { Success = false, StatusCode = 401, Message = "Invalid email or password" };
            }

            var token = _jwtHelper.GenerateToken(user.Id, user.Username);

            return new LoginResponseDto
            {
                Success = true,
                StatusCode = 200,
                Message = "Login successful",
                Token = token
            };
        }

        public async Task<ResponseDto> SignupAsync(CreateUserDto dto, int? userid)
        {
            if (dto.Username == null)
            {
                return new ResponseDto { Success = false, StatusCode = 400, Message = "Username is required." };
            }

            if (dto.Email == null)
            {
                return new ResponseDto { Success = false, StatusCode = 400, Message = "Email is required." };
            }

            if (dto.Password == null)
            {
                return new ResponseDto { Success = false, StatusCode = 400, Message = "Password is required." };
            }

            var existingUser = await _authRepository.GetByEmailAsync(dto.Email);

            if (existingUser != null)
            {
                return new ResponseDto { Success = false, StatusCode = 409, Message = "Email already exists." };
            }

            var existingUsername = await _authRepository.GetByUsernameAsync(dto.Username);

            if (existingUsername != null)
            {
                return new ResponseDto { Success = false, StatusCode = 409, Message = "Username already exists." };
            }

            var user = new User
            {
                Username = dto.Username,
                Email = dto.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                RoleId = dto.RoleId,
                IsActive = true,
                FailedAttempts = 0,
                IsLocked = false,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = userid
            };

            await _authRepository.CreateAsync(user);

            return new ResponseDto { Success = true, StatusCode = 201, Message = "User  Created successfully." };
        }

    }
}

public class LoginResponseDto
{
    public bool Success { get; set; }

    public int StatusCode { get; set; }

    public string? Message { get; set; }

    public string? Token { get; set; }
}

public class CreateUserDto
{
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public int RoleId { get; set; }
}

public class ResponseDto
{
    public bool Success { get; set; }

    public int StatusCode { get; set; }

    public string Message { get; set; } = string.Empty;
}

