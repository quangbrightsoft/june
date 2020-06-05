using System.Threading.Tasks;
using JuneApp.Models;

namespace JuneApp.Services
{
    public interface IAccountService
    {
        Task<LoginResultModel> LoginAsync(LoginModel model);
        Task<LoginResultModel> RegisterAsync(RegisterModel model);
        Task<bool> ForgotPassword(ForgotPasswordModel model);
        Task<bool> SetPassword(PasswordRecoverModel model);
    }
}
