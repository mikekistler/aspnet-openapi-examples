using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;

namespace RequestBodies.Controllers;

[ApiController]
[Route("/request-bodies")]
public class RequestBodiesController : ControllerBase
{
    // In a controller-based API app, an endpoint without a Consumes filter will accept any content type
    // for which an available input formatter handles the type of the FromBody parameter.
    // In this case, the requestBody in the generated OpenAPI document will contain all the content types
    // for input formatters that can handle the FromBody parameter type.
    // Note that in this example, "text/plain" is not one of the content types shown in the requestBody
    // in the OpenAPI document because it only supports request bodies of type string.
    [ProducesResponseType<string>(200, "application/json")]
    [HttpPost("no-consumes")]
    public ActionResult<Body> NoConsumes(
        [Description("The request body")]Body body  // Implicitly FromBody
    )
    {
        // Return the body as an application/json response.
        return Ok(body);
    }

    // You can use a [Consumes] filter, which is configured with the `[Consumes]` attributre, to restrict the content
    // types that a route handler will accept.
    // Curiously, when "application/json" is specified in the Consumes filter, the endpoint will also accept
    // "application/*+json" content types, and this is reflected in the requestBody in the OpenAPI document.
    [Consumes("application/json")]
    [ProducesResponseType<string>(200, "application/json")]
    [HttpPost("consumes-json")]
    public ActionResult<Body> ConsumesJson(
        Body body  // Implicitly FromBody
    )
    {
        // Return the body as an application/json response.
        return Ok(body);
    }

    // It is important to understand that Consumes filters cannot add support for a content type
    // that is not already supported by an input formatter. In this scenario, the requestBody in the
    // generated OpenAPI document will claim to support "application/json" and _not_ the content type
    // specified in the Consumes filter. However, a request with either content type will fail
    // at runtime with a 415 Unsupported Media Type response.
    [Consumes("applicaiton/pdf")]
    [ProducesResponseType<string>(200, "application/json")]
    [HttpPost("bad-consumes")]
    public ActionResult<Body> BadConsumes(
        Body body  // Implicitly FromBody
    )
    {
        // Return the body as an application/json response.
        return Ok(body);
    }

    // When the XML input formatter is enabled, you can use the Consumes attribute to restrict the
    // content types that the endpoint will accept it just XML.
    // Curiously, when "application/xml" is specified in the Consumes filter, the endpoint will also accept
    // "application/*+xml" content types, and this is reflected in the requestBody in the OpenAPI document.
    [Consumes("application/xml")]
    [ProducesResponseType<string>(200, "application/json")]
    [HttpPost("xml-body")]
    public ActionResult<XmlBody> Get(
        [Description("An Xml request body")] XmlBody body  // Implicitly FromBody
    )
    {
        // Return the body as an application/json response.
        return Ok(body);
    }

    // Note that the JSON and XML input formatters can handle the string type, so you must specify
    // a Consumes filter to restrict the content type to "text/plain" if that is desired.
    [Consumes("text/plain")]
    [ProducesResponseType<string>(200, "application/json")]
    [HttpPost("text-body")]
    public ActionResult<string> Post([FromBody] string body)
    {
        // Return an application/json response.
        return Ok("Good to go: " + body);
    }

    // Use the Consumes attribute to specify the content type of the request.
    // By default, the request body may be any content type for which an input formatter is available.
    // If you specify a content type for which no input formatter is available, the content type
    // will be shown in the OpenAPI but the request will fail at runtime with a 415 Unsupported Media Type response.
    [Consumes("application/octet-stream")]
    [ProducesResponseType<string>(200, "application/json")]
    [HttpPost("stream-body")]
    public async Task<IActionResult> Stream([FromBody] Stream body)
    {
        using var reader = new StreamReader(body);
        char[] buffer = new char[1024];
        int bytesRead, totalBytesRead = 0;
        while ((bytesRead = await reader.ReadAsync(buffer, 0, buffer.Length)) > 0)
        {
            // Process the chunk
            totalBytesRead += bytesRead;
        }
        return Ok($"Processed stream of {totalBytesRead} bytes");
    }

    // If the route handler does not have a [FromBody] or [FromForm] parameter, the route handler may read
    // the request body directly from the `Request.Body` stream and may use the Consumes attribute to
    // restrict the content types allowed, but no requestBody is generated in the OpenAPI document.
    [Consumes(typeof(byte[]), "application/octet-stream")]  // Not shown in generated OpenAPI
    [ProducesResponseType<string>(200, "application/json")]
    [HttpPost("from-context")]
    public async Task<IActionResult> BodyFromContext()
    {
        using var reader = new StreamReader(Request.Body);
        var content = await reader.ReadToEndAsync();
        // Process the streamed content
        return Ok($"Received content: {content}");
    }
}

public class Body
{
    public string prop1 { get; set; }
    public string? prop2 { get; set; }
    public string? prop3 { get; set; }
}
