# aspnet-openapi-examples

<!-- cspell:ignore aspnet openapi -->

Examples of using ASP.NET to create OpenAPI v3 API definitions.

This guide focuses on generating OpenAPI, specifically [OpenAPI v3.0.x], from
a Minimal APIs ASP.NET application.
It is also possible to generate OpenAPI for controller-based applications, but
some details will vary from the information below.

[OpenAPI v3.0.x]: https://github.com/OAI/OpenAPI-Specification/blob/3.0.3/versions/3.0.3.md

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

<!-- -->
Properties with the [Required attribute] are _not_ required in the generated schema.

[required modifier]: https://learn.microsoft.com/dotnet/csharp/language-reference/proposals/csharp-11.0/required-members#required-modifier
[Required attribute]: https://learn.microsoft.com/dotnet/api/system.componentmodel.dataannotations.requiredattribute

### default

<!-- Support added in PR #56219 (preview6) -->
Use the [`DefaultValue` attribute] to set the `default` value of a property in the schema.

Note that properties with a default value do _not_ have a default in the generated schema.

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

### example

Currently you must use a [DocumentTransformer] to add an `example` to a the schema.

### nullable

Properties defined as a nullable value or reference type will have `"nullable": true` in the generated schema.

## enum

Properties with an `enum` type are represented as an `enum` in the generated schema. Since all C# enums are
integer-based, the property is defined with `type: integer` and `format: int32`, and
the `enum` values are the implicit or explicit values of the C# enum.

To get string-based enums, you can use the [`JsonConverter` attribute] with a [`JsonStringEnumConverter`]
on a regular C# enum type.

[`JsonConverter` attribute]: https://learn.microsoft.com/dotnet/api/system.text.json.serialization.jsonconverterattribute
[`JsonStringEnumConverter`]: https://learn.microsoft.com/dotnet/api/system.text.json.serialization.jsonstringenumconverter

Note: the [`AllowedValues` attribute] does not set the `enum` values of a property.

[`AllowedValues` attribute]: https://learn.microsoft.com/dotnet/api/system.componentmodel.dataannotations.allowedvaluesattribute`]

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

Paths appear in the generated `paths` object in the order in which they are defined in the application.

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
no case-convention is applied -- but an alternate name can be specified in the parameters of the `From{Query,Path,Header}`
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

Response definitions can set using any of the following approaches:
- a `Produces` extension method on the endpoint
- a `ProducesResponseType` attribute on the route handler
- define the route handler return type to be one or more `TypedResults`

See [Describe response types] in the .NET documentation for more information.

[Describe response types]: https://learn.microsoft.com/aspnet/core/fundamentals/minimal-apis/openapi?view=aspnetcore-9.0#describe-response-types]

These approaches will set a numeric `statusCode`, the response MIME type (defaults to "applicaiton/json"),
and schema of the response object.
Use a [DocumentTransformer] or an [OperationTransformer] to define a response with a non-numeric status code,
such as "default" or "4XX".
The response `description` (which is required) is set to a standard value based on the status code,
but can be overridden with a transformer.

For the `Produces` extension method and `ProducesResponseType` attribute, 
there is no validation of the type specified against the actual response object returned
from the delegate method.

If the endpoint method returns typed results, most commonly using the `TypedResults` helper methods,
the response type may be inferred from the return type of the delegate method.
If the method returns a single result type, the return type does not need to be defined explicitly,
because it can be inferred by the compiler, but if there are multiple return types, you have to explicitly define
the return type on the handler using [`Results`]. For more information see the
[.NET docs on Learn](https://learn.microsoft.com/aspnet/core/fundamentals/minimal-apis/responses#typedresults-vs-results).

[`Results`]: https://learn.microsoft.com/aspnet/core/fundamentals/minimal-apis/responses?view=aspnetcore-9.0#resultstresult1-tresultn

Note that `Results` only supports from 2 to 6 different return types. Don't use `Results` with a single return type --
it will fail to compile. Likewise, if you try to specify more than 6 return types, it will fail to compile.

When the endpoint method is asynchronous, the return type must have the `Task<>` wrapper.

Only `TypedResults` helper methods that return a class that implements `IEndpointMetadataProvider` will create
a `responses` entry in the OpenAPI document. Here's a quick list of some of the `TypedResults` helper methods
that produce a `responses` entry:

| TypedResults helper method | status code |
| -------------------------- | ----------- |
| Ok()                       | 200         |
| Created()                  | 201         |
| CreatedAtRoute()           | 201         |
| Accepted()                 | 202         |
| AcceptedAtRoute()          | 202         |
| NoContent()                | 204         |
| BadRequest()               | 400         |
| ValidationProblem()        | 400         |
| NotFound()                 | 404         |
| Conflict()                 | 409         |
| UnprocessableEntity()      | 422         |

All of these methods except `NoContent` have a generic overload that allows you to specify the type of the response body,
which is used to produce an "application/json" content entry with the schema for the specified type. See the [content](#content)
section below for information about how to produce content entries for other media types.

Note that the `CreatedAtRoute` and `AcceptedAtRoute` methods do not set the `Location` header in the response object.

To return a typed ProlblemDetails response with the StatusCodePages middleware, pass `null` to the `TypedResults`
helper method -- this sets the body to `null` which will trigger the middleware to add a ProblemDetails response body.

Use a transformer to set the `headers`, `links`, or to add specification extensions to the response object.

### content

You can create multiple `mediaTypeObject` entries in the `content` property of a response object
by specifying all the media types in the `Produces` extension method on the endpoint.
However, each of these `mediaTypeObject` entries will have the same schema, so if you need different
schemas for different media types, you will need to use a transformer.

It is possible but currently a bit complicated to generate content entries for media types other than "application/json"
from the return type of the endpoint method. To do this, you need to create a class that implements `IResult` and `IEndpointMetadataProvider`
and make this class the return type of the endpoint method. There is an example of this in the [OkTextPlain] class in the
project.

Use a transformer to set the `example`, `examples`, `encoding`, or
to add specification extensions to any `mediaTypeObject` within the `content` of the response.

## More schema fields

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