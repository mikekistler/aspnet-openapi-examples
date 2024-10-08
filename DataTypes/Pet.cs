using System.Text.Json.Serialization;

[JsonPolymorphic(TypeDiscriminatorPropertyName = "petType")]
[JsonDerivedType(typeof(Dog), "dog")]
[JsonDerivedType(typeof(Cat), "cat")]
[JsonDerivedType(typeof(Fish), "fish")]
public class Pet  // Concrete base class
{
    public string Name { get; set; }
    public int Age { get; set; }
}

public class Dog : Pet
{
    public string Breed { get; set; }
}

public class Cat : Pet
{
    public bool Declawed { get; set; }
}

public class Fish : Pet
{
    public string? Species { get; set; }
}
