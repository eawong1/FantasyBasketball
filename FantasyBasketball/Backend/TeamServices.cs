
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text.Json;

namespace Utilities;

public class TeamServices : ITeamServices
{
    private readonly IUtilityFunctions m_utility;

    public TeamServices(IUtilityFunctions utility)
    {
        m_utility = utility;
    }

    public List<Dictionary<string, object>> GetTeams(JsonElement element)
    {
        return m_utility.JsonElementToListOfObjects(element);
    }

    public Dictionary<string, List<Player>> GetPositions(List<Player>? roster)
    {
        var positions = new Dictionary<string, List<Player>>();
        
        foreach(var player in roster)
        {
            var eligiblePositions = player.GetEligiblePositions();
            foreach(var pos in eligiblePositions)
            {
                if(pos == "BE" || pos == "IR" || pos == "UTIL")
                {
                    continue;
                }
                if(positions.TryGetValue(pos, out List<Player> value))
                {
                    value.Add(player);
                }
                else
                {
                    positions[pos] = new List<Player>{player};
                }
            }
        }

        return positions;
    }
}