using System.Collections.Generic;
using System.Text.Json;
using NSubstitute;
using NUnit.Framework;
using Utilities;

namespace FantasyBasketball.Tests;

[TestFixture]
public class LeagueTests
{
    private IUtilityFunctions _utilities;
    private ITeamServices _teamServices;
    [SetUp]
    public void Setup()
    {
        _utilities = Substitute.For<IUtilityFunctions>();
        _teamServices = Substitute.For<ITeamServices>();
    }
    [TearDown]
    public void Teardown()
    {
        _utilities = null;
        _teamServices = null;
    }

    [Test]
    public void League_TestGetTeams()
    {
        var responseData = new Dictionary<string, JsonElement> 
        { 
            { "teams", JsonDocument.Parse("[{\"name\": \"TeamName\"}, {\"name\": \"TeamName2\"}]").RootElement } 
        };
        var returnedTeams = new List<Dictionary<string, object>> 
        { 
            new Dictionary<string, object> { { "name", "TeamName" } }, 
            new Dictionary<string, object> { { "name", "TeamName2" } } 
        };

        _teamServices.GetTeams(responseData["teams"]).Returns(returnedTeams);

        var league = new League(responseData, _utilities, _teamServices);
        
        var teamNames = league.GetTeamNames();

        var expectedTeamNames = new List<string> {"TeamName", "TeamName2"};
        Assert.That(expectedTeamNames, Is.EquivalentTo(teamNames));
    }

    [Test]
    public void League_TestGetRoster()
    {
        // create a Team A and a roster with one player in positions PG and SG
        var teamName = "Team A";
        var expectedRoster = new List<Player>
        {
            new Player("Player 1", new List<string> { "PG", "SG" })
        };

        //creating and injecting an mock teams list structure
        var returnedTeams = new List<Dictionary<string, object>>
        {
            new Dictionary<string, object>
            {
                { "name", teamName },
                { "roster", "{\"entries\":[{\"playerPoolEntry\":{\"player\":{\"fullName\":\"Player 1\",\"eligibleSlots\":[0,1]}}}]}" }
            }
        };

        var responseData = new Dictionary<string, JsonElement> { { "teams", JsonDocument.Parse("[{\"name\": \"TeamName\"}, {\"name\": \"TeamName2\"}]").RootElement } };

        _teamServices.GetTeams(responseData["teams"]).Returns(returnedTeams);

        _utilities.GetStringPositions(0).Returns("PG");
        _utilities.GetStringPositions(1).Returns("SG");

        var league = new League(responseData, _utilities, _teamServices);

        var roster = league.GetRoster(teamName);

        Assert.That(roster.Count, Is.EqualTo(expectedRoster.Count));
        Assert.That(roster[0].GetName(), Is.EqualTo(expectedRoster[0].GetName()));
        Assert.That(roster[0].GetEligiblePositions(), Is.EquivalentTo(expectedRoster[0].GetEligiblePositions()));
    }
}
