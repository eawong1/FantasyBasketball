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
using Microsoft.VisualBasic;
using Utilities;

public class League
{
    private static Dictionary<string, JsonElement> m_responseData;
    private static List<Dictionary<string, object>>? m_teams;

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

    public List<string> GetRoster(string teamName)
    {
        
        List<string> roster = new List<string>();

        //! something is wrong with this filtering logic below
        var temp = m_teams.FirstOrDefault(dict => dict["name"].ToString() == teamName);

        return roster;
    }
}