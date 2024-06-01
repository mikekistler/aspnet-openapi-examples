using System.Text.Json.Serialization;

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
});

var app = builder.Build();

app.UseHttpsRedirection();

app.MapDataTypes();
app.MapSchemas();

app.Run();
