using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Interfaces;
using Microsoft.OpenApi.Models;

internal sealed class InfoTransformer() : IOpenApiDocumentTransformer
{
    public async Task TransformAsync(OpenApiDocument document, OpenApiDocumentTransformerContext context, CancellationToken cancellationToken)
    {
        // title and version are already set

        // Add a contact
        document.Info.Contact = new OpenApiContact()
        {
            Email = "admin@contoso.com",
            Name = "Contoso Admin",
            Url = new Uri("https://contoso.com/contacts")
        };

        // Add a description. This could be multi-line and contain markdown.
        document.Info.Description =
        """
        # Examples of using ASP.NET to create OpenAPI v3 API definitions

        This is an example of the info.description content. This description and most other
        description fields in OpenAPI v3.x support [CommonMark] markdown content.

        [CommonMark]: https://spec.commonmark.org/

        Below are examples of the CommonMark markdown elements.

        ## Headings

        ### H3

        ## Text styling

        **bold text**

        *italicized text*
        
        > blockquote

        `code`

        ## Lists

        ### Ordered List

        1. First item
        2. Second item
        3. Third item

        ### Unordered List

        - First item
        - Second item
        - Third item

        ## Horizontal Rule

        ---

        ### Links

        [Markdown Guide](https://www.markdownguide.org)

        ### Image

        ![alt text](https://www.markdownguide.org/assets/images/tux.png)

        ### Table

        | Syntax | Description |
        | ----------- | ----------- |
        | Header | Title |
        | Paragraph | Text |

        ### Fenced Code Block

        ```
        {
        "firstName": "John",
        "lastName": "Smith",
        "age": 25
        }
        ```

        """;

        document.Info.License = new OpenApiLicense()
        {
            Name = "MIT",
            Url = new Uri("https://opensource.org/licenses/MIT")
        };

        // Most of the OpenApi model classes contain an "Extensions" property where you can set
        // specification extensions.
        document.Info.Extensions = new Dictionary<string, IOpenApiExtension>
        {
            { "x-my-extension", new OpenApiString("my-extension-value") }
        };
    }
}