using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace EntityFrameworkCore.ClickHouse.IntegrationTests.Query.Internal;

[TestFixture, ExcludeFromCodeCoverage]
public class ClickHouseQuerySqlGeneratorTests : DatabaseFixture
{
    [Test]
    public async Task VisitSqlParameter_FilteredQuery_ShouldAppendParameter()
    {
        // Arrange
        var s = "Lorem ipsum";

        // Act
        var items = await Context.SimpleEntities.Where(e => e.Text == s).ToArrayAsync();

        // Assert
        items.Should().HaveCount(1);
    }
}
