using System.Threading.Tasks;
using JuneApp.Models;

namespace JuneApp.Services
{
    public interface IUserService
    {
        Task<PagedData<UserModel>> GetUsersAsync(GetUsersModel model);
        Task<UserModel> CreateUserAsync(CreateUserModel model);
        Task<UserModel> GetUserAsync(int id);
        Task<bool> UpdateUserAsync(int id, CreateUserModel model);
        Task<bool> DeleteUserAsync(int id, string currentUserName);
    }
}
