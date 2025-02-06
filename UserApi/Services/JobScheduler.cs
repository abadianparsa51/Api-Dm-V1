//using Hangfire;
//using System;
//using UserApi.Services;

//namespace YourNamespace.Services
//{
//    public class JobScheduler
//    {
//        private readonly IBackgroundJobClient _backgroundJobClient;
//        private readonly NotificationService _notificationService;

//        public JobScheduler(IBackgroundJobClient backgroundJobClient, NotificationService notificationService)
//        {
//            _backgroundJobClient = backgroundJobClient;
//            _notificationService = notificationService;
//        }

//        // این متد برای زمان‌بندی ارسال نوتیفیکیشن و انجام تراکنش کارت به کارت است
//        public void ScheduleJob(string userId, DateTime notifyTime, DateTime transactionTime)
//        {
//            // زمان‌بندی ارسال نوتیفیکیشن
//            _backgroundJobClient.Schedule(() => _notificationService.SendNotificationAsync(userId, "Your scheduled notification message"),
//                notifyTime - DateTime.Now);

//            // زمان‌بندی انجام تراکنش کارت به کارت
//            _backgroundJobClient.Schedule(() => _notificationService.ProcessCardTransaction(userId),
//                transactionTime - DateTime.Now);
//        }
//    }
//}
