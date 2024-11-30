using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using Avalonia.Markup.Xaml.Templates;
using Microsoft.VisualBasic;
using Utilities;

public class League
{
    private static Dictionary<string, JsonElement> m_responseData;

    public League(Dictionary<string, JsonElement> responseData)
    {
        m_responseData = responseData;
    }

    public List<string> GetTeamNames()
    {
        List<string> teams = new List<string>();
        
        var listTeams = UtilityFunctions.JsonElementToListOfObjects(m_responseData["teams"]);
        foreach(var team in listTeams)
        {
            teams.Add(team["name"].ToString());
        }

        return teams;
    }
}