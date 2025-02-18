using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;

namespace Utilities;

public interface ITeamServices
{
    List<Dictionary<string, object>> GetTeams(JsonElement element);
    public Dictionary<string, List<Player>> GetPositions(List<Player>? roster);
}