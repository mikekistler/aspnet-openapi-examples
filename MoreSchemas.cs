using System.Text.Json.Serialization;

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

[JsonDerivedType(typeof(Triangle), typeDiscriminator: "triangle")]
[JsonDerivedType(typeof(Square), typeDiscriminator: "square")]
[JsonDerivedType(typeof(Circle), typeDiscriminator: "circle")]
internal abstract class Shape
{
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
    public double diameter { get; set; }
}
