# aspnet-openapi-examples

<!-- cspell:ignore aspnet openapi swashbuckle -->

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

C# classes or records used in request or response bodies are represented as schemas
in the generated OpenAPI document.

### properties

By default, only public properties are represented in the schema.

### property names

Swashbuckle converts C# property names to lower camel-case in the generated schema.
There may be an option to configure a different naming strategy, but if not this could be
done with a [schema filter].

[schema filter]: https://github.com/domaindrivendev/Swashbuckle.AspNetCore?tab=readme-ov-file#schema-filters

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

### description

Use the [`summary` tag] in XML comments to set the `description` of a property.

[`summary` tag]: https://learn.microsoft.com/dotnet/csharp/language-reference/xmldoc/recommended-tags#summary

### required

Properties with either the [required modifier] or the [Required attribute] are required in the generated schema.

[required modifier]: https://learn.microsoft.com/dotnet/csharp/language-reference/proposals/csharp-11.0/required-members#required-modifier
[Required attribute]: https://learn.microsoft.com/dotnet/api/system.componentmodel.dataannotations.requiredattribute

### default

Properties with a default value do _not_ have a default in the generated schema.

Properties with the [`DefaultValue` attribute] have a default in the generated schema.

[`DefaultValue` attribute]: https://learn.microsoft.com/dotnet/api/system.componentmodel.defaultvalueattribute

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

Use the XML doc [`Example` tag] to set the `example` of a property.

[`Example` tag]: https://learn.microsoft.com/dotnet/csharp/language-reference/xmldoc/recommended-tags#example

### nullable

Reference type properties have `nullable: true` in the generated schema.

Nullable value type properties have `nullable: true` in the generated schema.

Non-nullable value type properties have `nullable: false` in the generated schema.

### enum

Properties with an `enum` type are represented as an `enum` in the generated schema. Since all C# enums are
integer-based, the property is defined with `type: integer` and `format: int32`, and
the `enum` values are the implicit or explicit values of the C# enum.

To get string-based enums, you can use the [`JsonConverter` attribute] with a [`JsonStringEnumConverter`]
on a regular C# enum type.

[`JsonConverter` attribute]: https://learn.microsoft.com/dotnet/api/system.text.json.serialization.jsonconverterattribute
[`JsonStringEnumConverter`]: https://learn.microsoft.com/dotnet/api/system.text.json.serialization.jsonstringenumconverter

Note: Swashbuckle does not add an `enum` to the schema for a property with the [`AllowedValues` attribute].

[`AllowedValues` attribute]: https://learn.microsoft.com/dotnet/api/system.componentmodel.dataannotations.allowedvaluesattribute`]

## Info

You can add or replace content in the Info section with a [Document Filter]. Within your filter code,
set fields in the `document.Info` property.

Swashbuckle automatically fills in the `Title` and `Version`. There are also `Contact`, `Licence`, and `Description` fields.
The `Info.Description` and most other description fields in OpenAPI v3.x support [CommonMark] markdown content.

[CommonMark]: https://spec.commonmark.org/

The Info model and most of the OpenApi model classes contain an "Extensions" property where you can set
specification extensions.

## Servers

You can set entries in the servers property with the `AddServer` method on the `AddSwaggerGen` options.

## Paths Object

The `paths` object is just a map of path to `pathItem`. Swashbuckle creates an entry in this map for every path
specified in a Map[Get,Put,Post,Delete,Patch] method.

Swashbuckle generates the `paths` object in alphabetical order by tags.

OpenAPI allows specification extensions in a `paths` object -- these can be added with a [Document Filter].

## Path Item Object

The `get`, `put`, `post`, `delete`, and `patch` fields of a `pathItem` can be set with the corresponding
ASP.NET Map[Get,Put,Post,Delete,Patch] method.

Use a [Document Filter] to set the `summary`, `description`, `options`, `head`, `trace`, `servers`, or
`parameters` fields, or to add specification extensions.

## Operation Object

Each Map[Get,Put,Post,Delete,Patch] method invocation will create an operation. The following operation properties
can be set using attributes or extension methods:

| to set property | use extension method | or attribute |
| --------------- | -------------------- | ------------ |
| summary         | WithSummary          | `[EndpointSummary()]` |
| description     | WithDescription      | `[EndpointDescription()]` |
| operationId     | WithName             | `[EndpointName()]` |
| tags            | WithTags             | |

Note that the extension methods are supported on both `RouteHandlerBuilder` and on `RouteGroupBuilder`,
but when used on `RouteGroupBuilder` they are applied to all operations in the group,
so it's not likely that `WithSummary`, `WithDescription`, or `WithName` should be used on a `RouteGroupBuilder`.

The `parameters` property of an operation is set from the parameters of the delegate method.
Delegate method parameters that are explicitly or implicitly `[FromQuery]`, `[FromPath]`, or `[FromHeader]`
are included in the parameter list.

If there is a delegate method parameter that is explicitly or implicitly `[FromBody]`, this is used
to set the `requestBody` property of the operation.

The `responses` object is populated from several sources.

- the declared return value of the delegate method
- the value(s) returned from the delegate method
- the `Produces` extension method on the delegate.

See the [Responses] section below for details on how to set `responses`.

Use a [Document Filter] or an [Operation Filter] to set the `externalDocs`, `callbacks`, `deprecated`, `security`,
or `servers` properties of an operation.

## Parameters / Parameter Object

Delegate method parameters that are explicitly or implicitly `[FromQuery]`, `[FromPath]`, or `[FromHeader]`
are included in the parameter list, with the `in` value set accordingly and a schema as described in the [Schemas] section above.

### name

The name of the parameter in the delegate method is used as-is in the `name` field of the parameter object --
no case-convention is applied -- but an alternate can be specified in the parameters of the `From{Query,Path,Header}`
attribute.

### description

There appears to be no simple means to add a description to operation parameters when using delegate methods.
But you can use the `[Description]` attribute on parameters of an endpoint handler implemented as a static method.

Neither the `[Description]` attribute nor xml comments seem to work.

However there is the fallback of using a [Document Filter] or an [Operation Filter] to set the `description` property.

### required

The `required` property of a parameter is determined by its nullability:

- a non-nullable parameter is marked as `required: true`
- a nullable parameter is implicitly `required: false`

### schema

The `schema` property of a parameter object is set as described in the [Schemas] section above.

### other properties

The `deprecated`, `allowEmptyValue`,`style`, `explode`, `allowReserved`, `example`, and `examples`
properties are not currently included in parameter objects.
Use a [Document Filter] or an [Operation Filter] to set any of these properties when needed.

### style and explode

## Request Body Object

If there is a delegate method parameter that is explicitly or implicitly `[FromBody]`, this is used
to set the `requestBody` property of the operation. By default the MIME type of the request body is
`application/json`, but this can be changed with the `Accepts` extension method on the delegate method.
Use `Accepts` if you need to specify multiple MIME types.

Note that if you specify `Accepts` multiple times, only the last one will be used -- they are not combined.

### required

The `required` property of a `requestBody` object is set to `true` if the `[FromBody]` method parameter
is a non-nullable type. Otherwise the `required` property is omitted and defaults to `false`.

### multipart/form-data

An operation that accepts "multipart/form-data" should use `Accepts` to set the correct MIME type and
a `FromBody` method parameter with a type that defines the form-data fields.

### mediaTypeObject

The `content` property of the `requestBody` object is a map of MIME type to `mediaTypeObject`.
The `schema` property of a `mediaTypeObject` is set as described above.

Use a [Document Filter] or an [Operation Filter] to set the `example`, `examples`, `encoding`, or
to add specification extensions to the `mediaTypeObject`.

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

<!-- Links -->
[Document Filter]: https://github.com/domaindrivendev/Swashbuckle.AspNetCore?tab=readme-ov-file#document-filters
[Operation Filter]: https://github.com/domaindrivendev/Swashbuckle.AspNetCore?tab=readme-ov-file#operation-filters