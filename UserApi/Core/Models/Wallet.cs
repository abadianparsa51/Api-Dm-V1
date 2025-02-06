namespace UserApi.Core.Models
{
    public class Wallet
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();  // شناسه یکتا برای کیف پول
        public decimal Balance { get; set; } = 0; // موجودی اولیه کیف پول

        public string UserId { get; set; }  // ارتباط یک به یک با کاربر
        public ApplicationUser User { get; set; }
    }
}
