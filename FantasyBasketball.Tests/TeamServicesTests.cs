using System.Collections.Generic;
using NUnit.Framework;
using NSubstitute;

namespace Utilities.Tests
{
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
        public void GetPositions_ShouldReturnCorrectPositions_WhenRosterIsValid()
        {
            // Arrange
            var roster = new List<Player>
            {
                new Player("Player1", new List<string> { "PG", "SG" }),
                new Player("Player2", new List<string> { "SF", "PF" }),
                new Player("Player3", new List<string> { "C", "UTIL" })
            };

            // Act
            var result = _teamServices.GetPositions(roster);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.ContainsKey("PG"), Is.True);
            Assert.That(result.ContainsKey("SG"), Is.True);
            Assert.That(result.ContainsKey("SF"), Is.True);
            Assert.That(result.ContainsKey("PF"), Is.True);
            Assert.That(result.ContainsKey("C"), Is.True);
            Assert.That(result.ContainsKey("UTIL"), Is.False);
        }

        [Test]
        public void GetPositions_ShouldReturnEmptyDictionary_WhenRosterIsNull()
        {
            // Act
            var result = _teamServices.GetPositions(null);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.Empty);
        }

        [Test]
        public void GetPositions_ShouldReturnEmptyDictionary_WhenRosterIsEmpty()
        {
            // Arrange
            var roster = new List<Player>();

            // Act
            var result = _teamServices.GetPositions(roster);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.Empty);
        }
    }
}