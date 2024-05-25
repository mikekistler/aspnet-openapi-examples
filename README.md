# aspnet-openapi-examples

<!-- cspell:ignore aspnet openapi -->

Examples of using Swashbuckle and ASP.NET to create OpenAPI v3 API definitions.

This project is using version 6.6.2 of Swashbuckle.AspNetCore, the most recent version when the project was developed.

A common means to provide Swashbuckle with information about your API is to use [XML comments]
on methods and classes.

[XML comments]: https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/xmldoc/recommended-tags

You must configure your project to generate an XML documentation file and then
configure Swashbuckle to use it. See the [XML Comments] section of the ASP.NET documentation
on using Swashbuckle for more information.

[XML Comments]: https://learn.microsoft.com/en-us/aspnet/core/tutorials/getting-started-with-swashbuckle?view=aspnetcore-8.0&tabs=visual-studio#xml-comments

## Schemas

### type and format

Swashbuckle maps standard C# types to OpenAPI `type` and `format` as follows:

| C# Type        | OpenAPI `type` | OpenAPI `format` |
| -------------- | -------------- | ---------------- |
| int            | integer        | int32            |
| long           | integer        | int64            |
| short          | integer        | int32            |
| float          | number         | float            |
| double         | number         | double           |
| bool           | boolean        |                  |
| string         | string         |                  |
| byte           | string         | int32            |
| DateTimeOffset | string         | date-time        |
| DateOnly       | string         | "$ref": "#/components/schemas/DateOnly" |
| TimeOnly       | string         | ""$ref": #/components/schemas/TimeOnly" |
| Uri            | string         | uri              |
| Guid           | string         | uuid             |

However, it is very easy to override these mappings with the `MapType` method on the `AddSwaggerGen` options.

For example, to change the mapping of `DateTimeOnly` to `type: string, format: date`:

```csharp
c.MapType<DateOnly>(() => new OpenApiSchema { Type = "string", Format = "date" });
```

### enum

### required

### default

### Schema assertions

## Info / Servers

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
