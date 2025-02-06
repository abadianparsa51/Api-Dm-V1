namespace UserApi.Helper;
using Microsoft.AspNetCore.SignalR;
    public class NotificationHub : Hub  
    {
        public async Task SendOtp(string otp)
        {
            await Clients.All.SendAsync("ReceiveOtp", otp);
        }
    }

