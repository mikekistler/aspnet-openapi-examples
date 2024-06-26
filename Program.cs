using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapGet("/shapes",
(
    [FromQuery] string type
) =>
{
    // Return a circle, triangle, or square based on the query parameter
    Shape shape = type switch
    {
        "circle" => new Circle { ShapeType = "circle", Radius = 1 },
        "triangle" => new Triangle { ShapeType = "triangle", Hypotenuse = 1 },
        "square" => new Square { ShapeType = "square", Area = 1 },
        _ => null
    };
    return TypedResults.Ok<Shape>(shape);
});

// An endpoint that receives a shape and returns the name of the type of shape
app.MapPost("/shape-type",
(
      Shape shape
) =>
{
    // use reflection to get the type of the shape
    Type shapeType = shape.GetType();
    string shapeTypeName = shapeType.Name;
    return TypedResults.Ok<string>(shapeTypeName);
});

app.Run();
