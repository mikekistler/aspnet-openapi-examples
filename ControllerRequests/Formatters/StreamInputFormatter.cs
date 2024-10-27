using Microsoft.AspNetCore.Mvc.Formatters;

public class StreamInputFormatter : InputFormatter
{
    public StreamInputFormatter()
    {
        SupportedMediaTypes.Add("application/octet-stream");
    }

    protected override bool CanReadType(Type type)
    {
        return type == typeof(Stream);
    }

    public override async Task<InputFormatterResult> ReadRequestBodyAsync(InputFormatterContext context)
    {
        return InputFormatterResult.Success(context.HttpContext.Request.Body);
    }
}