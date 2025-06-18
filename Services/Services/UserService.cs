using AutoMapper;
using Models;
using Models.Dtos.User;
using Models.Requests;
using Models.Responses;
using Repository.Interfaces;
using Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services
{
    public class UserService(IUnitOfWork unitOfWork, IMapper mapper, ITokensService tokenService) : IUserService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;
        private readonly ITokensService _tokenService = tokenService;

        public async Task<UserDto?> GetUserByIdAsync(int id)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(id);
            return user != null ? _mapper.Map<UserDto>(user) : null;
        }

        public async Task<IEnumerable<UserDto>> GetAllUsersAsync()
        {
            var users = await _unitOfWork.Users.GetAllAsync();
            return _mapper.Map<IEnumerable<UserDto>>(users);
        }

        public async Task<UserDto> CreateUserAsync(CreateUserDto createUserDto)
        {
            // Validar que el username no exista
            if (await _unitOfWork.Users.IsUsernameExistsAsync(createUserDto.Username))
            {
                throw new InvalidOperationException($"Username '{createUserDto.Username}' already exists.");
            }

            // Validar que el email no exista
            if (await _unitOfWork.Users.IsEmailExistsAsync(createUserDto.Email))
            {
                throw new InvalidOperationException($"Email '{createUserDto.Email}' already exists.");
            }

            var user = _mapper.Map<User>(createUserDto);
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(createUserDto.Password);
            user.CreatedBy = "System"; // En una implementación real, obtendrías esto del contexto del usuario actual

            await _unitOfWork.Users.AddAsync(user);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<UserDto>(user);
        }

        public async Task<UserDto> UpdateUserAsync(int id, UpdateUserDto updateUserDto)
        {
            var existingUser = await _unitOfWork.Users.GetByIdAsync(id);
            if (existingUser == null)
            {
                throw new KeyNotFoundException($"User with ID {id} not found.");
            }

            // Validar que el email no exista en otro usuario
            if (await _unitOfWork.Users.IsEmailExistsAsync(updateUserDto.Email, id))
            {
                throw new InvalidOperationException($"Email '{updateUserDto.Email}' already exists.");
            }

            _mapper.Map(updateUserDto, existingUser);
            existingUser.ModifiedBy = "System"; // En una implementación real, obtendrías esto del contexto del usuario actual

            await _unitOfWork.Users.UpdateAsync(existingUser);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<UserDto>(existingUser);
        }

        public async Task DeleteUserAsync(int id)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(id) ?? throw new KeyNotFoundException($"User with ID {id} not found.");
            await _unitOfWork.Users.DeleteAsync(user);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<LoginResponseDto?> LoginAsync(LoginDto loginDto)
        {
            var user = await _unitOfWork.Users.GetByUsernameAsync(loginDto.UsernameOrEmail);

            if (user == null || !BCrypt.Net.BCrypt.Verify(loginDto.Password, user.PasswordHash))
            {
                return null;
            }

            // Actualizar última fecha de login
            await _unitOfWork.Users.UpdateLastLoginAsync(user.Id);
            await _unitOfWork.SaveChangesAsync();

            var token = _tokenService.GenerateJwtToken(user);
            var expiresAt = _tokenService.GetTokenExpiration(token);

            return new LoginResponseDto
            {
                Token = token,
                User = _mapper.Map<UserDto>(user),
                ExpiresAt = expiresAt
            };
        }

        public async Task<bool> IsUsernameAvailableAsync(string username, int? excludeUserId = null)
        {
            return !await _unitOfWork.Users.IsUsernameExistsAsync(username, excludeUserId);
        }

        public async Task<bool> IsEmailAvailableAsync(string email, int? excludeUserId = null)
        {
            return !await _unitOfWork.Users.IsEmailExistsAsync(email, excludeUserId);
        }

        public async Task<IEnumerable<UserDto>> SearchUsersAsync(string searchTerm)
        {
            var users = await _unitOfWork.Users.SearchUsersAsync(searchTerm);
            return _mapper.Map<IEnumerable<UserDto>>(users);
        }

        public async Task<IEnumerable<UserDto>> GetPagedUsersAsync(int pageNumber, int pageSize)
        {
            var users = await _unitOfWork.Users.GetPagedAsync(pageNumber, pageSize);
            return _mapper.Map<IEnumerable<UserDto>>(users);
        }
    }
}

