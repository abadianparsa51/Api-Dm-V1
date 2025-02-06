namespace UserApi.Core.Models
{
    public class JobRequest
    {
        public int Id { get; set; }  // برای شناسایی یونیک هر رکورد در پایگاه داده

        public string UserId { get; set; }

        public DateTime NotifyTime { get; set; }
        public DateTime TransactionTime { get; set; }
    }
}
