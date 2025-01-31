using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;

namespace Utilities;

public interface IJsonDocumentFunctions
{
    JsonDocument Parse(string jsonString);
    string Serialize(object players);
    JsonElement GetProperty(JsonElement element, string propertyName);
    JsonElement.ArrayEnumerator EnumerateArray(JsonElement element);
    (bool result, JsonElement output) TryGetProperty(JsonElement entries, string propertyName);
}