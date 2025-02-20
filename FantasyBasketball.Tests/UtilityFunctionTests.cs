using NUnit.Framework;
using System.Collections.Generic;
using System.Text.Json;
using System.Net;
using Utilities;
using NSubstitute;

namespace FantasyBasketball.Tests;

public class MockHttpMessageHandler : HttpMessageHandler
{
    public HttpResponseMessage ResponseMessage { get; set; }
    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, System.Threading.CancellationToken cancellationToken)
    {
        return Task.FromResult(ResponseMessage);
    }
}

[TestFixture]
public class UtilityFunctionsTests
{
    private UtilityFunctions _utilityFunctions;
    private MockHttpMessageHandler _mockHandler;
    private HttpClient _mockClient;

    [SetUp]
    public void SetUp()
    {
        _utilityFunctions = new UtilityFunctions();

        _mockHandler = Substitute.For<MockHttpMessageHandler>();
        _mockClient = new HttpClient(_mockHandler);
    }

    [TearDown]
    public void TearDown()
    {
        _mockClient.Dispose();
        _mockHandler.Dispose();
    }

    [Test]
    public void UtilityFunctions_JsonElementToListOfObjects_ShouldReturnExpectedList()
    {
        var jsonElement = JsonDocument.Parse("[{\"name\": \"TeamName\"}, {\"name\": \"TeamName2\"}]").RootElement;
        var expectedList = new List<Dictionary<string, object>>
        {
            new Dictionary<string, object> { { "name", "TeamName" } },
            new Dictionary<string, object> { { "name", "TeamName2" } }
        };

        var result = _utilityFunctions.JsonElementToListOfObjects(jsonElement);

        Assert.That(result, Is.EqualTo(expectedList));
    }

    [Test]
    public void UtilityFunctions_GetStringPositions_ShouldReturnExpectedPosition()
    {
        var positionMapping = new Dictionary<int, string>
        {
            { 0, "PG" },
            { 1, "SG" },
            { 2, "SF" },
            { 3, "PF" },
            { 4, "C" },
            { 5, "G" },
            { 6, "F" },
            { 7, "UTIL" },
            { 8, "UTIL" },
            { 9, "UTIL" },
            { 10, "BE" },
            { 11, "BE" },
            { 12, "BE" },
            { 13, "IR" }
        };

        foreach (var pos in positionMapping)
        {
            var result = _utilityFunctions.GetStringPositions(pos.Key);

            Assert.That(result, Is.EqualTo(pos.Value));
        }
    }

    [Test]
    public async Task Login_ShouldThrowException_WhenLeagueIdIsNull()
    {
        // Arrange
        string leagueId = null;
        string leagueYear = "2023";
        string swid = "some_swid";
        string espn = "some_espn";

        // Act & Assert
        var ex = Assert.ThrowsAsync<Exception>(async () => await UtilityFunctions.Login(leagueId, leagueYear, swid, espn, _mockClient));
        Assert.That(ex.Message, Is.EqualTo("League ID and League Year cannot be empty."));
    }

    [Test]
    public async Task Login_ShouldThrowException_WhenLeagueYearIsNull()
    {
        // Arrange
        string leagueId = "12345";
        string leagueYear = null;
        string swid = "some_swid";
        string espn = "some_espn";

        // Act & Assert
        var ex = Assert.ThrowsAsync<Exception>(async () => await UtilityFunctions.Login(leagueId, leagueYear, swid, espn, _mockClient));
        Assert.That(ex.Message, Is.EqualTo("League ID and League Year cannot be empty."));
    }

    [Test]
    public async Task Login_ShouldReturnResponseData_WhenRequestIsSuccessful()
    {
        // Arrange
        string leagueId = "12345";
        string leagueYear = "2023";
        string swid = "some_swid";
        string espn = "some_espn";
        // var url = $"https://lm-api-reads.fantasy.espn.com/apis/v3/games/fba/seasons/{leagueYear}/segments/0/leagues/{leagueId}?view=mTeam&view=mRoster&view=mMatchup&view=mSettings&view=mStandings";

        var responseData = new Dictionary<string, JsonElement>
        {
            { "key1", JsonDocument.Parse("\"value1\"").RootElement },
            { "key2", JsonDocument.Parse("\"value2\"").RootElement }
        };

        var responseMessage = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(JsonSerializer.Serialize(responseData))
        };

        _mockHandler.ResponseMessage = responseMessage;

        // Act
        var result = await UtilityFunctions.Login(leagueId, leagueYear, swid, espn, _mockClient);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result["key1"].GetString(), Is.EqualTo("value1"));
        Assert.That(result["key2"].GetString(), Is.EqualTo("value2"));
    }

    [Test]
    public async Task Login_ShouldThrowException_WhenRequestFails()
    {
        // Arrange
        string leagueId = "12345";
        string leagueYear = "2023";
        string swid = "some_swid";
        string espn = "some_espn";
        var url = $"https://lm-api-reads.fantasy.espn.com/apis/v3/games/fba/seasons/{leagueYear}/segments/0/leagues/{leagueId}?view=mTeam&view=mRoster&view=mMatchup&view=mSettings&view=mStandings";

        var responseMessage = new HttpResponseMessage(HttpStatusCode.BadRequest);

        // _mockHandler.SendAsync(Arg.Any<HttpRequestMessage>()).Returns(Task.FromResult(responseMessage));
        _mockHandler.ResponseMessage = responseMessage;

        // Act & Assert
        var ex = Assert.ThrowsAsync<Exception>(async () => await UtilityFunctions.Login(leagueId, leagueYear, swid, espn, _mockClient));
        Assert.That(ex.Message, Is.EqualTo("ResponseData Failed"));
    }
}
