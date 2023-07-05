using Microsoft.AspNetCore.Mvc;

namespace Helpdesk.WebApi.Controllers;

[ApiController]
[Route("/")]
public class DefaultController : ControllerBase
{
    [HttpGet]
    public IActionResult Index()
    {
        return Redirect("/docs/api");
    }
}