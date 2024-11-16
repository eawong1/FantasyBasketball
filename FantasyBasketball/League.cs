using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using Avalonia.Markup.Xaml.Templates;
using Microsoft.VisualBasic;

public class League
{
    private string? m_leagueId;
    private string? m_leagueYear;
    private string? m_swid;
    private string? m_espn;
    private Dictionary<string, object> m_responseData;

    //$"https://lm-api-reads.fantasy.espn.com/apis/v3/games/fba/seasons/2025/segments/0/leagues/1282650304";
    private const string BASE_ENDPOINT = $"https://lm-api-reads.fantasy.espn.com/apis/v3/games/fba/seasons";

    public League(string? leagueId, string? leagueYear, string? swid, string? espn)
    {
        m_leagueId = leagueId;
        m_leagueYear = leagueYear;
        m_swid = swid;
        m_espn = espn;

        Login();
    }

    public async void Login()
    {
        if(m_leagueId == null || m_leagueYear == null)
        {
            throw new Exception("League ID and League Year cannot be empty.");
        }
        // var url = BASE_ENDPOINT + string.Format("/{0}/segments/0/leagues/{1}", m_leagueYear, m_leagueId);
        var url = BASE_ENDPOINT + $"/{m_leagueYear}/segments/0/leagues/{m_leagueId}?view=mTeam&view=mRoster&view=mMatchup&view=mSettings&view=mStandings";

        var handler = new HttpClientHandler
        {
            CookieContainer = new CookieContainer()
        };

        // Add cookies to the handler's CookieContainer
        handler.CookieContainer.Add(new Uri(url), new Cookie("swid", m_swid));
        handler.CookieContainer.Add(new Uri(url), new Cookie("espn_s2", m_espn));

        // Create an HttpClient instance
        using (HttpClient client = new HttpClient(handler))
        {
            try
            {
                // Make the GET request
                HttpResponseMessage response = await client.GetAsync(url);

                // Check if the request was successful
                if (response.IsSuccessStatusCode)
                {
                    // Read the response content as a string
                    var responseData = await response.Content.ReadAsStringAsync();
                    // var responseData = await response.Content.ReadFromJsonAsync();
                    //keys are draftDetail, gameId, id, members, schedule, scoringPeriodId, seasonId, segmentId, settings, status, teams
                    m_responseData = JsonSerializer.Deserialize<Dictionary<string, object>>(responseData);
                    
                    var temp = m_responseData["teams"];
                    File.AppendAllText("temp.txt", temp.ToString());

                }
                else
                {
                    throw new Exception("ResponseData Failed");
                }
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}