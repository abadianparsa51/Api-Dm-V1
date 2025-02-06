namespace UserApi.Core.Models.DTOs
{
    public class WalletDto
    {
        public string WalletId { get; set; }  // شناسه کیف پول
        public string UserId { get; set; }    // شناسه کاربر
        public decimal Balance { get; set; }  // موجودی کیف پول
    }
}
