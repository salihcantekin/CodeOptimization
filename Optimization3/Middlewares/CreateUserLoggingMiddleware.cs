
using Optimization3.Models;
using System.Text;
using System.Text.Json;

namespace Optimization3.Middlewares;

public class CreateUserLoggingMiddleware : IMiddleware
{
    private readonly ILogger<CreateUserLoggingMiddleware> logger;

    public CreateUserLoggingMiddleware(ILogger<CreateUserLoggingMiddleware> logger)
    {
        this.logger = logger;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        if (context.Request.Path.Value.Contains("CreateUser"))
        {
            var req = context.Request;

            req.EnableBuffering();

            using var reader = new StreamReader(req.Body, Encoding.UTF8, true, 1024, true);
            var bodyStr = await reader.ReadToEndAsync();

            var userViewModel = JsonSerializer.Deserialize<UserViewModel>(bodyStr);

            logger.LogInformation($"User {userViewModel.FirstName} {userViewModel.LastName} is created.");

            req.Body.Position = 0;
        }

        await next(context);
    }
}
