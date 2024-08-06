using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace aspnet_openapi_examples.Controllers;

[ApiController]
[Route("/form-bodies")]
public class FormBodiesController : ControllerBase
{
    [HttpPost("from-form")]
    public IResult FromForm(
        [FromForm][Description("Name")][Required][MaxLength(26)][RegularExpression(@"^[A-Za-z. ]*$")] string name,
        [FromForm][Description("Age")][Range(0,100)] int age,
        // The property name will be the same as the parameter name with no case-coercion
        [FromForm][Description("Pascal-cased parameter")] int? Pascal,
        // as with the other parameter binding attributes, FromForm supports a Name parameter to specify the form field name
        [FromForm(Name = "other-name")] string? otherName
    )
    {
        return Results.Ok("Good to go");
    }

}
