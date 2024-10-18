using System.Reflection;
using System.Xml.Linq;
using System.Xml.Serialization;
using Microsoft.AspNetCore.Http.Metadata;

// This class represents a request body that is expected to be in XML format.
// It implements the IBindableFromHttpContext interface to allow the framework to bind the parameter
// from the request body in the HTTP context.
// It also implements the IEndpointParameterMetadataProvider interface to provide metadata about the parameter.
public class Todo : IBindableFromHttpContext<Todo>, IEndpointParameterMetadataProvider
{
    // This method is called by the framework to bind the parameter from the request body
    // in the HTTP context.
    // The framework validates the content type of the request, i.e. that it matches one of
    // the content types in the AcceptsMetadata, before calling this method.
    public static async ValueTask<Todo?> BindAsync(HttpContext context, ParameterInfo parameter)
    {
        var xmlDoc = await XDocument.LoadAsync(context.Request.Body, LoadOptions.None, context.RequestAborted);
        var serializer = new XmlSerializer(typeof(Todo));
        return (Todo?)serializer.Deserialize(xmlDoc.CreateReader());
    }

    public static void PopulateMetadata(ParameterInfo parameter, EndpointBuilder builder)
    {
        builder.Metadata.Add(new AcceptsMetadata(["application/xml", "text/xml"], typeof(Todo)));
    }

    public int Id { get; set; }

    public string Title { get; set; } = default!;

    public string? Content { get; set; } = default!;

    public string? DueOn { get; set; }

    public string? CompletedOn { get; set; }
}