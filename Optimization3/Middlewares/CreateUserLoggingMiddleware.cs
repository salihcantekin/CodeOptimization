using Microsoft.AspNetCore.Mvc.Controllers;
using Optimization3.Models;
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
        var controllerActionDescriptor = context
                                            .GetEndpoint()
                                            .Metadata
                                            .GetMetadata<ControllerActionDescriptor>();

        var controllerName = controllerActionDescriptor.ControllerName;
        var actionName = controllerActionDescriptor.ActionName;

        //var controllerName = context.Request.RouteValues["controller"]?.ToString();
        //var actionName = context.Request.RouteValues["action"]?.ToString();

        if (actionName.Equals("createuser", StringComparison.OrdinalIgnoreCase)
            && controllerName.Equals("user", StringComparison.OrdinalIgnoreCase))
        {
            context.Request.EnableBuffering();

            var userViewModel = await JsonSerializer.DeserializeAsync<UserViewModel>(context.Request.Body);

            logger.LogInformation($"User {userViewModel.FirstName} {userViewModel.LastName} is created.");

            context.Request.Body.Position = 0;
            //context.Request.Body.Seek(0, SeekOrigin.Begin);
        }

        await next(context);
    }
}
