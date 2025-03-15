using System;
using System.Collections.Generic;
using System.Net;
using System.IO;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using NJsonSchema;
using System.Xml.Schema;

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

    public void ParseJsonLogin(string jsonPath, out string leagueId, out string leagueYear, out string? swid, out string? espnS2)
    {
        if (!File.Exists(jsonPath))
        {
            throw new FileNotFoundException($"The file at path {jsonPath} was not found.");
        }

        using (StreamReader reader = new StreamReader(jsonPath))
        {
            string jsonString = reader.ReadToEnd();
            using JsonDocument doc = JsonDocument.Parse(jsonString);
            JsonElement root = doc.RootElement;

            leagueId = root.GetProperty("leagueId").GetString();
            leagueYear = root.GetProperty("seasonId").GetString();
            if (root.TryGetProperty("swid", out JsonElement swidElement) && root.TryGetProperty("espnS2", out JsonElement espnS2Element))
            {
                swid = root.GetProperty("swid").GetString();
                espnS2 = root.GetProperty("espnS2").GetString();
            }
            else
            {
                swid = null;
                espnS2 = null;
            }
        }
    }

    //TODO: finish fixing this method
    public void CheckLoginJsonSchema(string jsonPath, string? schemaPath)
    {
        if (!File.Exists(jsonPath))
        {
            throw new FileNotFoundException($"The file at path {jsonPath} was not found.");
        }

        if(schemaPath == null)
        {
            schemaPath = Path.Combine("/home/hobble/Documents/FantasyBasketball/FantasyBasketball", "Data", "Login.schema.json");
        }

        if(!File.Exists(schemaPath))
        {
            throw new FileNotFoundException($"The schema file at path {schemaPath} was not found.");
        }

        string jsonString;
        using (StreamReader reader = new StreamReader(jsonPath))
        {
            jsonString = reader.ReadToEnd();
        }

        string schemaString;
        using (StreamReader reader = new StreamReader(schemaPath))
        {
            schemaString = reader.ReadToEnd();
        }

        var schema = JsonSchema.FromJsonAsync(schemaString).Result;
        var errors = schema.Validate(jsonString);

        if (errors.Count > 0)
        {
            throw new Exception("JSON validation failed: " + string.Join(", ", errors));
        }
    }

    public static async Task<Dictionary<string, JsonElement>> Login(string? leagueId, string? leagueYear, string? swid, string? espn, HttpClient? client = null)
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
        client ??= new HttpClient(handler);
        
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

        return responseDataDict;
    }
}


