using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Text.Json;
using System.Threading.Tasks;

namespace Utilities;

public class UtilityFunctions : IUtilityFunctions
{
    private const string BASE_ENDPOINT = $"https://lm-api-reads.fantasy.espn.com/apis/v3/games/fba/seasons";

    public List<Dictionary<string, object>> JsonElementToListOfObjects(JsonElement element)
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

    public string GetStringPositions(int position)
    {
        switch(position)
        {
            case 0:
                return "PG";
            case 1:
                return "SG";
            case 2:
                return "SF";
            case 3:
                return "PF";
            case 4:
                return "C";
            case 5:
                return "G";
            case 6:
                return "F";
            case 7:
                return "UTIL";
            case 8:
                return "UTIL";
            case 9:
                return "UTIL";
            case 10:
                return "BE";
            case 11:
                return "BE";
            case 12:
                return "BE";
            case 13:
                return "IR";
            default:
                return "";
        }
    }

    public static async Task<Dictionary<string, JsonElement>> Login(string? leagueId, string? leagueYear, string? swid, string? espn)
    {
        Dictionary<string, JsonElement> responseDataDict;

        if (leagueId == null || leagueYear == null)
        {
            throw new Exception("League ID and League Year cannot be empty.");
        }

        var url = BASE_ENDPOINT + $"/{leagueYear}/segments/0/leagues/{leagueId}?view=mTeam&view=mRoster&view=mMatchup&view=mSettings&view=mStandings";

        var handler = new HttpClientHandler
        {
            CookieContainer = new CookieContainer()
        };

        // Add cookies to the handler's CookieContainer
        handler.CookieContainer.Add(new Uri(url), new Cookie("swid", swid));
        handler.CookieContainer.Add(new Uri(url), new Cookie("espn_s2", espn));

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
                    //Read the response content as a string
                    var responseData = await response.Content.ReadAsStringAsync();

                    //keys are draftDetail, gameId, id, members, schedule, scoringPeriodId, seasonId, segmentId, settings, status, teams
                    responseDataDict = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(responseData);
                }
                else
                {
                    throw new Exception("ResponseData Failed");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        return responseDataDict;
    }
}


