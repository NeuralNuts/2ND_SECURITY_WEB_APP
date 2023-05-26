namespace _2ND_SECURITY_WEB_APP.Models
{
    public class UserModel
    {
        public int user_id { get; set; }
        public string? email { get; set; }
        public string? hashPassword { get; set; }
        public string? role { get; set; }
        public string? GUID { get; set; }
    }
}
