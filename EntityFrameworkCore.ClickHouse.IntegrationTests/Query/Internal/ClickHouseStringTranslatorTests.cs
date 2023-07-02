using System.Linq;
using FluentAssertions;
using NUnit.Framework;

namespace EntityFrameworkCore.ClickHouse.IntegrationTests.Query.Internal;

[TestFixture]
public class ClickHouseStringTranslatorTests : DatabaseFixture
{
    [Test]
    public void StartsWith()
    {
        // Arrange
        var lorem = "Lorem";

        // Act

        // Assert
        Context.SimpleEntities.Any(e => e.Text.StartsWith(lorem)).Should().BeTrue();
    }

    [Test]
    public void EndsWith()
    {
        // Arrange
        var ipsum = "ipsum";

        // Act

        // Assert
        Context.SimpleEntities.Any(e => e.Text.EndsWith(ipsum)).Should().BeTrue();
    }
}
