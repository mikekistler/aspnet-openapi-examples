using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

internal static class FormBodiesApi
{
    public static RouteGroupBuilder MapFormBodies(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/form-bodies")
            .WithTags("Form Bodies");

        // Endpoints that define [FromForm] parameters will be generated with both
        // "multipart/form-data" and "application/x-www-form-urlencoded" content entries
        group.MapPost("/from-form",
        (
            [FromForm][Description("Name")][Required][MaxLength(26)][RegularExpression(@"^[A-Za-z0-9-]*$")] string Name,
            [FromForm][Description("Age")][Range(0,100)] int Age,
            [FromForm][Description("Photo")][MaxLength(1024*1024)] byte[] photo
        ) =>
        {
            // Create a dynamic object to return the values
            return TypedResults.Ok(new { Name, Age });
        })
        .DisableAntiforgery(); // Anitforgery is enabled by default for form endpoints

        return group;
    }
}
