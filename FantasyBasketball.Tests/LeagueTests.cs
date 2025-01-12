using System.Collections.Generic;
using System.Text.Json;
using NSubstitute;
using NUnit.Framework;
using Utilities;

namespace FantasyBasketball.Tests;

[TestFixture]
public class Tests
{
    private IUtilityFunctions _utilities;
    [SetUp]
    public void Setup()
    {
        _utilities = Substitute.For<IUtilityFunctions>();
    }

    [Test]
    public void League_TestGetTeams()
    {
        var responseData = new Dictionary<string, JsonElement> { { "teams", JsonDocument.Parse("[{\"name\": \"TeamName\"}, {\"name\": \"TeamName2\"}]").RootElement } };
        var league = new League(responseData, _utilities);

        // var returnedTeams = new List<string> {"TeamName"};
        var returnedTeams = new List<Dictionary<string, object>> { new Dictionary<string, object> { { "name", "TeamName" } }, new Dictionary<string, object> { { "name", "TeamName2" } } };

        _utilities.JsonElementToListOfObjects(responseData["teams"]).Returns(returnedTeams);
        
        var teamNames = league.GetTeamNames();

        var expectedTeamNames = new List<string> {"TeamName", "TeamName2"};
        Assert.That(expectedTeamNames, Is.EquivalentTo(teamNames));
    }
}
