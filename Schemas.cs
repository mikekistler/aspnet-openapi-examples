﻿using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor.
public class Schemas
{
    // Internal properties are not included in the generated schema properties
    internal string Internal { get; set; }

    // By default, class fields are not included in the generated schema properties
    string Field;

    [Description("Properties with the Description attribute have a description in the generated schema")]
    public string Description { get; set; }

    [Description("Properties with the required modifier are required in the generated schema")]
    public required int RequiredModifier { get; set; }

    [Description("Properties with the Required attribute are _not_ required in the generated schema")]
    [Required]
    public int RequiredAttribute { get; set; }

    [Description("Properties with a default value do _not_ have a default in the generated schema")]
    public int Default { get; set; } = 42;

    [Description("Properties with a DefaultValue attribute have a default in the generated schema")]
    [DefaultValue(42)]
    public int DefaultValueAttribute { get; set; }

    [Range(1, int.MaxValue)]
    public int IntWithRange { get; set; }

    [Range(0.00, 1.00)]
    public double DoubleWithRange { get; set; }

    //[Range(typeof(DateOnly), "2024/05/27", "2024/12/31")]
    //public DateOnly DateWithRange { get; set; }
    // build fails with "The input string '2024/05/27' was not in a correct format."

    [MaxLength(63)]
    public string StringWithMaxLength { get; set; }

    [MinLength(1)]
    public string StringWithMinLength { get; set; }

    [RegularExpression(@"^[A-Za-z0-9-]*$")]
    public string StringWithPattern { get; set; }

    public string? NullableString { get; set; }

    [Description("Nested schema")]
    public Nested Inner { get; set; }
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

#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

