using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// Minimal API apps can be configured to generate problem details response
// for all HTTP client and server error responses that don't have body content
// yet by using the AddProblemDetails extension method.
// ref: https://learn.microsoft.com/aspnet/core/fundamentals/minimal-apis/handle-errors#problem-details
builder.Services.AddProblemDetails();

// ProblemDetailsServiceCollectionExtensions.AddProblemDetails(builder.Services);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

// The Status Code Pages middleware can be configured to produce a common body content, when empty,
// for all HTTP client (400-499) or server (500 -599) responses.
// ref: https://learn.microsoft.com/aspnet/core/fundamentals/minimal-apis/handle-errors#problem-details
app.UseStatusCodePages();

app.UseHttpsRedirection();

app.MapGet("/requires-query-param", (int foo) => "Hello World!");

app.MapGet("/requires-path-param/{id}",
// C# cannot infer the union return type, so we must declare it explicitly
Results<Ok<string>, NotFound<ProblemDetails>> (int id) =>
{
    if (id < 0 || id > 10)
    {
        return TypedResults.NotFound(new ProblemDetails());
    }
    return TypedResults.Ok($"Hello World with id={id}!");
});

app.Run();
