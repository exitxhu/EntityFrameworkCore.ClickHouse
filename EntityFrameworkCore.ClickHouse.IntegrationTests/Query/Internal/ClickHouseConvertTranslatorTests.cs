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
public class ClickHouseConvertTranslatorTests : DatabaseFixture
{
    #region Convert.ToXX()
    
    [Test]
    public void ToInt8()
    {
        // Arrange
        
        // Act
        var v = Context.SimpleEntities.Select(e => Convert.ToSByte(e.Int8AsString)).Single();

        // Assert
        v.Should().Be(SimpleEntity.Int8Value);
    }
    
    [Test]
    public void ToInt16()
    {
        // Arrange
        
        // Act
        var v = Context.SimpleEntities.Select(e => Convert.ToInt16(e.Int16AsString)).Single();

        // Assert
        v.Should().Be(SimpleEntity.Int16Value);
    }
    
    [Test]
    public void ToInt32()
    {
        // Arrange
        
        // Act
        var v = Context.SimpleEntities.Select(e => Convert.ToInt32(e.Int32AsString)).Single();

        // Assert
        v.Should().Be(SimpleEntity.Int32Value);
    }
    
    [Test]
    public void ToInt64()
    {
        // Arrange
        
        // Act
        var v = Context.SimpleEntities.Select(e => Convert.ToInt64(e.Int64AsString)).Single();

        // Assert
        v.Should().Be(SimpleEntity.Int64Value);
    }
    
    [Test]
    public void ToUInt8()
    {
        // Arrange
        
        // Act
        var v = Context.SimpleEntities.Select(e => Convert.ToByte(e.UInt8AsString)).Single();

        // Assert
        v.Should().Be(SimpleEntity.UInt8Value);
    }
    
    [Test]
    public void ToUInt16()
    {
        // Arrange
        
        // Act
        var v = Context.SimpleEntities.Select(e => Convert.ToUInt16(e.UInt16AsString)).Single();

        // Assert
        v.Should().Be(SimpleEntity.UInt16Value);
    }
    
    [Test]
    public void ToUInt32()
    {
        // Arrange

        // Act
        var v = Context.SimpleEntities.Select(e => Convert.ToUInt32(e.UInt32AsString)).Single();

        // Assert
        v.Should().Be(SimpleEntity.UInt32Value);
    }
    
    [Test]
    public void ToUInt64()
    {
        // Arrange
        
        // Act
        var v = Context.SimpleEntities.Select(e => Convert.ToUInt64(e.UInt64AsString)).Single();

        // Assert
        v.Should().Be(SimpleEntity.UInt64Value);
    }
    
    [Test]
    public void ToFloat32()
    {
        // Arrange

        // Act
        var v = Context.SimpleEntities.Select(e => Convert.ToSingle(e.FloatAsString)).Single();

        // Assert
        v.Should().Be(SimpleEntity.FloatValue);
    }

    [Test]
    public void ToFloat64()
    {
        // Arrange

        // Act
        var v = Context.SimpleEntities.Select(e => Convert.ToDouble(e.DoubleAsString)).Single();

        // Assert
        v.Should().Be(SimpleEntity.DoubleValue);
    }

    [Test]
    public void ToDateTime()
    {
        // Arrange

        // Act
        var v = Context.SimpleEntities.Select(e => Convert.ToDateTime(e.DateTimeAsString)).Single();

        // Assert
        v.Should().Be(SimpleEntity.DateTimeValue);
    }

    #endregion

    #region Type.Parse()

    [Test]
    public void Int8Parse()
    {
        // Arrange
        
        // Act
        var v = Context.SimpleEntities.Select(e => sbyte.Parse(e.Int8AsString)).Single();

        // Assert
        v.Should().Be(SimpleEntity.Int8Value);
    }
    
    [Test]
    public void Int16Parse()
    {
        // Arrange

        // Act
        var v = Context.SimpleEntities.Select(e => short.Parse(e.Int16AsString)).Single();

        // Assert
        v.Should().Be(SimpleEntity.Int16Value);
    }

    [Test]
    public void Int32Parse()
    {
        // Arrange
        
        // Act
        var v = Context.SimpleEntities.Select(e => int.Parse(e.Int32AsString)).Single();

        // Assert
        v.Should().Be(SimpleEntity.Int32Value);
    }
    
    [Test]
    public void Int64Parse()
    {
        // Arrange
        
        // Act
        var v = Context.SimpleEntities.Select(e => long.Parse(e.Int64AsString)).Single();

        // Assert
        v.Should().Be(SimpleEntity.Int64Value);
    }
    
    [Test]
    public void UInt8Parse()
    {
        // Arrange
        
        // Act
        var v = Context.SimpleEntities.Select(e => byte.Parse(e.UInt8AsString)).Single();

        // Assert
        v.Should().Be(SimpleEntity.UInt8Value);
    }
    
    [Test]
    public void UInt16Parse()
    {
        // Arrange

        // Act
        var v = Context.SimpleEntities.Select(e => ushort.Parse(e.UInt16AsString)).Single();

        // Assert
        v.Should().Be(SimpleEntity.UInt16Value);
    }
    
    [Test]
    public void UInt32Parse()
    {
        // Arrange
        
        // Act
        var v = Context.SimpleEntities.Select(e => uint.Parse(e.UInt32AsString)).Single();

        // Assert
        v.Should().Be(SimpleEntity.UInt32Value);
    }
    
    [Test]
    public void UInt64Parse()
    {
        // Arrange
        
        // Act
        var v = Context.SimpleEntities.Select(e => ulong.Parse(e.UInt64AsString)).Single();

        // Assert
        v.Should().Be(SimpleEntity.UInt64Value);
    }
    
    [Test]
    public void FloatParse()
    {
        // Arrange
        
        // Act
        var v = Context.SimpleEntities.Select(e => float.Parse(e.FloatAsString)).Single();

        // Assert
        v.Should().Be(SimpleEntity.FloatValue);
    }

    [Test]
    public void DoubleParse()
    {
        // Arrange
        
        // Act
        var v = Context.SimpleEntities.Select(e => double.Parse(e.DoubleAsString)).Single();

        // Assert
        v.Should().Be(SimpleEntity.DoubleValue);
    }

    [Test]
    public void DateTimeParse()
    {
        // Arrange

        // Act
        var v = Context.SimpleEntities.Select(e => DateTime.Parse(e.DateTimeAsString)).Single();

        // Assert
        v.Should().Be(SimpleEntity.DateTimeValue);
    }

    #endregion
}
