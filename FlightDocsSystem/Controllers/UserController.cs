using FlightDocsSystem.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;
using FlightDocsSystem.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Net;
using AutoMapper;
using FlightDocsSystem.Helpers;
using System.Numerics;
using FlightDocsSystem.Data;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Authorization;
using FlightDocsSystem.Models.Model;
using FlightDocsSystem.Models.ViewModel;
using FlightDocsSystem.Models.EditModel;

namespace FlightDocsSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class UserController : ControllerBase
    {
        readonly int AVATAR_MAX_SIZE = int.MaxValue; // Giá trị tối đa 2MB

        private readonly IUserRepository _userRepository;
        private readonly AppSetting _appSettings;
        private readonly IMapper _mapper;
        private readonly ILogger<UserController> _logger;

        public UserController(IUserRepository userRepository, IOptions<AppSetting> appSettings, IMapper mapper, ILogger<UserController> logger)
        {
            _userRepository = userRepository;
            _appSettings = appSettings.Value;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            try
            {
                var users = await _userRepository.GetAllUsersAsync();
                if (users == null) return NotFound();
                var userVMs = users.Select(x =>
                {
                    var UseVM = _mapper.Map<UserVM>(x);
                    if (x.Avatar != null)
                    {
                        UseVM.Avatar = Convert.ToBase64String(x.Avatar!);
                    }
                    return UseVM;
                });

                _logger.LogInformation("Get all users successfully");
                return Ok(userVMs);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error when getting all users: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "Error when getting all users");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            try
            {
                var user = await _userRepository.GetUserByIdAsync(id);
                if (user == null) return NotFound();

                var userVM = _mapper.Map<UserVM>(user);
                if (user.Avatar != null)
                {
                    userVM.Avatar = Convert.ToBase64String(user.Avatar!);
                }
                _logger.LogInformation($"Get user by Id {id} successfully");
                return Ok(userVM);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error when getting user with id {id}: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error when getting user with Id {id}");
            }
        }

        /*
         *  API tạo mới 1 tài khoản
         *  Email đúng theo định dạng đuôi "@vietjetair.com", không trùng
         *  Tên không trống
         *  Mật khẩu (8-40 kí tự, có chữ in hoa, in thường, số), cần mã hóa (hiện tại chưa có)
         *  
         */
        [HttpPost]
        public async Task<IActionResult> CreateUser(UserModel request)
        {
            try
            {
                //check email, password, phone format
                var checkEmailResult = RegexHelper.CheckEmail(request.Email);
                if (!checkEmailResult.IsMatched)
                {
                    return BadRequest(checkEmailResult.Message);
                }

                var checkPasswordResult = RegexHelper.CheckPassword(request.Password);
                if (!checkPasswordResult.IsMatched)
                {
                    return BadRequest(checkPasswordResult.Message);
                }

                var checkPhoneNumberResult = RegexHelper.CheckPhoneNumber(request.Phone);
                if (!checkPhoneNumberResult.IsMatched)
                {
                    return BadRequest(checkPhoneNumberResult.Message);
                }

                if (request.Name.IsNullOrEmpty())
                {
                    return BadRequest("Name must not be empty");
                }

                if (await _userRepository.IsEmailExistAsync(request.Email))
                {
                    return BadRequest("Email has been duplicated");
                }

                //byte[]? avatarBytes = null;
                //if (request.Avatar != null && request.Avatar.Length > AVATAR_MAX_SIZE)
                //{
                //    using (var ms = new MemoryStream())
                //    {
                //        request.Avatar.CopyTo(ms);
                //        avatarBytes = ms.ToArray();
                //    }
                //}

                string hashPassword = Utils.HashPassword(request.Password);

                var user = new User
                {
                    Name = request.Name,
                    Email = request.Email,
                    Password = hashPassword,
                    Phone = request.Phone,
                    Role = request.Role,
                    CreatedDate = DateTime.Now
                };
                await _userRepository.CreateUserAsync(user);

                _logger.LogInformation($"Create new user with email {request.Email} successfully");
                return Ok("User registered successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error when creating new user: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "Error when creating new user.");

            }

        }

        [HttpPut]
        public async Task<IActionResult> UpdateUser(UserVM request)
        {
            try
            {
                //check email, phone format
                var checkEmailResult = RegexHelper.CheckEmail(request.Email);
                if (!checkEmailResult.IsMatched)
                {
                    return BadRequest(checkEmailResult.Message);
                }

                var checkPhoneNumberResult = RegexHelper.CheckPhoneNumber(request.Phone);
                if (!checkPhoneNumberResult.IsMatched)
                {
                    return BadRequest(checkPhoneNumberResult.Message);
                }

                if (request.Name.IsNullOrEmpty())
                {
                    return BadRequest("Name must not empty");
                }

                //if (await _userRepository.IsEmailExistAsync(request.Email))
                //{
                //    return BadRequest("Email has been duplicated");
                //}

                var user = new User
                {
                    UserId = request.UserId,
                    Name = request.Name,
                    Email = request.Email,
                    Phone = request.Phone,
                };

                await _userRepository.UpdateUserAsync(user);

                _logger.LogInformation($"Update user with id {request.UserId} successfully");
                return Ok($"Update user with id {request.UserId} successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error when updating user with id {request.UserId}: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error when updating user with id {request.UserId}");

            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            try
            {
                await _userRepository.DeleteUserAsync(id);
                _logger.LogInformation($"Delete user with id {id} successfully");
                return Ok($"Delete user with id {id} successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error when deleting user with id {id}: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error when deleting user with id {id}");
            }
        }

        [HttpPost("Login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginModel model)
        {
            try
            {
                var user = await _userRepository.LoginAsync(model);
                if (user == null) //user ko ton tai
                {
                    return Ok(new ApiResponse
                    {
                        Success = false,
                        Message = "Invalid username/password"
                    });
                }

                //cấp token
                var token = GenerateToken(user);
                _logger.LogInformation($"User {model.Email} login successfully");
                return Ok(new ApiResponse
                {
                    Success = true,
                    Message = "Authenticate success",
                    Data = token
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error when checking login information.");

            }
        }

        private TokenModel GenerateToken(User user)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();

            var secretKeyBytes = Encoding.UTF8.GetBytes(_appSettings.SecretKey);

            var tokenDescription = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] {
                    new Claim(ClaimTypes.Name, user.Name),
                    new Claim(JwtRegisteredClaimNames.Email, user.Email),
                    new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim("UserId", user.UserId.ToString()),

                    //roles
                }),
                Expires = DateTime.UtcNow.AddMinutes(5),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(secretKeyBytes), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = jwtTokenHandler.CreateToken(tokenDescription);
            var accessToken = jwtTokenHandler.WriteToken(token);
            //var refreshToken = GenerateRefreshToken();

            //Lưu database
            //var refreshTokenEntity = new RefreshToken
            //{
            //    Id = Guid.NewGuid(),
            //    JwtId = token.Id,
            //    UserId = nguoiDung.Id,
            //    Token = refreshToken,
            //    IsUsed = false,
            //    IsRevoked = false,
            //    IssuedAt = DateTime.UtcNow,
            //    ExpiredAt = DateTime.UtcNow.AddHours(1),
            //};

            //await _context.AddAsync(refreshTokenEntity);
            //await _context.SaveChangesAsync();

            return new TokenModel
            {
                AccessToken = accessToken,
                //RefreshToken = refreshToken
            };
        }

        [HttpPost("ChangeAvatar")]
        public async Task<IActionResult> ChangeAvatar([FromForm] AvatarDTO request)
        {
            try
            {
                byte[]? avatarBytes = null;
                if (request.Avatar != null)
                {
                    if (request.Avatar.Length <= AVATAR_MAX_SIZE)
                    {
                        using (var ms = new MemoryStream())
                        {
                            request.Avatar.CopyTo(ms);
                            avatarBytes = ms.ToArray();
                        }
                    }
                    else
                    {
                        return BadRequest("Avatar size is too big (must be smaller than 2MB)");
                    }
                }

                await _userRepository.ChangeAvatarAsync(request.UserId, avatarBytes);

                _logger.LogInformation($"change avatar of user with id {request.UserId} successfully");
                return Ok("Avatar changed successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error when changing avatar of user with Id {request.UserId}: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error when changing avatar");
            }

        }
    }
}
