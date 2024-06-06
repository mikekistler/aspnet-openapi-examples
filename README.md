# aspnet-openapi-examples

<!-- cspell:ignore aspnet openapi -->

Examples of using ASP.NET to create OpenAPI v3 API definitions.

This guide focuses on generating OpenAPI from a Minimal APIs ASP.NET application.
It is also possible to generate OpenAPI for controller-based applications, but
some details will vary from the information below.

## Schemas

C# classes or records used in request or response bodies are represented as schemas
in the generated OpenAPI document.

### properties

By default, only public properties are represented in the schema, but there are [JsonSerializerOptions]
to also create schema properties for fields. 

### property names

By default in a .NET web application, property names in a schema are the camel-case form
of the class or record property name. This can be changed using the `PropertyNamigPolicy` in the
[JsonSerializerOptions], and can be changed on an individual property with the the 
[`JsonPropertyName`] attribute.

[`JsonPropertyName`]: https://docs.microsoft.com/dotnet/api/system.text.json.serialization.jsonpropertynameattribute?view=net-9.0

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

Required properties in a record constructor are also required in the generated schema.

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

You can set entries in the servers property with a [DocumentTransformer]. Within your transformer code,
set or add entries to the `document.Servers` property.

## Paths Object

The `paths` object is just a map of path to `pathItem`. ASP.NET creates an entry in this map for every path
specified in a Map[Get,Put,Post,Delete,Patch] method.

OpenAPI allows specification extensions in a `paths` object -- these can be added with a [DocumentTransformer].

## Path Item Object

The `get`, `put`, `post`, `delete`, and `patch` fields of a `pathItem` can be set with the corresponding
ASP.NET Map[Get,Put,Post,Delete,Patch] method.

Use a [DocumentTransformer] to set the `summary`, `description`, `options`, `head`, `trace`, `servers`, or
`parameters` fields, or to add specification extensions.

## Operation Object

Each Map[Get,Put,Post,Delete,Patch] method invocation will create an operation. The following operation properties
can be set using attributes or extension methods:

| to set property | use extension method | or attribute | 
| --------------- | --- | ----|
| summary | WithSummary | `[EndpointSummary()]` |
| description | WithDescription | `[EndpointDescription()]` |
| operationId | WithName | `[EndpointName()]` |
| tags | WithTags | |

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

Use a [DocumentTransformer] or an [OperationTransformer] to set the `externalDocs`, `callbacks`, `deprecated`, `security`,
or `servers` properties of an operation.

## Parameters / Parameter Object

Delegate method parameters that are explicitly or implicitly `[FromQuery]`, `[FromPath]`, or `[FromHeader]`
are included in the parameter list, with the `in` value set accordingly and a schema as described in the [Schemas] section above.

### name

The name of the parameter in the delegate method is used as-is in the `name` field of the parameter object --
no case-convention is applied -- but an alternate can be specified in the parameters of the `From{Query,Path,Header}`
attribute.

### description

Currently you need to use a [DocumentTransformer] or an [OperationTransformer] to set the `description` on a parameter --
the `[Description]` attribute sets the description in the schema but not on the parameter itself.

Unfortunately, some tools e.g. SwaggerUi only use the description in the parameter and ignore the one in the parameter schema.

### required

The `required` property of a parameter is determined by its type:
- a non-nullable value type parameter is marked as `required: true`
- a nullable value type parameter is implicitly `required: false`
- a non-nullable reference type parameter is marked as `required: true`
- a nullable reference type parameter is implicitly `required: false`

Note this differs from the way properties of a schema are determined to be required.

### schema

The `schema` property of a parameter object is set as described in the [Schemas] section above.

### other properties

The `deprecated`, `allowEmptyValue`,`style`, `explode`, `allowReserved`, `example`, and `examples`
properties are not currently included in parameter objects. 
Use a [DocumentTransformer] or an [OperationTransformer] to set any of these properties when needed.

## Request Body Object

If there is a delegate method parameter that is explicitly or implicitly `[FromBody]`, this is used
to set the `requestBody` property of the operation. By default the MIME type of the request body is
`application/json`, but this can be changed with the `Accepts` extension method on the delegate method.
Use `Accepts` if you need to specify multiple MIME types.

Note that if you specify `Accepts` multiple times, only the last one will be used -- they are not combined.

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

<!-- Links -->

[DocumentTransformer]: https://learn.microsoft.com/aspnet/core/fundamentals/minimal-apis/aspnetcore-openapi?view=aspnetcore-9.0#openapi-document-transformers
[OperationTransformer]: https://learn.microsoft.com/aspnet/core/fundamentals/minimal-apis/aspnetcore-openapi?view=aspnetcore-9.0#use-operation-transformers
[JsonSerializerOptions]: https://docs.microsoft.com/dotnet/api/system.text.json.jsonserializeroptions?view=net-9.0