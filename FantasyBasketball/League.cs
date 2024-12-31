using System;
using System.Collections.Generic;
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
    // private Dictionary<string, List<Positions>>? m_roster;
    public League(Dictionary<string, JsonElement> responseData)
    {
        m_responseData = responseData;
    }

    public List<string> GetTeamNames()
    {
        List<string> teamNames = new List<string>();
        
        m_teams = UtilityFunctions.JsonElementToListOfObjects(m_responseData["teams"]);
        
        foreach(var team in m_teams)
        {
            teamNames.Add(team["name"].ToString());
        }

        return teamNames;
    }

    public Dictionary<string, List<int>> GetRoster(string teamName)
    {
        
        var roster = new Dictionary<string, List<int>>();

        //! something is wrong with this filtering logic below
        var team = m_teams.FirstOrDefault(dict => dict["name"].ToString() == teamName);
        
        var temp1 = JsonSerializer.Serialize(team["roster"]);
        using JsonDocument jsonDoc = JsonDocument.Parse(temp1);
        JsonElement root = jsonDoc.RootElement;
        if (root.ValueKind == JsonValueKind.String)
        {
            // Console.WriteLine($"String value: {root.GetString()}");
            var jsonString = root.GetString();
            using JsonDocument doc = JsonDocument.Parse(jsonString);
            JsonElement parsedElement = doc.RootElement;

            // var temp = ;
            foreach(var entries in parsedElement.GetProperty("entries").EnumerateArray())
            {
                if (entries.TryGetProperty("playerPoolEntry", out JsonElement playerPoolEntryElement))
                {
                    var playerElement = playerPoolEntryElement.GetProperty("player");
                    var eligibleSlots = playerElement.GetProperty("eligibleSlots").EnumerateArray();
                    List<int> playerPositions = new List<int>();

                    foreach(var slot in eligibleSlots)
                    {
                        playerPositions.Add(slot.GetInt32());
                    }

                    roster[playerElement.GetProperty("fullName").GetString()] = playerPositions;
                }
            }
        }

        return roster;
    }
}