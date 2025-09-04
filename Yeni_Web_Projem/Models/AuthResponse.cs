namespace Yeni_Web_Projem.Models
{
    public class AuthResponse
    {
        public bool isSuccess { get; set; }
        public string Message { get; set; }
        public UserInfo User { get; set; }
    }

    public class UserInfo
    {
        public string Email { get; set; }
    }
}