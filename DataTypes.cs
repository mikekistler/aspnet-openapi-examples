using Microsoft.AspNetCore.DataProtection;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

public class DataTypes
{
    /// <summary>
    /// type: integer, format: int32
    /// </summary>
    public int Int { get; set; }

    /// <summary>
    /// type: integer, format: int64
    /// </summary>
    public long Long { get; set; }

    /// <summary>
    /// type: integer, format: int32
    /// </summary>
    public short Short { get; set; }

    /// <summary>
    /// type: number, format: float
    /// </summary>
    public float Float { get; set; }

    /// <summary>
    /// type: integer, format: double
    /// </summary>
    public double Double { get; set; }

    /// <summary>
    /// type: boolean
    /// </summary>
    public bool Bool { get; set; }

    /// <summary>
    /// type: string
    /// </summary>
    public string String { get; set; }

    /// <summary>
    /// type: string, format: int32
    /// </summary>
    public byte Byte { get; set; }

    // This property has no `type` defined.
    //public unknown notype { get; set; }

    /// <summary>
    /// type: string, format: date-time
    /// </summary>
    public DateTimeOffset DateTimeOffset { get; set; }

    /// <summary>
    /// Customized to type: string, format: date
    /// </summary>
    public DateOnly DateOnly { get; set; }

    /// <summary>
    /// Customized to type: string, format: time
    /// </summary>
    public TimeOnly TimeOnly { get; set; }

    // type: string, format: password
    //public string password { get; set; }

    // OpenAPI allows any string as a format.
    // There is a registry of common formats at
    // https://spec.openapis.org/registry/format

    //[Description("type: string, format: decimal")]
    //public decimal Decimal { get; set; }

    //[Description("type: string, format: int8")]
    //public sbyte Int8 { get; set; }

    //[Description("type: integer, format: uint8")]
    //public byte Uint8 { get; set; }

    /// <summary>
    /// type: string, format: uri
    /// </summary>
    public Uri Uri { get; set; }

    /// <summary>
    /// type: string, format: uuid
    /// </summary>
    public Guid Uuid { get; set; }

    // For formats that are not supported directly, you can ...
    //@encode("http-date")
    //public utcDateTime httpDate { get; set; }
}