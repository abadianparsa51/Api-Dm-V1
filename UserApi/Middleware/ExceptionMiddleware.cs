using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace UserApi.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = 500;
                var value = new
                {
                    Message = "Internal Server Error",
                    Details = ex.Message
                };
                await context.Response.WriteAsync(value.ToString());
            }
        }
    }
}
