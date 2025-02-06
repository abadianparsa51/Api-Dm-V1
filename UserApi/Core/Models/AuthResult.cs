namespace UserApi.Core.Models
{
    public class AuthResult
    {
        public string Token { get; set; } = "";
        public string Email { get; set; } = "";
        public string PhoneNumber { get; set; } = "";
        public bool Result { get; set; }

        public string WalletId { get; set; } // New field for wallet ID
        public decimal WalletBalance { get; set; } // New field for wallet balance
        public List<string>? Errors { get; set; }

    }
}
