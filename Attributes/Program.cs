using System.ComponentModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

app.MapOpenApi();

app.UseHttpsRedirection();

// FromHeader example

app.MapGet("/user-agent", ([FromHeader(Name = "User-Agent")] string userAgent) => userAgent);

// FromQuery example

app.MapGet("/version", ([FromQuery(Name = "api-version")] string apiVersion) => apiVersion);

// FromForm example

app.MapPost("/from-form", ([FromForm] string name, [FromForm] int age) => new { Name = name, Age = age })
    .DisableAntiforgery(); // Antiforgery is enabled by default for form endpoints

// FromRoute example

app.MapGet("/from-route/{id}", ([FromRoute] string id) => id);

// What happens if you pass a name argument that doesn't match the route template, like [FromRoute(Name = "notId")]?

// app.MapGet("/other-route/{id}", ([FromRoute(Name = "notId")] string id) => id);

// The build fails:
// /home/vscode/.nuget/packages/microsoft.extensions.apidescription.server/9.0.0-rc.2.24474.3/build/Microsoft.Extensions.ApiDescription.Server.targets(68,5): error : System.AggregateException: One or more errors occurred. ('notId' is not a route parameter.)

app.MapGet("/pets/{id}", ([FromRoute(Name = "id")] string petId) => petId);

// FromBody example

app.MapPost("/from-body", ([FromBody] Person person) => person);

app.MapPost("/allow-empty-body",
(
    [Description("An optional request body")]
    [FromBody(EmptyBodyBehavior = EmptyBodyBehavior.Allow)] Body body
) =>
{
    return TypedResults.Ok("Good to go - " + (body == null ? "no body" : "body present"));
});

// FromServices example

app.MapGet("/from-services", ([FromServices] IServiceProvider serviceProvider) =>
{
    var service = serviceProvider.GetRequiredService<IServiceProvider>();
    return service;
});

app.Run();

public record Person(string Name, int Age);

internal record Body
{
    public string prop1 { get; set; } = String.Empty;
    public long? prop2 {  get; set; }
    public bool? prop3 {  get; set; }
}