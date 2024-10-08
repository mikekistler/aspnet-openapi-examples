using System;

public class DataTypes
{
    // type: integer, format: int32
    public int Int { get; set; }

    // type: integer, format: int64
    public long Long { get; set; }

    // type: integer, format: int16
    public short Short { get; set; }

   // type: string, format: uint8
    public byte Byte { get; set; }

    // type: number, format: float
    public float Float { get; set; }

    // type: number, format: double
    public double Double { get; set; }

    // type: number, format: double
    public decimal Decimal { get; set; }

    // type: boolean
    public bool Bool { get; set; }

    // type: string
    public string String { get; set; }

    // type: string, format: char
    public char Char { get; set; }

    // type: string, format: byte
    public byte[] ByteArray { get; set; }

    // type: string, format: date-time
    public DateTimeOffset DateTimeOffset { get; set; }

    // type: string, format: date
    public DateOnly DateOnly { get; set; }

    // type: string, format: time
    public TimeOnly TimeOnly { get; set; }

    // type: string, format: uri
    public Uri Uri { get; set; }

    // type: string, format: uuid
    public Guid Uuid { get; set; }

    // An object property has no `type` defined.
    public object Object { get; set; }

    // dynamic properties also have no `type` defined.
    public dynamic Dynamic { get; set; }
}