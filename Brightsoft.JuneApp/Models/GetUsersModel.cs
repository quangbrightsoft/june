namespace Brightsoft.JuneApp.Models
{
    public class GetUsersModel
    {
        public string Search { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
        public string SortBy { get; set; }
        public bool Descending { get; set; }
        public string[] Roles { get; set; }
    }
}