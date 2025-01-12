using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;

namespace Utilities;

public interface IUtilityFunctions
{
    List<Dictionary<string, object>> JsonElementToListOfObjects(JsonElement element);
    string GetStringPositions(int position);
}