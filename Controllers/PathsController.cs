using Microsoft.AspNetCore.Mvc;

namespace aspnet_openapi_examples.Controllers;

[ApiController]
[Route("/paths/{id}")]
public class PathsController : ControllerBase
{
    [HttpGet]
    // get property of pathItem
    public IResult Get([FromRoute] string id)
    {
        return Results.Ok();
    }

    [HttpPut]
    // put property of pathItem
    public IResult Put([FromRoute] string id)
    {
        return Results.Ok();
    }

    [HttpPost]
    // post property of pathItem
    public IResult Post([FromRoute] string id)
    {
        return Results.Ok();
    }

    [HttpDelete]
    // delete property of pathItem
    public IResult Delete([FromRoute] string id)
    {
        return Results.Ok();
    }

    [HttpPatch]
    // patch property of pathItem
    public IResult Patch([FromRoute] string id)
    {
        return Results.Ok();
    }

    [HttpOptions]
    // options property of pathItem
    public IResult Options([FromRoute] string id)
    {
        return Results.Ok();
    }

    [HttpHead]
    // head property of pathItem
    public IResult Head([FromRoute] string id)
    {
        return Results.Ok();
    }

    // trace - use DocumentTransformer

    // parameters - use DocumentTransformer
    // summary - use DocumentTransformer
    // description - use DocumentTransformer
}
