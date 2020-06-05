using System.Threading.Tasks;
using Brightsoft.JuneApp.Models;

namespace Brightsoft.JuneApp.Services
{
    public interface IAccountService
    {
        Task<LoginResultModel> LoginAsync(LoginModel model);
        Task<LoginResultModel> RegisterAsync(RegisterModel model);
        Task<bool> ForgotPassword(ForgotPasswordModel model);
        Task<bool> SetPassword(PasswordRecoverModel model);
    }
}
