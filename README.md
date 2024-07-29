# aspnet-openapi-examples

Examples of using ASP.NET to create OpenAPI v3 API definitions

This guide focuses on generating OpenAPI, specifically [OpenAPI v3.0.x], from
a controller-based ASP.NET application.

[OpenAPI v3.0.x]: https://github.com/OAI/OpenAPI-Specification/blob/3.0.3/versions/3.0.3.md

## Schemas

C# classes or records used in request or response bodies are represented as schemas
in the generated OpenAPI document.

### properties

By default, only public properties are represented in the schema, but there are [JsonSerializerOptions]
to also create schema properties for fields.

### property names

By default in a .NET web application, property names in a schema are the camel-case form
of the class or record property name. This can be changed using the `PropertyNamingPolicy` in the
[JsonSerializerOptions], and can be changed on an individual property with the
[`JsonPropertyName`] attribute.

[`JsonPropertyName`]: https://docs.microsoft.com/dotnet/api/system.text.json.serialization.jsonpropertynameattribute?view=net-9.0

### type and format

The JSON Schema library maps standard C# types to OpenAPI `type` and `format` as follows:

| C# Type        | OpenAPI `type` | OpenAPI `format` |
| -------------- | -------------- | ---------------- |
| int            | integer        | int32            |
| long           | integer        | int64            |
| short          | integer        | int16            |
| byte           | integer        | uint8            |
| float          | number         | float            |
| double         | number         | double           |
| decimal        | number         | double           |
| bool           | boolean        |                  |
| string         | string         |                  |
| char           | string         | char             |
| byte[]         | string         | byte             |
| DateTimeOffset | string         | date-time        |
| DateOnly       | string         | date             |
| TimeOnly       | string         | time             |
| Uri            | string         | uri              |
| Guid           | string         | uuid             |
| object         | &lt.omitted&gt. |                 |
| dynamic        | &lt.omitted&gt. |                 |

The `type` and `format` can also be set with a [Schema Transformer].

### Schema assertions

### enum

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

Currently you must use a [DocumentTransformer] to add an `example` to a the schema.

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

The `paths` object is just a map of path to `pathItem`. ASP.NET creates an entry in this map for each
path specified on a controller class using the [`RouteAttribute`].

Paths appear in the generated paths object in alphabetical order by path.

OpenAPI allows specification extensions in a paths object -- these can be added with a DocumentTransformer.

[`RouteAttribute`]: https://learn.microsoft.com/dotnet/api/microsoft.aspnetcore.mvc.routeattribute

## Path Item Object

The get, put, post, delete, patch, options, and head fields of a pathItem can be set by
defining an endpoint method in the controller with the correspoinding HTTP method attribute, e.g. [`HttpGetAttribute`].

Use a DocumentTransformer to set the summary, description, trace, servers, or parameters fields, or to add specification extensions.

[`HttpGetAttribute`]: https://learn.microsoft.com/dotnet/api/microsoft.aspnetcore.mvc.httpgetattribute

## Operation Object

Each method of the controller with an Http method attribute will create an operation.
The following operation properties can be set using attributes:

| to set property | use attribute |
| --------------- | --- | ----|
| summary | `[EndpointSummary()]` |
| description | `[EndpointDescription()]` |
| operationId | `[EndpointName()]` |

Xml doc comments are not currently supported to set the operation summary, description,
or descriptions on parameters.

The tags property of the operation is set with a single tag name derived from the controller
class name by removing the "Controller" suffix (convention).

The `parameters` property of an operation is set from the parameters of the endpoint method.
See the [Parameters / Parameter Object](#parameters-/-parameter-object) section below for details on how to set `responses`.

If there is a endpoint method parameter that is explicitly or implicitly `[FromBody]`, this is used
to set the `requestBody` property of the operation.

The `responses` object is populated from the `Produces` attribute on the endpoint method.
See the [Responses] section below for details on how to set `responses`.

Use a [DocumentTransformer] or an [OperationTransformer] to set the `externalDocs`, `callbacks`, `deprecated`, `security`,
or `servers` properties of an operation.

## Parameters / Parameter Object

Endpoint method parameters that are explicitly or implicitly `[FromQuery]`, `[FromPath]`, or `[FromHeader]`
are included in the parameter list, with the `in` value set accordingly and a schema as described in the [Schemas] section above.

### name

The name of the parameter in the endpoint method is used as-is in the `name` field of the parameter object --
no case-convention is applied -- but an alternate name can be specified in the parameters of the `From{Query,Path,Header}`
attribute.

### description

Use the `[Description]` attribute to set the description on the parameter.

### required

The `required` property of a parameter can be is set to `true` for all parameters that are
explicitly or implicitly `[FromPath]` and all parameters with the `[Required]` attribute.

### style and explode

The `style` and `explode` properties are not explicitly specified in parameter objects
because their default value is consistent with the behavior of ASP.NET parameter binding.
In particular, the defaults for query parameters are `style: form, explode: true`, which
means that each element of an array parameter is specified with its own `name=` in the query string.

### schema

The `schema` property of a parameter object is set as described in the [Schemas] section above.

### other properties

The `deprecated`, `allowEmptyValue`, `allowReserved`, `example`, and `examples`
properties are not currently included in parameter objects.
Use a [DocumentTransformer] or an [OperationTransformer] to set any of these properties when needed.

## Request Body Object

If a route handler has a parameter that is explicitly or implicitly `FromBody`, this is used
to set the `requestBody` property of the operation. The `requestBody` is also set if the route
handler has any parameters with the `[FromForm]` attribute.

It is also possible to use the [`Consumes`]  attribute on the route handler to define the request body
in cases where the route handler does not define a `FromBody` or `FromForm` parameter but accesses the request body
from the HTTPContext. `Consumes` can specify multiple content-types and content-type ranges, e.g. "image/*".
`Consumes` adds a filter to the route handler that checks the content-type of incoming requests and rejects
any request that does not match one of the specified content-types.

Note that if you specify `Consumes` multiple times, only the last one will be used -- they are not combined.

### required

The `required` property of a `requestBody` object is set to `true` if the `[FromBody]` method parameter
is a non-nullable type. Otherwise the `required` property is omitted and defaults to `false`.

### multipart/form-data

An operation that accepts "multipart/form-data" should use `Accepts` to set the correct MIME type and
a `FromBody` method parameter with a type that defines the form-data fields.

### mediaTypeObject

The `content` property of the `requestBody` object is a map of MIME type to `mediaTypeObject`.
The `schema` property of a `mediaTypeObject` is set as described above.

Use a [DocumentTransformer] or an [OperationTransformer] to set the `example`, `examples`, `encoding`, or
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
