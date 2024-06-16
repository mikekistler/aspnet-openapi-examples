using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Http.Metadata;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

// https://learn.microsoft.com/aspnet/core/fundamentals/minimal-apis/openapi#describe-response-types
internal static class ResponsesApi
{
    public static RouteGroupBuilder MapResponses(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/responses")
            .WithTags("Responses");

        // Success response with MIME type
        group.MapGet("/", () =>
        {
            return Results.Ok("Good to go");
        })
        .Produces<string>(200, "text/plain");

        // Multiple response types with Produces extension methods
        group.MapPut("/{id}", (string id) =>
        {
            // Note: Here we return a string -- not a todo --
            // and there is no build error.
            return Results.Ok("Good to go");
        })
        .Produces<Todo>(200)
        .Produces<Todo>(201);

        // Both success and error responses
        group.MapPost("/", () =>
        {
            return Results.Ok("Good to go");
        })
        .Produces<Todo>(200)
        .ProducesProblem(400);

        // Multiple success and error responses
        group.MapDelete("/", () =>
        {
            return Results.Ok("Good to go");
        })
        .Produces<Todo>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status204NoContent)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status404NotFound);

        // Success response with MIME type
        group.MapGet("/with-attributes", [ProducesResponseType<Todo>(200)] () =>
        {
            return Results.Ok("Good to go");
        });

        // Using the TypedResults helper
        group.MapGet("/with-typed-results", () =>
        {
            return TypedResults.Ok(new Todo[0]);
        });

        // Using the TypedResults helper with multiple response types
        group.MapGet("/{id}/with-typed-results", Results<Ok<Todo>, NotFound<ProblemDetails>> (int id) =>
        {
            if (id <= 99)
            {
                return TypedResults.Ok(new Todo { Id = id, Title = $"Todo {id}" });
            }
            // Pass null so that the StatusCodePagesMiddleware will generate a problem details response
            return TypedResults.NotFound<ProblemDetails>(null);
        });

        group.MapGet("/todos/{id}", Results<Ok<Todo>, NotFound<ProblemDetails>> (int id) =>
        {
            if (id <= 99)
            {
                return TypedResults.Ok(new Todo { Id = id, Title = $"Todo {id}" });
            }
            // Pass null so that the StatusCodePagesMiddleware will generate a problem details response
            return TypedResults.NotFound<ProblemDetails>(null);
        })
            .WithName("GetTodo");

        // Using the TypedResults helper
        group.MapPost("/success-typed-results", Results<Ok<Todo>, CreatedAtRoute<Todo>, AcceptedAtRoute<Todo>, NoContent,
            BadRequest<ProblemDetails>, NotFound<ProblemDetails>>
        (
            int v
        ) =>
        {
            var todo = new Todo { Id = 1, Title = "Todo 1" };
            var routeValues = new Dictionary<string, int>
            {
                { "id", todo.Id }
            };
            switch (v)
            {
                case 200:
                    return TypedResults.Ok(todo);
                case 201:
                    return TypedResults.CreatedAtRoute(todo, "GetTodo", routeValues);
                case 202:
                    return TypedResults.AcceptedAtRoute(todo, "GetTodo", routeValues);
                case 204:
                    return TypedResults.NoContent();
                case 400:
                    return TypedResults.BadRequest<ProblemDetails>(null);
                default:
                    return TypedResults.NotFound<ProblemDetails>(null);
            }
        });

        // Using the TypedResults helper
        group.MapPost("/error-typed-results",
            Results<Ok<Todo>, NoContent, BadRequest<ProblemDetails>, NotFound<ProblemDetails>,
            Conflict<ProblemDetails>, UnprocessableEntity<ProblemDetails>>
        (
            int v
        ) =>
        {
            var todo = new Todo { Id = 1, Title = "Todo 1" };
            var routeValues = new Dictionary<string, int>
            {
                { "id", todo.Id }
            };
            switch (v)
            {
                case 200:
                    return TypedResults.Ok(todo);
                case 400:
                    return TypedResults.BadRequest<ProblemDetails>(null);
                case 404:
                    return TypedResults.NotFound<ProblemDetails>(null);
                case 409:
                    return TypedResults.Conflict<ProblemDetails>(null);
                case 422:
                    return TypedResults.UnprocessableEntity<ProblemDetails>(null);
                default:
                    return TypedResults.NotFound<ProblemDetails>(null);
            }
        });

        // When the endpoint method is asynchronous, the return type must have the `Task<>` wrapper.
        group.MapPost("/async-typed-results",
            async Task<Results<Ok<Todo>, NoContent, BadRequest<ProblemDetails>, NotFound<ProblemDetails>>>
        (
            int v
        ) =>
            {
                var todo = new Todo { Id = 1, Title = "Todo 1" };
                var routeValues = new Dictionary<string, int>
            {
                { "id", todo.Id }
            };
                switch (v)
                {
                    case 200:
                        return TypedResults.Ok(todo);
                    case 204:
                        return TypedResults.NoContent();
                    case 400:
                        return TypedResults.BadRequest<ProblemDetails>(null);
                    default:
                        return TypedResults.NotFound<ProblemDetails>(null);
                }
            });

        // Non-Json 200 response
        group.MapGet("/{id}/as-text", Results<ContentHttpResult, NotFound<ProblemDetails>> (int id) =>
        {
            if (id <= 99)
            {
                return TypedResults.Content($"Todo {id}", "text/plain");
            }
            // Pass null so that the StatusCodePagesMiddleware will generate a problem details response
            return TypedResults.NotFound<ProblemDetails>(null);
        });

        // Using the TypedResults helper with multiple response types
        group.MapGet("/{id}/multi-typed-results", Results<FileContentHttpResult, StatusCodeHttpResult> (int id, [FromHeader] string accept) =>
        {
            if (accept.Contains("image/jpeg"))
            {
                // Return JPEG representation of the Todo
                var jpeg = new byte[] { 0xFF, 0xD8, 0xFF, 0xDB, 0x00, 0x84, 0x00, 0x06 };
                return TypedResults.File(jpeg, "image/jpeg");
            }
            else if (accept.Contains("image/png"))
            {
                // Return PNG representation of the Todo
                var png = new byte[] { 0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A };
                return TypedResults.File(png, "image/png");
            }
            // Pass null so that the StatusCodePagesMiddleware will generate a problem details response
            return TypedResults.StatusCode(415);
        });

        return group;
    }
}

class Todo
{
    public int Id { get; set; }

    [Required]
    public string Title { get; set; } = default!;

    public string? Content { get; set; } = default!;

    public DateOnly? DueOn { get; set; }

    public DateOnly? CompletedOn { get; set; }
}
