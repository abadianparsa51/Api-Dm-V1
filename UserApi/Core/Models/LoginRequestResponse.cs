namespace UserApi.Core.Models
{
    public class LoginRequestResponse : AuthResult

    {
        public string UserId { get; set; }
        public string Email { get; set; }
    }
}
