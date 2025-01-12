using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Runtime.InteropServices;
using System.Text.Json;
using Avalonia.Markup.Xaml.Templates;
using Avalonia.Win32.Interop.Automation;
using Microsoft.VisualBasic;
using Utilities;

public class League
{
    private static Dictionary<string, JsonElement> m_responseData;
    private static List<Dictionary<string, object>>? m_teams;
    private List<Player>? m_roster;
    private IUtilityFunctions m_utility;
    public League(Dictionary<string, JsonElement> responseData, IUtilityFunctions utility)
    {
        m_responseData = responseData;
        m_utility = utility;
    }

    public List<string> GetTeamNames()
    {
        List<string> teamNames = new List<string>();
        
        m_teams = m_utility.JsonElementToListOfObjects(m_responseData["teams"]);
        
        foreach(var team in m_teams)
        {
            teamNames.Add(team["name"].ToString());
        }

        return teamNames;
    }

    public List<Player> GetRoster(string teamName)
    {
        
        if(m_roster == null)
        {
            m_roster = new List<Player>();
        }
        var team = m_teams.FirstOrDefault(dict => dict["name"].ToString() == teamName);
        
        using JsonDocument jsonDoc = JsonDocument.Parse(JsonSerializer.Serialize(team["roster"]));
        JsonElement root = jsonDoc.RootElement;
        if (root.ValueKind == JsonValueKind.String)
        {
            var jsonString = root.GetString();
            using JsonDocument doc = JsonDocument.Parse(jsonString);
            JsonElement parsedElement = doc.RootElement;

            foreach(var entries in parsedElement.GetProperty("entries").EnumerateArray())
            {
                if (entries.TryGetProperty("playerPoolEntry", out JsonElement playerPoolEntryElement))
                {
                    var playerElement = playerPoolEntryElement.GetProperty("player");
                    var eligibleSlots = playerElement.GetProperty("eligibleSlots").EnumerateArray();
                    var playerPositions = new List<string>();

                    foreach(var slot in eligibleSlots)
                    {
                        playerPositions.Add(m_utility.GetStringPositions(slot.GetInt32()));
                    }

                    m_roster.Add(new Player(playerElement.GetProperty("fullName").GetString(), playerPositions));
                }
            }
        }

        return m_roster;
    }

    public Dictionary<string, List<Player>> GetPositions(string teamName)
    {
        var positions = new Dictionary<string, List<Player>>();
        
        if(m_roster?.Any() != true)
        {
            GetRoster(teamName);
        }
        
        foreach(var player in m_roster)
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