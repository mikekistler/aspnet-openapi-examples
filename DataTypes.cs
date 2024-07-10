using Microsoft.AspNetCore.DataProtection;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

public class DataTypes
{
    [Description("type: integer, format: int32")]
    public int Int { get; set; }

    [Description("type: integer, format: int64")]
    public long Long { get; set; }

    [Description("type: integer, format: int16")]
    public short Short { get; set; }

    [Description("type: number, format: float")]
    public float Float { get; set; }

    [Description("type: integer, format: double")]
    public double Double { get; set; }

    [Description("type: boolean")]
    public bool Bool { get; set; }

    [Description("type: string")]
    public string String { get; set; }

    [Description("type: string, format: byte")]
    public byte Byte { get; set; }

    [Description("type: ???, format: ???")]
    public byte[] ByteArray { get; set; }

    public decimal Decimal { get; set; }

    // An object property has no `type` defined.
    public object Object { get; set; }

    // dynamic properties also have no `type` defined.
    public dynamic Dynamic { get; set; }

    [Description("type: string, format: date-time")]
    public DateTimeOffset DateTimeOffset { get; set; }

    [Description("type: string, format: date")]
    public DateOnly DateOnly { get; set; }

    [Description("type: string, format: time")]
    public TimeOnly TimeOnly { get; set; }

    // type: string, format: password
    //public string password { get; set; }

    // OpenAPI allows any string as a format.
    // There is a registry of common formats at
    // https://spec.openapis.org/registry/format

    //[Description("type: string, format: int8")]
    //public sbyte Int8 { get; set; }

    //[Description("type: integer, format: uint8")]
    //public byte Uint8 { get; set; }

    [Description("type: string, format: uri")]
    public Uri Uri { get; set; }

    [Description("type: string, format: uuid")]
    public Guid Uuid { get; set; }

    // For formats that are not supported directly, you can ...
    //@encode("http-date")
    //public utcDateTime httpDate { get; set; }
}