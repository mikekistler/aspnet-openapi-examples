using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;

namespace aspnet_openapi_examples.Controllers;

[ApiController]
[Route("/request-bodies")]
public class RequestBodiesController : ControllerBase
{
    [Consumes("application/json")]
    [HttpPost("json-body")]
    public IResult JsonBody(
        [Description("A Json request body")] [FromBody] JsonBody body
    )
    {
        return Results.Ok("Good to go");
    }

    [Consumes("application/json")]
    [HttpPost("optional-body")]
    public IResult OptionalBody(
        [Description("An optional Json request body")] [FromBody] JsonBody? body
    )
    {
        return Results.Ok("Good to go");
    }

    // For non-json request bodies, the route handler can read and parse the request body directly
    // from the HttpContext. In this case, use the Consumes attribute to specify the allowed
    // content types and the expected type of the request body.
    [HttpPost("consumes")]
    [Consumes("image/jpeg", "image/png", "image/tiff")]
    public IResult Consumes()
    {
        return TypedResults.Ok("Good to go - " + Request.ContentType);
    }

    [Consumes("text/plain")]
    [HttpPost("text-plain")]
    public IResult NonJsonBody(
        [Description("A non-Json request body")] [FromBody] string body
    )
    {
        return Results.Ok("Good to go: " + body);
    }

    [Consumes("image/jpeg", "image/png", "image/tiff")]
    [HttpPost("multi-content-type")]
    public IResult OptionalNonJsonBody(
        [Description("An optional non-Json request body")] [FromBody] byte[]? body
    )
    {
        return Results.Ok("Good to go");
    }

    [HttpPost("form-data")]
    public IResult FormData(
        [Description("A form data request body")] [FromForm] JsonBody body
    )
    {
        return Results.Ok("Good to go");
    }
}

public record JsonBody
{
    public string Prop1 { get; set; }
    public long? Prop2 { get; set; }
    public bool? Prop3 { get; set; }
}