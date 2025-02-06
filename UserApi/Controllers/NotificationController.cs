//using Microsoft.AspNetCore.Mvc;
//using UserApi.Services;
//using UserApi.Core.Models;

//namespace UserApi.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class NotificationController : ControllerBase
//    {
//        private readonly NotificationService _notificationService;

//        public NotificationController(NotificationService notificationService)
//        {
//            _notificationService = notificationService;
//        }

//        // Endpoint to send a notification
//        [HttpPost("send-notification")]
//        public async Task<IActionResult> SendNotification([FromBody] NotificationRequest notificationRequest)
//        {
//            if (notificationRequest == null)
//            {
//                return BadRequest("Invalid notification data.");
//            }

//            // Call the service to send the notification and store it in the database
//            await _notificationService.SendNotificationAsync(notificationRequest.UserId, notificationRequest.Message);

//            return Ok("Notification sent successfully.");
//        }

//        // Endpoint to process a card transaction
//        [HttpPost("process-card-transaction")]
//        public IActionResult ProcessCardTransaction([FromQuery] string userId)
//        {
//            if (string.IsNullOrEmpty(userId))
//            {
//                return BadRequest("Invalid data for card transaction.");
//            }

//            // Call the service to process the card transaction
//            _notificationService.ProcessCardTransaction(userId);

//            return Ok("Card transaction processed successfully.");
//        }
//    }
//}
