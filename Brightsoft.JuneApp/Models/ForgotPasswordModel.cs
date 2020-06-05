using System.ComponentModel.DataAnnotations;

namespace Brightsoft.JuneApp.Models
{
    public class ForgotPasswordModel
    {
        [Required]
        public string Email {get; set; }
    }
}