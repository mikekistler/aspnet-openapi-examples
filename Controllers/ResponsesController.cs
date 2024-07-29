using Microsoft.AspNetCore.Mvc;

namespace aspnet_openapi_examples.Controllers;

[ApiController]
[Route("/responses")]
public class ResponsesController : ControllerBase
{
    // Success response with MIME type
    [Produces("text/plain")]
    [ProducesResponseType<string>(StatusCodes.Status200OK)]
    [HttpGet("")]
    public ActionResult<string> GetSuccess()
    {
        var result = new ActionResult<string>("Good to go");
        return result;
    }

    // Multiple response types with Produces extension methods
    [Produces("application/json")]
    [HttpGet("multiple")]
    public IActionResult GetMultiple()
    {
        // randomly return 200 or 201
        if (new Random().Next(0, 2) == 0)
        {
            return Ok("Good to go");
        }
        else
        {
            return Created("/multiple", "Created");
        }
    }
}
