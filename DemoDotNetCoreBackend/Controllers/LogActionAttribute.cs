using Microsoft.AspNetCore.Mvc.Filters;

namespace DemoDotNetCoreBackend.Controllers;

public class LogActionAttribute: ActionFilterAttribute
{
    private readonly ILogger<LogActionAttribute> _logger;

    public LogActionAttribute(ILogger<LogActionAttribute> logger)
    {
        this._logger = logger;
    }

    public override void OnActionExecuting(ActionExecutingContext context)
    {
        base.OnActionExecuting(context);

        var actionName = context.ActionDescriptor.DisplayName;
        _logger.LogInformation($"Action {actionName} is being  executedat {DateTime.UtcNow}");
    }

    public override void OnActionExecuted(ActionExecutedContext context)
    {
        base.OnActionExecuted(context);

        var actionName = context.ActionDescriptor.DisplayName;
        _logger.LogInformation($"Action {actionName} was executed at {DateTime.UtcNow}");
    }
}