using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;

namespace aspnet_openapi_examples.Controllers;

[ApiController]
[Route("/request-bodies")]
public class RequestBodiesController : ControllerBase
{
    //[Consumes(typeof(JsonBody), "application/json")]
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

    [Consumes("application/octet-stream")]
    [HttpPost("non-json-body")]
    public IResult NonJsonBody(
        [Description("A non-Json request body")] [FromBody] byte[] body
    )
    {
        return Results.Ok("Good to go");
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