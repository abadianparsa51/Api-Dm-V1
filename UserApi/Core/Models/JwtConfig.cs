namespace UserApi.Core.Models
{
    public class JwtConfig
    {
        internal double ExpiryInHours;

        public string Secret { get; set; } = "";
        public double ExpiryInDays { get; set; }
    }
}
