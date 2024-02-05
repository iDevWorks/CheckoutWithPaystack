using System.Text.Json;

namespace iDevWorks
{
    internal class Common
    {
        public static JsonSerializerOptions JsonOptions = new()
        {
            PropertyNameCaseInsensitive = true, 
            AllowTrailingCommas = true, 
        };

    }
}
