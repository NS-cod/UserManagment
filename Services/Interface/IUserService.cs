using Models.Dtos.User;
using Models.Requests;
using Models.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interface
{
    public interface IUserService
    {
        Task<UserDto?> GetUserByIdAsync(int id);
        Task<IEnumerable<UserDto>> GetAllUsersAsync();
        Task<UserDto> CreateUserAsync(CreateUserDto createUserDto);
        Task<UserDto> UpdateUserAsync(int id, UpdateUserDto updateUserDto);
        Task DeleteUserAsync(int id);
        Task<LoginResponseDto?> LoginAsync(LoginDto loginDto);
        Task<bool> IsUsernameAvailableAsync(string username, int? excludeUserId = null);
        Task<bool> IsEmailAvailableAsync(string email, int? excludeUserId = null);
        Task<IEnumerable<UserDto>> SearchUsersAsync(string searchTerm);
        Task<IEnumerable<UserDto>> GetPagedUsersAsync(int pageNumber, int pageSize);
    }
}
