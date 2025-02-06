//using UserApi.Data;

//namespace UserApi.Services
//{
//    public class NotificationService
//    {
//        private readonly ApiDbContext _context;

//        public NotificationService(ApiDbContext context)
//        {
//            _context = context;
//        }

//        // Method to send a notification and store it in the database
//        public async Task SendNotificationAsync(string userId, string message)
//        {
//            // 1. Save notification to database
//            var notification = new Core.Models.NotificationRequest
//            {
//                UserId = userId,
//                Message = message
//            };

//            _context.NotificationRequests.Add(notification);
//            await _context.SaveChangesAsync();  // Save to the database

//            // 2. Send notification (simulated here as a console message)
//            Console.WriteLine($"Sending notification to user {userId}: {message}");

//            // Optional: Integrate with real services (SMS, email, etc.)
//            // Example: Call some external notification API (like Twilio, SendGrid)
//        }

//        // Method to simulate card-to-card transaction
//        public void ProcessCardTransaction(string userId)
//        {
//            // 1. Simulate transaction processing
//            Console.WriteLine($"Processing card-to-card transaction for user {userId} ");

//            // 2. Simulate success/failure logic (e.g., check balance, transaction status)
//            Console.WriteLine($"Transaction successful for user {userId}");
//        }
//    }
//}
