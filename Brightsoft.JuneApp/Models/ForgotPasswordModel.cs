using System.ComponentModel.DataAnnotations;

namespace JuneApp.Models
{
    public class ForgotPasswordModel
    {
        [Required]
        public string Email {get; set; }
    }
}