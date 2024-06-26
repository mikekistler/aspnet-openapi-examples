using System.Text.Json.Serialization;
using Swashbuckle.AspNetCore.Annotations;

[SwaggerDiscriminator("shapeType")]
[SwaggerSubType(typeof(Triangle), DiscriminatorValue = "triangle")]
[SwaggerSubType(typeof(Square), DiscriminatorValue = "square")]
[SwaggerSubType(typeof(Circle), DiscriminatorValue = "circle")]
[JsonPolymorphic(TypeDiscriminatorPropertyName = "shapeType")]
[JsonDerivedType(typeof(Triangle), typeDiscriminator: "triangle")]
[JsonDerivedType(typeof(Square), typeDiscriminator: "square")]
[JsonDerivedType(typeof(Circle), typeDiscriminator: "circle")]
public abstract class Shape
{
    public string ShapeType { get; set; }
    public string Color { get; set; } = string.Empty;
    public int Sides { get; set; }
}

internal class Triangle : Shape
{
    public double Hypotenuse { get; set; }
}
internal class Square : Shape
{
    public double Area { get; set; }
}
internal class Circle : Shape
{
    public double Radius { get; set; }
}