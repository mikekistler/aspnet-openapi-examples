using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;

namespace ControllerRequests.Controllers;

[ApiController]
public class BodyTypesController : ControllerBase
{
    [HttpPost("string-body")]
    public ActionResult<string> PostString(
        [Description("The request body")] string body
    )
    {
        // Return the body as an application/json response.
        return Ok(body);
    }

    [HttpPost("object-body")]
    public ActionResult<ObjectBody> PostObject(
        [Description("The request body")] ObjectBody body
    )
    {
        // Return the body as an application/json response.
        return Ok(body);
    }

    public class ObjectBody
    {
        public string prop1 { get; set; }
        public string? prop2 { get; set; }
        public string? prop3 { get; set; }
    }

}
