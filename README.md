# aspnet-openapi-examples

<!-- cspell:ignore aspnet openapi swashbuckle -->

<!-- Editorial note: use "route handler" rather than "route handler" or "endpoint method" -->
<!-- Only use "code style" for OpenAPI fields - use links for C#/.NET classes/methods/properties -->
<!-- Wrap attribute names in brackets -->

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
of the class or record property name. This can be changed using the [PropertyNamingPolicy] in the
[JsonSerializerOptions], and can be changed on an individual property with the
[\[JsonPropertyName\]] attribute.

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
| object         | _omitted_      |                  |
| dynamic        | _omitted_      |                  |

The `type` and `format` can also be set with a [Schema Transformer].

Swashbuckle provides the [MapType method on the AddSwaggerGen] options to customize the
`type` and `format` for a C# type.
The examples project includes a schema transformer called [TypeTransformer](./Transformers/TypeTransformer.cs)
that does more or less the same thing.

[MapType method on the AddSwaggerGen]: https://github.com/domaindrivendev/Swashbuckle.AspNetCore?tab=readme-ov-file#override-schema-for-specific-types

### description

Use the [\[Description\]] attribute to set the `description` of a property.

### required

Properties with either the [required] modifier or the [\[Required\]] attribute are required in the generated schema.

Required properties in a record constructor are also required in the generated schema.

[required]: https://learn.microsoft.com/dotnet/csharp/language-reference/proposals/csharp-11.0/required-members#required-modifier

### default

Use the [\[DefaultValue\]] attribute to set the `default` value of a property in the schema.

Note that properties with a default value do _not_ have a default in the generated schema.

### minimum and maximum

Use the [\[Range\]] attribute to set the `minimum` and `maximum` values of an `integer`, or `number`.

From the docs, it looks like it should be possible to use [\[Range\]] with [DateTimeOffset] properties, but it doesn't seem to work.

Also, there does not seem to be a way to produce just `minimum` or `maximum` without the other.

### minLength and maxLength

Use the [\[MinLength\]] attribute and [\[MaxLength\]] attribute to set the `minLength` and `maxLength` of a `string`.

### pattern

Use the [\[RegularExpression\]] attribute to set the `pattern` of a `string`.

### example

Currently you must use a [DocumentTransformer] to add an `example` to a the schema.

### nullable

Properties defined as a nullable value or reference type will have `"nullable": true` in the generated schema.

### readOnly

Computed properties (properties with an initial value and no setter) are _not_ marked as `readOnly` in the generated schema.

Properties with the `ReadOnly` attribute are _not_ marked as `readOnly` in the generated schema.

### enum

Properties with an `enum` type are represented as an `enum` in the generated schema. Since all C# enums are
integer-based, the property is defined with `type: integer` and `format: int32`, and
the `enum` values are the implicit or explicit values of the C# enum.

To get string-based enums, you can use the [\[JsonConverter\]] attribute with a [\[JsonStringEnumConverter\]]
on a regular C# enum type.

Note: the [\[AllowedValues\]] attribute does not set the `enum` values of a property.

## Info

You can add or replace content in the Info section with a [DocumentTransformer]. Within your transformer code,
set fields in the `document.Info` property.

.NET automatically fills in the `Title` and `Version`. There are also `Contact`, `License`, and `Description` fields.
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
| `summary` | [WithSummary] | [\[EndpointSummary()\]] |
| `description` | [WithDescription] | [\[EndpointDescription()\]] |
| `operationId` | [WithName] | [\[EndpointName()\]] |
| `tags` | [WithTags] | |

Note that the extension methods are supported on both [RouteHandlerBuilder] and on [RouteGroupBuilder],
but when used on [RouteGroupBuilder] they are applied to all operations in the group,
so it's not likely that [WithSummary], [WithDescription], or [WithName] should be used on a [RouteGroupBuilder].

The `parameters` property of an operation is set from the parameters of the route handler.
Route handler parameters that are explicitly or implicitly [FromQuery], [FromPath], or [FromHeader]
are included in the parameter list.

If there is a route handler parameter that is explicitly or implicitly [FromBody], this is used
to set the `requestBody` property of the operation.

The `responses` object is populated from several sources.

- the declared return value of the route handler
- the [Produces] extension method on the delegate.

See the [Responses] section below for details on how to set `responses`.

Use a [DocumentTransformer] or an [OperationTransformer] to set the `externalDocs`, `callbacks`, `deprecated`, `security`,
or `servers` properties of an operation.

## Parameters / Parameter Object

route handler parameters that are explicitly or implicitly [FromQuery], [FromPath], or [FromHeader]
are included in the parameter list, with the `in` value set accordingly and a schema as described in the [Schemas] section above.

### name

The name of the parameter in the route handler is used as-is in the `name` field of the parameter object --
no case-convention is applied -- but an alternate name can be specified in the parameters of the From{Query,Path,Header}
attribute.

### description

Use the [\[Description\]] attribute to set the description on the parameter object.

### required

The `required` property of a parameter is determined by its type:

- a non-nullable value type parameter is marked as `required: true`
- a nullable value type parameter is implicitly `required: false`
- a non-nullable reference type parameter is marked as `required: true`
- a nullable reference type parameter is implicitly `required: false`

Note this differs from the way properties of a schema are determined to be required.

### style and explode

The `style` and `explode` properties are not explicitly specified in parameter objects
because their default value is consistent with the behavior of ASP.NET parameter binding.
In particular, the defaults for query parameters are `style: form, explode: true`, which
means that each element of an array parameter is specified with its own `name=` in the query string.

### schema

The `schema` property of a parameter object is set as described in the [Schemas] section above,
with the exception that parameter schemas never include `nullable: true`.

### other properties

The `deprecated`, `allowEmptyValue`, `allowReserved`, `example`, and `examples`
properties are not currently included in parameter objects.
Use a [DocumentTransformer] or an [OperationTransformer] to set any of these properties when needed.

## Request Body Object

If a route handler has a parameter that is explicitly or implicitly [FromBody] or an explicit
[FromForm] parameter, this is used to set the `requestBody` property of the operation.

### description

If the [FromBody] or [FromForm] parameter has a [Description] attribute, this description
is included in the requestBody in the generated OpenAPI document.

Set the `description` field of the requestBody with a [\[Description\]] attribute on the [FormBody] parameter.

### required

If the [FromBody] parameter is non-nullable, the requestBody is required and will have its
`required` property set to `true` in the generated OpenAPI document. Form bodies are always required.

### content

`FromBody` parameters are "application/json" by default, and this support is built in to Minimal APIs.

To accept non-json request bodies you should define a custom type for the `FromBody` parameter
that implements [IBindableFromHttpContext\<T\>] and [IEndpointParameterMetadataProvider].
This allows the framework to bind the parameter from the request body in the HTTP context.
For request bodies that are primitive types or sealed classes, you can create a class
that wraps the actual request body and implements the necessary interfaces.

The `Accepts` extension method could be used to specify the content type of the request body
instead of implementing [IEndpointParameterMetadataProvider], but this is not the recommended approach
as it separates the parameter metadata from the parameter binding logic.

If the endpoint does not define a `FromBody` parameter, but instead consumes the request body
from the `HttpContext` directly, use the `Accepts` extension method to specify the content type
that the endpoint accepts. This is a common scenario for endpoints that process the request body
as a stream.

Note that if you specify Accepts multiple times, only the last one will be used -- they are not combined.

#### Form bodies

[FromForm] parameters may be bound from either a "multipart/form-data" or "application/x-www-form-urlencoded"
request body, except when the route handler defines a file-type, i.e. [IFormFile], parameter,
which is only supported for "multipart/form-data".
ASP.NET sets the endpoint metadata to specify which content-types are allowed and this produces
the appropriate `content` entries for operation in the generated OpenAPI document.

The request body schema will contain a property for each FromForm parameter with a schema derived from the parameter type.
The property name will be the same as the parameter name with no case-coercion, but this can be overridden
with the Name parameter on FromForm, similar to the other parameter binding attributes.

### mediaTypeObject

The `content` property of the `requestBody` object is a map of MIME type to `mediaTypeObject`.
The `schema` property of a `mediaTypeObject` is set as described above.

Use a [DocumentTransformer] or an [OperationTransformer] to set the `example`, `examples`, `encoding`, or
to add specification extensions to the `mediaTypeObject`.

## Responses

Response definitions can set using any of the following approaches:

- a [Produces] extension method on the endpoint
- a [`ProducesProblem`] extension method on the endpoint for error responses
- a `ProducesResponseType` attribute on the route handler
- define the route handler return type to be one or more `TypedResults`

See [Describe response types] in the .NET documentation for more information.

[Describe response types]: https://learn.microsoft.com/aspnet/core/fundamentals/minimal-apis/openapi?view=aspnetcore-9.0#describe-response-types

These approaches will set a numeric `statusCode`, the response MIME type (defaults to "application/json"),
and schema of the response object.
Use a [DocumentTransformer] or an [OperationTransformer] to define a response with a non-numeric status code,
such as "default" or "4XX".
The response `description` (which is required) is set to a standard value based on the status code,
but can be overridden with a transformer.

For the [Produces] extension method and `ProducesResponseType` attribute,
there is no validation of the type specified against the actual response object returned
from the route handler.

The `ProducesProblem` extension method will add an entry to `responses` for the specified status code
with an "application/problem+json" content entry with a schema for the [ProblemDetails] class.

Responses can also be populated from the route handler return type.
The [`TypedResults`] class provides a set of static methods to wrap a response object and
define a corresponding operation response.
Note that the route handler return type may be defined implicitly when all return points
return an object of the same type.
If there are multiple return types, the return type must be explicitly defined
on the handler using [`Results`]. For more information see the
[.NET docs on Learn](https://learn.microsoft.com/aspnet/core/fundamentals/minimal-apis/responses#typedresults-vs-results).

[`TypedResults`]: https://learn.microsoft.com/dotnet/api/microsoft.aspnetcore.http.typedresults?view=aspnetcore-9.0
[`Results`]: https://learn.microsoft.com/aspnet/core/fundamentals/minimal-apis/responses?view=aspnetcore-9.0#resultstresult1-tresultn

Note that `Results` only supports from 2 to 6 different return types.
Don't use `Results` with a single return type -- it will fail to compile.
Likewise, if you try to specify more than 6 return types, it will fail to compile.

When the route handler is asynchronous, the return type must have the `Task<>` wrapper.

Only return types that implement [IEndpointMetadataProvider] will create a `responses` entry in the OpenAPI document.
Here's a quick list of some of the [TypedResults] helper methods that produce a `responses` entry:

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
section below for information about how to produce content entries for other status codes or media types.

You can implement your own class to set the endpoint metadata that is used to create a `response` object.
The examples project contains two classes that illustrate this:

- The [OkTextPlain] class creates a 200 response and sets the MIME type to "text/plain".
- The [OkImage] class also creates a 200 response with three `content` entries for "image/jpeg", "image/png", and "image/tiff".

Note that the [CreatedAtRoute] and [AcceptedAtRout] methods do not define a `Location` header in the response object.

To return a typed [ProblemDetails] response with the StatusCodePages middleware, pass `null` to the [TypedResults]
helper method -- this sets the body to `null` which will trigger the middleware to add a [ProblemDetails] response body.

Use a transformer to set the `headers`, `links`, or to add specification extensions to the response object.

[OkTextPlain]: ./OkTextPlain.cs
[OkImage]: ./OkImage.cs

### content

You can create multiple `mediaTypeObject` entries in the `content` property of a response object
by specifying all the media types in the [Produces] extension method on the endpoint.
However, each of these `mediaTypeObject` entries will have the same schema, so if you need different
schemas for different media types, you will need to use a transformer.

It is possible but currently a bit complicated to generate content entries for media types other than "application/json"
from the return type of the route handler. To do this, you need to create a class that implements [IResult] and [IEndpointMetadataProvider]
and make this class the return type of the route handler.
There is an example of this in the [OkTextPlain] class in the examples project.

Use a transformer to set the `example`, `examples`, `encoding`, or
to add specification extensions to any `mediaTypeObject` within the `content` of the response.

## More schema fields

### additionalProperties

Schemas are generated without an `additionalProperties` assertion by default, which then applies the default of `true`.

To generate a schema with a specific `additionalProperties` assertion, define the property or class as a Dictionary<string, type>.
The key type for the dictionary should be `string`, and the schema for the value type will be the `additionalProperties` schema.

There does not appear to be a way to create a schema that has both properties and `additionalProperties`.
Defining a class that extends Dictionary and has named properties does not work -- its schema only has `additionalProperties`.
<!-- https://github.com/dotnet/aspnetcore/issues/56707 -->

The [\[JsonExtensionData\]] attribute also cannot be used to generate `additionalProperties` next to named properties,
because the value type of the dictionary must be object or JsonElement -- it cannot be a more restrictive type
like string or int.
In other words, `JsonExtensionData` only corresponds to `additionalProperties: true`, which is the default.

### allOf

ASP.NET "flattens" the schema of child classes, meaning that it includes all the properties of the parent class
in the schema for the child class. It does not use `allOf` to add the parent's properties to the schema of the child class.

### discriminator with anyOf

Use the `System.Text.Json` `JsonPolymorphic` and `JsonDerivedType` attributes on a parent class to
to specify the discriminator property and subtypes for a polymorphic type.
The `JsonDerivedType` attribute adds the discriminator property to the schema for each subclass,
with an enum specifying the specific discriminator value for the subclass.
This attribute also modifies the constructor of each derived class to set the discriminator value.

### anyOf / oneOf

There is no in-built way to produce an `anyOf` without a discriminator or a `oneOf`.
If you use case needs one of these then you will need to use a schema transformer to produce them.

## securitySchemes and security requirements

Use a [DocumentTransformer] to add security schemes and security requirements.

## Specification Extensions

Most of the OpenApi model classes contain an "Extensions" property, and you can use a transformer to set
specification extensions using this property.

## Links to Docs on Learn

- AspNetCore.Http
  - [Accepts] extension method
  - [IBindableFromHttpContext\<T\>]
  - [IEndpointParameterMetadataProvider]
  - [IFormFile]
  - [Produces] extension method
  - [`ProducesProblem`] extension method

[Accepts]: https://learn.microsoft.com/dotnet/api/microsoft.aspnetcore.http.openapiroutehandlerbuilderextensions.accepts
[IBindableFromHttpContext\<T\>]: https://learn.microsoft.com/dotnet/api/microsoft.aspnetcore.http.ibindablefromhttpcontext-1
[IEndpointParameterMetadataProvider]: https://learn.microsoft.com/dotnet/api/microsoft.aspnetcore.http.metadata.iendpointparametermetadataprovider
[IFormFile]: https://learn.microsoft.com/dotnet/api/microsoft.aspnetcore.http.iformfile?view=aspnetcore-9.0
[Produces]: https://learn.microsoft.com/dotnet/api/microsoft.aspnetcore.http.openapiroutehandlerbuilderextensions.produces
[`ProducesProblem`]: https://learn.microsoft.com/dotnet/api/microsoft.aspnetcore.http.openapiroutehandlerbuilderextensions.producesproblem

- AspNetCore.Mvc
  - [FromForm] attribute
  - [ProblemDetails]
  - [ProducesResponseType]

[FromForm]: https://learn.microsoft.com/dotnet/api/microsoft.aspnetcore.mvc.fromformattribute
[ProblemDetails]: https://learn.microsoft.com/dotnet/api/microsoft.aspnetcore.mvc.problemdetails
[ProducesResponseType]: https://learn.microsoft.com/dotnet/api/microsoft.aspnetcore.mvc.producesresponsetypeattribute

- AspNetCore.OpenApi
  - [DocumentTransformer]
  - [OperationTransformer]

[DocumentTransformer]: https://learn.microsoft.com/aspnet/core/fundamentals/minimal-apis/aspnetcore-openapi?view=aspnetcore-9.0#openapi-document-transformers
[OperationTransformer]: https://learn.microsoft.com/aspnet/core/fundamentals/minimal-apis/aspnetcore-openapi?view=aspnetcore-9.
0#use-operation-transformers

- System.ComponentModel
  - [\[AllowedValues\]] attribute
  - [\[DefaultValue\]] attribute
  - [\[Description\]] attribute
  - [\[MaxLength\]] attribute
  - [\[MinLength\]] attribute
  - [\[Range\]] attribute
  - [\[RegularExpression\]] attribute
  - [\[Required\]] attribute

[\[AllowedValues\]]: https://learn.microsoft.com/dotnet/api/system.componentmodel.dataannotations.allowedvaluesattribute
[\[DefaultValue\]]: https://learn.microsoft.com/dotnet/api/system.componentmodel.defaultvalueattribute
[\[Description\]]: https://learn.microsoft.com/dotnet/api/system.componentmodel.descriptionattribute
[\[MinLength\]]: https://learn.microsoft.com/dotnet/api/system.componentmodel.dataannotations.minlengthattribute
[\[MaxLength\]]: https://learn.microsoft.com/dotnet/api/system.componentmodel.dataannotations.maxlengthattribute
[\[Range\]]: https://learn.microsoft.com/dotnet/api/system.componentmodel.dataannotations.rangeattribute
[\[RegularExpression\]]: https://learn.microsoft.com/dotnet/api/system.componentmodel.dataannotations.regularexpressionattribute
[\[Required\]]: https://learn.microsoft.com/dotnet/api/system.componentmodel.dataannotations.requiredattribute

- System.Text.Json
  - [\[JsonConverter\]] attribute
  - [\[JsonExtensionData\]] attribute
  - [\[JsonPropertyName\]] attribute
  - [\[JsonStringEnumConverter\]] attribute
  - [JsonSerializerOptions]
    - [PropertyNamingPolicy]

[\[JsonConverter\]]: https://learn.microsoft.com/dotnet/api/system.text.json.serialization.jsonconverterattribute
[\[JsonExtensionData\]]: https://learn.microsoft.com/dotnet/api/system.text.json.serialization.jsonextensiondataattribute
[\[JsonPropertyName\]]: https://docs.microsoft.com/dotnet/api/system.text.json.serialization.jsonpropertynameattribute
[\[JsonStringEnumConverter\]]: https://learn.microsoft.com/dotnet/api/system.text.json.serialization.jsonstringenumconverter
[JsonSerializerOptions]: https://docs.microsoft.com/dotnet/api/system.text.json.jsonserializeroptions
[PropertyNamingPolicy]: https://docs.microsoft.com/dotnet/api/system.text.json.jsonserializeroptions.propertynamingpolicy
