using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

internal static class FormBodiesApi
{
    public static RouteGroupBuilder MapFormBodies(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/form-bodies")
            .WithTags("Form Bodies")
            .DisableAntiforgery(); // Antiforgery is enabled by default for form endpoints

        // Endpoints that define [FromForm] parameters will be generated with both
        // "multipart/form-data" and "application/x-www-form-urlencoded" content entries
        group.MapPost("/from-form",
        (
            [FromForm][Description("Name")][Required][MaxLength(26)][RegularExpression(@"^[A-Za-z0-9-]*$")] string name,
            [FromForm][Description("Age")][Range(0,100)] int age,
            // The property name will be the same as the parameter name with no case-coercion
            [FromForm][Description("Pascal-cased parameter")] int? Pascal,
            // as with the other parameter binding attributes, FromForm supports a Name parameter to specify the form field name
            [FromForm(Name = "other-name")] string? otherName
        ) =>
        {
            // Create a dynamic object to return the values
            return TypedResults.Ok(new { name, age });
        });

        // An endpoint that accepts a file upload will only accept "multipart/form-data"
        group.MapPost("/with-file",
        (
            [FromForm][Description("Name")][Required][MaxLength(26)][RegularExpression(@"^[A-Za-z0-9-]*$")] string name,
            [FromForm][Description("Age")][Range(0,100)] int age,
            [FromForm][Description("Photo")][MaxLength(1024*1024)] FormFile photo
        ) =>
        {
            // Create a dynamic object to return the values
            return TypedResults.Ok(new { name, age, photo.FileName });
        });

        return group;
    }
}
