using System.Text.Json;

namespace Shall.Verify.Common.Helpers;

public class JsonSerializerOptionsWrapper
{
    public JsonSerializerOptions Options { get; }

    public JsonSerializerOptionsWrapper()
    {
        Options = new JsonSerializerOptions(
            JsonSerializerDefaults.Web);
    }
}