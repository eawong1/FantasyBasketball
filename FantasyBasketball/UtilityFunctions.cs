using System.Collections.Generic;
using System.Text.Json;

namespace Utilities
{
    public class UtilityFunctions
    {
        public static List<Dictionary<string, object>> JsonElementToListOfObjects(JsonElement element)
        {
            var list = new List<Dictionary<string, object>>();

            if (element.ValueKind == JsonValueKind.Array)
            {
                foreach (var item in element.EnumerateArray())
                {
                    var dictionary = new Dictionary<string, object>();

                    foreach (var prop in item.EnumerateObject())
                    {
                        dictionary[prop.Name] = prop.Value.ToString(); // Handle the value conversion as needed
                    }

                    list.Add(dictionary);
                }
            }

            return list;
        }
    }
}

