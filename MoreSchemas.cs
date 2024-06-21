using System.ComponentModel;
using System.Text.Json.Serialization;
using Swashbuckle.AspNetCore.Annotations;

public class MoreSchemas
{
}

internal class DictBody : Dictionary<string, string> { }

internal class DictProperty
{
    public Dictionary<string, string> Dict { get; set; }
}

internal class ExtendsDict : Dictionary<string, string>
{
    public string Title { get; set; }
    public int Priority { get; set; }
}

public class Parent
{
    public string Prop1 { get; set; }
    public string Prop2 { get; set; }
}

public class Child : Parent
{
    public string Prop3 { get; set; }
}

[SwaggerDiscriminator("shapeType")]
[SwaggerSubType(typeof(Triangle), DiscriminatorValue = "triangle")]
[SwaggerSubType(typeof(Square), DiscriminatorValue = "square")]
[SwaggerSubType(typeof(Circle), DiscriminatorValue = "circle")]
internal abstract class Shape
{
    public string ShapeType { get; set; } = string.Empty;
    public string Color { get; set; } = string.Empty;
    public int Sides { get; set; }
}

internal class Triangle : Shape
{
    public new string ShapeType { get; set; } = "triangle";
    public double Hypotenuse { get; set; }
}
internal class Square : Shape
{
    public new string ShapeType { get; set; } = "square";
    public double Area { get; set; }
}
internal class Circle : Shape
{
    public new string ShapeType { get; set; } = "circle";
    public double Radius { get; set; }
}
