using System.ComponentModel.DataAnnotations;

namespace Brightsoft.JuneApp.Models
{
    public class PasswordRecoverModel
    {
        [Required]
        public string Email { get; set; }
        public string Token { get; set; }
        public string NewPassword { get; set; }
    }
}