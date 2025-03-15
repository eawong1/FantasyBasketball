using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;

namespace Utilities;

public interface IUtilityFunctions
{
    List<Dictionary<string, object>> JsonElementToListOfObjects(JsonElement element);
    string GetStringPositions(int position);
    void ParseJsonLogin(string jsonPath, out string leagueId, out string leagueYear, out string swid, out string espnS2);
    void CheckLoginJsonSchema(string jsonPath, string? schemaPath);
}