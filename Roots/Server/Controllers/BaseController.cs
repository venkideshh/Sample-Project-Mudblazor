using EDC.Server.Data;
using EDC.Server.Extensions;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.Mvc;


namespace EDC.Server.Controllers;

public class BaseController(DefaultDbContext defaultDbContext, ILogger<BaseController> logger)
    : ControllerBase
{
    public DefaultDbContext DefaultDbContext => defaultDbContext;
    public ILogger<BaseController> Logger => logger;

    protected ActionResult Error(Exception ex)
    {
        logger.LogCritical(ex, "Exception Occurred");
        return StatusCode(500, ex.GetAllMessages());
    }
}