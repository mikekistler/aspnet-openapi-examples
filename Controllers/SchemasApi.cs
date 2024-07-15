using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace aspnet_openapi_examples.Controllers;

[ApiController]
[Route("/schemas")]
public class SchemasController : ControllerBase
{
    [HttpGet(Name = "GetSchemas")]
    [Produces<Schemas>]
    public Ok<Schemas> Get()
    {
        var Schemas = new Schemas()
        {
            RequiredModifier = 42
        };
        return TypedResults.Ok(Schemas);
    }
}
