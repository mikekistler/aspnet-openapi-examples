using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor.
/// <summary>
/// Represents the schemas.
/// </summary>
public class Schemas
{
    // Internal properties are not included in the generated schema properties
    internal string Internal { get; set; }

    // By default, class fields are not included in the generated schema properties
    string Field;

    /// <summary>
    /// Use xml comments with the <summary> tag to produce a description in the generated schema
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// Properties with the required modifier are _not_ required in the generated schema
    /// </summary>
    public required int RequiredModifier { get; set; }

    /// <summary>
    /// Properties with the Required attribute are required in the generated schema
    /// </summary>
    [Required]
    public int RequiredAttribute { get; set; }

    /// <summary>
    /// Properties with a default value do _not_ have a default in the generated schema
    /// </summary>
    public int Default { get; set; } = 42;

    /// <summary>
    /// Properties with a Default Attribute have a default in the generated schema
    /// </summary>
    [DefaultValue(42)]
    public int DefaultAttribute { get; set; }

    /// <summary>
    /// Specifies the range of valid values for IntWithRange
    /// </summary>
    [Range(1, int.MaxValue)]
    public int IntWithRange { get; set; }

    /// <summary>
    /// Specifies the range of valid values for DoubleWithRange
    /// </summary>
    [Range(0.00, 1.00)]
    public double DoubleWithRange { get; set; }

    /// <summary>
    /// Specifies the maximum length of StringWithMaxLength
    /// </summary>
    [MaxLength(63)]
    public string StringWithMaxLength { get; set; }

    /// <summary>
    /// Specifies the minimum length of StringWithMinLength
    /// </summary>
    [MinLength(1)]
    public string StringWithMinLength { get; set; }

    /// <summary>
    /// Specifies the pattern that StringWithPattern must match
    /// </summary>
    [RegularExpression(@"^[A-Za-z0-9-]*$")]
    public string StringWithPattern { get; set; }

    /// <summary>
    /// Swashbuckle defines all strings as nullable, whether or not they are C# nullable.
    /// </summary>
    public string? NullableString { get; set; }
}
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

