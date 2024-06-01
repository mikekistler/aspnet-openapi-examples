# aspnet-openapi-examples

<!-- cspell:ignore aspnet openapi -->

Examples of using ASP.NET to create OpenAPI v3 API definitions

## Schemas

### type and format

The JSON Schema library maps standard C# types to OpenAPI `type` and `format` as follows:

| C# Type        | OpenAPI `type` | OpenAPI `format` |
| -------------- | -------------- | ---------------- |
| int            | integer        | int32            |
| long           | integer        | int64            |
| short          | integer        | int16            |
| float          | number         | float            |
| double         | number         | double           |
| bool           | boolean        |                  |
| string         | string         |                  |
| byte           | string         | byte             |
| DateTimeOffset | string         | date-time        |
| DateOnly       | string         | date             |
| TimeOnly       | string         | time             |
| Uri            | string         | uri              |
| Guid           | string         | uuid             |

### description

Use the [`Description` attribute] to set the `description` of a property.

[`Description` attribute]: https://learn.microsoft.com/dotnet/api/system.componentmodel.descriptionattribute?view=net-8.0

### required

Properties with the [required modifier] are required in the generated schema.

Properties with the [Required attribute] are _not_ required in the generated schema.

[required modifier]: https://learn.microsoft.com/dotnet/csharp/language-reference/proposals/csharp-11.0/required-members#required-modifier
[Required attribute]: https://learn.microsoft.com/dotnet/api/system.componentmodel.dataannotations.requiredattribute

### default

Properties with a default value do _not_ have a default in the generated schema.

### minimum and maximum

Use the [`Range` attribute] to set the `minimum` and `maximum` values of an `integer`, or `number`.

[`Range` attribute]: https://learn.microsoft.com/dotnet/api/system.componentmodel.dataannotations.rangeattribute

From the docs, it looks like it should be possible to use `Range` with `DateTimeOffset` properties, but it doesn't seem to work.

Also, there does not seem to be a way to produce just `minimum` or `maximum` without the other.

### minLength and maxLength

Use the [`MinLength` attribute] and [`MaxLength` attribute] to set the `minLength` and `maxLength` of a `string`.

[`MinLength` attribute]: https://learn.microsoft.com/dotnet/api/system.componentmodel.dataannotations.minlengthattribute
[`MaxLength` attribute]: https://learn.microsoft.com/dotnet/api/system.componentmodel.dataannotations.maxlengthattribute

### pattern

Use the [`RegularExpression` attribute] to set the `pattern` of a `string`.

[`RegularExpression` attribute]: https://learn.microsoft.com/dotnet/api/system.componentmodel.dataannotations.regularexpressionattribute

### example / examples

### nullable

Properties defined as a nullable value type will have `"nullable": true` in the generated schema.

## Info

You can add or replace content in the Info section with a [DocumentTransformer]. Within your transformer code,
set fields in the `document.Info` property.

.NET automatically fills in the `Title` and `Version`. There are also `Contact`, `Licence`, and `Description` fields.
The `Info.Description` and most other description fields in OpenAPI v3.x support [CommonMark] markdown content.

[CommonMark]: https://spec.commonmark.org/

The Info model and most of the OpenApi model classes contain an "Extensions" property where you can set
specification extensions.

## Servers

## Paths Object

## Path Item Object

## Operation Object

### tag

### description and summary

### externalDocs

### operationId

## Parameters / Parameter Object

### style and explode

## Request Body Object

### Mime Type

### multipart/form-data

## Responses

## Response Object

## Schema Object

### additionalProperties

### allOf

### discriminator

### anyOf

### oneOf

### oneOf with discriminator

### nullable

### readOnly

### external $ref

## securityDefinitions / securitySchemes

## Specification Extensions
