using System.Text.Json.Serialization;

// STJ will add the type discriminator property to the derived types
[JsonPolymorphic(TypeDiscriminatorPropertyName = "shapeType")]
[JsonDerivedType(typeof(Circle), typeDiscriminator: "circle")]
[JsonDerivedType(typeof(Square), typeDiscriminator: "square")]
[JsonDerivedType(typeof(Triangle), typeDiscriminator: "triangle")]
internal abstract class Shape
{
    // Don't define the discriminator property on the base class
    // STJ will automatically add it to the derived classes
    public string Color { get; set; } = string.Empty;
    public int Sides { get; set; }
}

internal class Circle : Shape
{
    // Don't define the discriminator property in child classes
    public double Radius { get; set; }
}
internal class Square : Shape
{
    // Don't define the discriminator property in child classes
    public double Area { get; set; }
}
internal class Triangle : Shape
{
    // Don't define the discriminator property in child classes
    public double Hypotenuse { get; set; }
}

