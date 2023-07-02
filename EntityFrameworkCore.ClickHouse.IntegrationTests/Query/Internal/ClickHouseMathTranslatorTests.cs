using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using ClickHouse.EntityFrameworkCore.Extensions;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System.Diagnostics.CodeAnalysis;

namespace EntityFrameworkCore.ClickHouse.IntegrationTests.Query.Internal;

[TestFixture, ExcludeFromCodeCoverage]
public class ClickHouseMathTranslatorTests : DatabaseFixture
{
    [Test]
    public void Exp()
    {
        // Arrange
        const double v = 1;

        // Act
        var exp = Context.SimpleEntities.Select(e => Math.Exp(v)).First();

        // Assert
        exp.Should().BeApproximately(Math.Exp(v), 0.0001);
    }

    [Test]
    public void Log()
    {
        // Arrange
        const double v = 2;

        // Act
        var log = Context.SimpleEntities.Select(e => Math.Log(v)).First();

        // Assert
        log.Should().BeApproximately(Math.Log(v), 0.0001);
    }
    
    [Test]
    public void Log10()
    {
        // Arrange
        const double v = 3;

        // Act
        var log10 = Context.SimpleEntities.Select(e => Math.Log10(v)).First();

        // Assert
        log10.Should().BeApproximately(Math.Log10(v), 0.0001);
    }
    
    [Test]
    public void Sqrt()
    {
        // Arrange
        const double v = 4;

        // Act
        var sqrt = Context.SimpleEntities.Select(e => Math.Sqrt(v)).First();

        // Assert
        sqrt.Should().BeApproximately(Math.Sqrt(v), 0.0001);
    }
    
    [Test]
    public void Cbrt()
    {
        // Arrange
        const double v = 5;

        // Act
        var cqrt = Context.SimpleEntities.Select(e => Math.Cbrt(v)).First();

        // Assert
        cqrt.Should().BeApproximately(Math.Cbrt(v), 0.0001);
    }
    
    [Test]
    public void Sin()
    {
        // Arrange
        const double v = 6;

        // Act
        var sin = Context.SimpleEntities.Select(e => Math.Sin(v)).First();

        // Assert
        sin.Should().BeApproximately(Math.Sin(v), 0.0001);
    }
    
    [Test]
    public void Cos()
    {
        // Arrange
        const double v = 7;

        // Act
        var cos = Context.SimpleEntities.Select(e => Math.Cos(v)).First();

        // Assert
        cos.Should().BeApproximately(Math.Cos(v), 0.0001);
    }
    
    [Test]
    public void Tan()
    {
        // Arrange
        const double v = 8;

        // Act
        var tan = Context.SimpleEntities.Select(e => Math.Tan(v)).First();

        // Assert
        tan.Should().BeApproximately(Math.Tan(v), 0.0001);
    }
    
    [Test]
    public void Asin()
    {
        // Arrange
        const double v = 0.6;

        // Act
        var asin = Context.SimpleEntities.Select(e => Math.Asin(v)).First();

        // Assert
        asin.Should().BeApproximately(Math.Asin(v), 0.0001);
    }
    
    [Test]
    public void Acos()
    {
        // Arrange
        const double v = 0.5;

        // Act
        var acos = Context.SimpleEntities.Select(e => Math.Acos(v)).First();

        // Assert
        acos.Should().BeApproximately(Math.Acos(v), 0.0001);
    }
    
    [Test]
    public void Atan()
    {
        // Arrange
        const double v = 11;

        // Act
        var atan = Context.SimpleEntities.Select(e => Math.Atan(v)).First();

        // Assert
        atan.Should().BeApproximately(Math.Atan(v), 0.0001);
    }
    
    [Test]
    public void Pow()
    {
        // Arrange
        const double v = 12;

        // Act
        var pow = Context.SimpleEntities.Select(e => Math.Pow(v, 3)).First();

        // Assert
        pow.Should().BeApproximately(Math.Pow(v, 3), 0.0001);
    }
}
