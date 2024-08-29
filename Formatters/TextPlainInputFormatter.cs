using System.Text;
using Microsoft.AspNetCore.Mvc.Formatters;

// Sample text/plain input formatter from
// https://github.com/aspnet/Entropy/blob/master/samples/Mvc.Formatters/TextPlainInputFormatter.cs

public class TextPlainInputFormatter : TextInputFormatter
{
    public TextPlainInputFormatter()
    {
        SupportedMediaTypes.Add("text/plain");
        SupportedEncodings.Add(UTF8EncodingWithoutBOM);
        SupportedEncodings.Add(UTF16EncodingLittleEndian);
    }

    protected override bool CanReadType(Type type)
    {
        return type == typeof(string);
    }

    public override async Task<InputFormatterResult> ReadRequestBodyAsync(InputFormatterContext context, Encoding encoding)
    {
        string data = null;
        using (var streamReader = context.ReaderFactory(context.HttpContext.Request.Body, encoding))
        {
            data = await streamReader.ReadToEndAsync();
        }
        return InputFormatterResult.Success(data);
    }
}
