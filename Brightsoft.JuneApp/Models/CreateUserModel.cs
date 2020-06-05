namespace JuneApp.Models
{
    public class CreateUserModel
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string[] Roles { get; set; }
    }
}
