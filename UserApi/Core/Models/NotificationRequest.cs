namespace UserApi.Core.Models
{
    public class NotificationRequest
    {
        public int Id { get; set; } // Primary key
        public string UserId { get; set; }
        public string Message { get; set; }
    }
}
