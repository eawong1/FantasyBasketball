using System.Collections.Generic;
using System.Text.Json;
using NSubstitute;
using NUnit.Framework;
using Utilities;

namespace FantasyBasketball.Tests;

[TestFixture]
public class TeamServicesTests
{
    private TeamServices _teamServices;
    private IUtilityFunctions _mockUtilityFunctions;

    [SetUp]
    public void SetUp()
    {
        _mockUtilityFunctions = Substitute.For<IUtilityFunctions>();
        _teamServices = new TeamServices(_mockUtilityFunctions);
    }

    [Test]
    public void TeamServices_GetTeams_ShouldReturnExpectedTeams()
    {
        var jsonElement = JsonDocument.Parse("[{\"name\": \"TeamName\"}, {\"name\": \"TeamName2\"}]").RootElement;
        var expectedTeams = new List<Dictionary<string, object>>
        {
            new Dictionary<string, object> { { "name", "TeamName" } },
            new Dictionary<string, object> { { "name", "TeamName2" } }
        };

        _mockUtilityFunctions.JsonElementToListOfObjects(jsonElement).Returns(expectedTeams);

        var teams = _teamServices.GetTeams(jsonElement);

        Assert.That(teams, Is.EquivalentTo(expectedTeams));
    }

    [Test]
    public void TeamServices_GetPositions_ShouldReturnCorrectPositions_WhenRosterIsValid()
    {
        var roster = new List<Player>
        {
            new Player("Player1", new List<string> { "PG", "SG" }),
            new Player("Player2", new List<string> { "SF", "PG" }),
            new Player("Player3", new List<string> { "C", "UTIL", "IR", "BE" })
        };

        var result = _teamServices.GetPositions(roster);

        Assert.That(result, Is.Not.Null);
        Assert.That(result.ContainsKey("UTIL"), Is.False);

        Assert.That(result["PG"].Count, Is.EqualTo(2));
        Assert.That(result["SG"].Count, Is.EqualTo(1));
        Assert.That(result["SF"].Count, Is.EqualTo(1));
        Assert.That(result["C"].Count, Is.EqualTo(1));

        Assert.That(result["PG"], Has.Exactly(1).Matches<Player>(p => p.GetName() == "Player1"));
        Assert.That(result["PG"], Has.Exactly(1).Matches<Player>(p => p.GetName() == "Player2"));
        Assert.That(result["SG"], Has.Exactly(1).Matches<Player>(p => p.GetName() == "Player1"));
        Assert.That(result["SF"], Has.Exactly(1).Matches<Player>(p => p.GetName() == "Player2"));
        Assert.That(result["C"], Has.Exactly(1).Matches<Player>(p => p.GetName() == "Player3"));
    }

    [Test]
    public void TeamServices_GetPositions_ShouldReturnEmptyDictionary_WhenRosterIsEmpty()
    {
        var roster = new List<Player>();

        var result = _teamServices.GetPositions(roster);

        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.Empty);
    }
}
