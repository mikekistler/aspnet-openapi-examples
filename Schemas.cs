using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

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
    /// Use xml comments with the summary tag to produce a description in the generated schema
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
    /// String with example value
    /// </summary>
    /// <example>Example value</example>
    public string StringWithExample { get; set; }

    /// <summary>
    /// Int with example value
    /// </summary>
    /// <example>42</example>
    public int IntWIthExample { get; set; }

    public string NonNullableRef { get; set; }
    public string? NullableRef { get; set; }
    public int NonNullableValue { get; set; }
    public int? NullableValue { get; set; }

    public DayOfTheWeek Enum { get; set; }
    public Whacky WhackyEnum { get; set; }
    public DayOfTheWeekAsString EnumAsString { get; set; }

    [AllowedValues("Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday")]
    public string AllowedValues { get; set; }

    [Description("Nested schema")]
    public Nested Inner { get; set; }
}
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

public enum DayOfTheWeek
{
    Sunday,
    Monday,
    Tuesday,
    Wednesday,
    Thursday,
    Friday,
    Saturday
}

[JsonConverter(typeof(JsonStringEnumConverter<DayOfTheWeekAsString>))]
public enum DayOfTheWeekAsString
{
    Sunday,
    Monday,
    Tuesday,
    Wednesday,
    Thursday,
    Friday,
    Saturday
}

public enum Whacky {
    Gravity = 1,
    Antigravity = -1,
    Answer = 42
}

public record Nested(
    [Description("this is a date")]
    DateOnly Date,

    [Description("Temp in Celcius")]
    [Range(0, 100)]
    int TemperatureC,

    [MinLength(1)]
    [MaxLength(63)]
    string? Summary)
{
    [Description("Temp in Farenheit")]
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}