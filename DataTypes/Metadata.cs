using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

public class Metadata
{
    // Properties with the Description attribute have a description in the generated schema
    [Description("A description of the property")]
    public string Description { get; set; }

    // Properties with the Required attribute are required in the generated schema
    [Required]
    public int RequiredAttribute { get; set; }

    // Properties with a DefaultValue attribute have a default in the generated schema
    [DefaultValue(42)]
    public int DefaultValueAttribute { get; set; }

    [Range(1, 100)]
    public int IntWithRange { get; set; }

    [Range(0.00, 1.00)]
    public double DoubleWithRange { get; set; }

    [MaxLength(63)]
    public string StringWithMaxLength { get; set; }

    [MinLength(1)]
    public string StringWithMinLength { get; set; }

    [RegularExpression(@"^[A-Za-z0-9-]*$")]
    public string StringWithPattern { get; set; }
}
