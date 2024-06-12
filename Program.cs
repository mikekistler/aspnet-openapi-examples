using System.Reflection; // For the Assembly class
using System.Text.Json.Serialization;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    // Include the XML comments file
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
    c.EnableAnnotations();

    // Customize generation of DateOnly and TimeOnly types
    c.MapType<DateOnly>(() => new OpenApiSchema { Type = "string", Format = "date" });
    c.MapType<TimeOnly>(() => new OpenApiSchema { Type = "string", Format = "time" });

    // Use a document filter to add a description and contact info
    c.DocumentFilter<InfoFilter>();

    // Add servers to the OpenAPI document
    // Note: the GetSetting trick does not work for build-time OpenAPI generation with Swashbuckle
    var serverUrl = builder.WebHost.GetSetting(WebHostDefaults.ServerUrlsKey) ?? "https://localhost:5001";
    c.AddServer(new OpenApiServer { Url = serverUrl, Description = "Local development server"});
});

builder.Services.ConfigureHttpJsonOptions(options =>
{
    // Don't serialize null values
    options.SerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
});

// Convert exceptions to problem details responses
// ref: https://learn.microsoft.com/en-us/aspnet/core/fundamentals/error-handling#problem-details
builder.Services.AddProblemDetails();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
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
