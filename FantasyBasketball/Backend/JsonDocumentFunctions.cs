using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Text.Json;
using System.Threading.Tasks;

namespace Utilities;

public class JsonDocumentFunctions : IJsonDocumentFunctions
{
    JsonElement.ArrayEnumerator IJsonDocumentFunctions.EnumerateArray(JsonElement element)
    {
        return element.EnumerateArray();
    }

    JsonElement IJsonDocumentFunctions.GetProperty(JsonElement element, string propertyName)
    {
        return element.GetProperty(propertyName);
    }

    JsonDocument IJsonDocumentFunctions.Parse(string jsonString)
    {
        return JsonDocument.Parse(jsonString);
    }

    string IJsonDocumentFunctions.Serialize(object players)
    {
        return JsonSerializer.Serialize(players);
    }
}