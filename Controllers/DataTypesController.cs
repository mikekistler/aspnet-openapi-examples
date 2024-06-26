using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace aspnet_openapi_examples.Controllers;

[ApiController]
[Route("/data-types")]
public class DataTypesController : ControllerBase
{
    [HttpGet(Name = "GetDataTypes")]
    [Produces<DataTypes>]
    public Ok<DataTypes> Get()
    {
        var dataTypes = new DataTypes();
        return TypedResults.Ok(dataTypes);
    }
}
