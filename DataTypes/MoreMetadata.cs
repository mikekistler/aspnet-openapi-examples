using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

public class MoreMetadata
{
    // Properties with the required modifier are required in the generated schema
    public required int RequiredModifier { get; set; }

    // nullable value or reference type will have `nullable: true` in the generated schema
    public string NonNullableRef { get; set; }
    public int NonNullableValue { get; set; }
    public string? NullableRef { get; set; }
    public int? NullableValue { get; set; }

    // A Dictionary will be represented as an object with additionalProperties in the generated schema
    public Dictionary<string, string> Dictionary { get; set; }
}

public class Enums
{
    // Enums with a JsonConverter attribute are represented as `type: string` with an `enum` in the generated schema.
    public DayOfTheWeekAsString EnumAsString { get; set; }

    // Normal C# enum types are represented as `type: integer` with no `enum` in the generated schema.
    public DayOfTheWeek Enum { get; set; }

    // The AllowedValues attribute does not add an `enum` to the generated schema.
    [AllowedValues("Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday")]
    public string AllowedValues { get; set; }

    // C# flag enums are represented as `type: integer` with no `enum` in the generated schema.
    public FlagsEnum FlagsEnum { get; set; }

    // Flag enums with a JsonConverter attribute are represented as `type: string` with an `enum` in the generated schema.
    public FlagsEnumAsString FlagsEnumAsString { get; set; }
}

public enum DayOfTheWeek { Sunday, Monday, Tuesday, Wednesday, Thursday, Friday, Saturday }

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

[Flags]
public enum FlagsEnum { Value1 = 1, Value2 = 2, Value4 = 4, Value8 = 8 }

[Flags, JsonConverter(typeof(JsonStringEnumConverter<FlagsEnumAsString>))]
public enum FlagsEnumAsString { Value1 = 1, Value2 = 2, Value4 = 4, Value8 = 8 }