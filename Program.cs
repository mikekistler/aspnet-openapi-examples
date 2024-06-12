using System.Text.Json.Serialization;
using Microsoft.OpenApi.Models;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.ConfigureHttpJsonOptions(options =>
{
    // Don't serialize null values
    options.SerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
});

// Convert exceptions to problem details responses
// ref: https://learn.microsoft.com/en-us/aspnet/core/fundamentals/error-handling#problem-details
builder.Services.AddProblemDetails();


builder.Services.AddOpenApi(options => {
    options.UseTransformer<InfoTransformer>();
    options.UseTransformer((document, context, cancellationToken) => 
    {
        var serverUrl = builder.WebHost.GetSetting(WebHostDefaults.ServerUrlsKey);
        document.Servers = new List<OpenApiServer>
        {
            new OpenApiServer
            {
                Url = serverUrl,
                Description = "Local development server"
            }
        };
        return Task.CompletedTask;
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();
app.UseStatusCodePages();

app.MapDataTypes();
app.MapSchemas();
app.MapPaths();
app.MapOperations();
app.MapRequestBodies();
app.MapResponses();

app.Run();
