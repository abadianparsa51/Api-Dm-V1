//using Hangfire;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using UserApi.Core.Models;
//using UserApi.Data;

//namespace UserApi.Controllers
//{
//    public class JobController : ControllerBase
//    {
//        private readonly ApiDbContext _appDbContext;
//        private readonly IBackgroundJobClient _backgroundJobClient;

//        // Inject both the main ApplicationDbContext and IBackgroundJobClient for Hangfire
//        public JobController(ApiDbContext appDbContext, IBackgroundJobClient backgroundJobClient)
//        {
//            _appDbContext = appDbContext;
//            _backgroundJobClient = backgroundJobClient;
//        }

//        [HttpPost]
//        [Route("schedule-job")]
//        public IActionResult ScheduleJob([FromBody] JobRequest jobRequest)
//        {
//            // Validate the request
//            if (jobRequest == null || string.IsNullOrEmpty(jobRequest.UserId))
//            {
//                return BadRequest("Invalid job request.");
//            }

//            // Find the user in the database
//            var user = _appDbContext.Users.FirstOrDefault(u => u.Id == jobRequest.UserId);
//            if (user == null)
//            {
//                return NotFound("User not found.");
//            }

//            try
//            {
//                // Save the JobRequest to the database
//                _appDbContext.JobRequests.Add(jobRequest);
//                _appDbContext.SaveChanges();

//                // Schedule a Hangfire background job
//                var jobId = _backgroundJobClient.Enqueue(() => ProcessJob(jobRequest.UserId, jobRequest.NotifyTime, jobRequest.TransactionTime));

//                return Ok(new { message = "Job scheduled and saved to database.", jobId });
//            }
//            catch (Exception ex)
//            {
//                // Log the error
//                Console.WriteLine($"Error: {ex.Message}");
//                return StatusCode(500, "An error occurred while processing the job.");
//            }
//        }

//        // Background job method
//        public void ProcessJob(string userId, DateTime notifyTime, DateTime transactionTime)
//        {
//            Console.WriteLine($"Processing job for user: {userId}");
//            Console.WriteLine($"Notify at: {notifyTime}, Process transaction at: {transactionTime}");
//        }

//    }
//}
