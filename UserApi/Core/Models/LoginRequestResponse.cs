namespace UserApi.Core.Models
{
    public class LoginRequestResponse : AuthResult
    {
        public string UserId { get; set; }
        public string Email { get; set; }
        public string Token { get; set; }
        public string PhoneNumber { get; set; }
        public string WalletId { get; set; }
        public decimal WalletBalance { get; set; }
    }
}
