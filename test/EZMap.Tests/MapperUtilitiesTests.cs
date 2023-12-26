using EZMap.Tests.Models;
using EZMap.Tests.Services;
using EZMap.Utilities;
using Microsoft.Extensions.DependencyInjection;
using System.Linq.Expressions;

namespace EZMap.Tests;

public class MapperUtilitiesTests
{
    [Fact]
    public void Should_ReturnAllReadablePropertiesAndFields()
    {
        var type = typeof(SampleSourceType);

        var members = MapperUtilities.GetReadableFieldsAndProperties(type).Select(x => x.Name);
        Assert.Contains(nameof(SampleSourceType.Id), members);
        Assert.Contains(nameof(SampleSourceType.Name), members);
        Assert.Contains(nameof(SampleSourceType.Description), members);
        Assert.Contains(nameof(SampleSourceType.Age), members);
        Assert.DoesNotContain(nameof(SampleSourceType.MY_SUPER_CONSTANT), members);
        
    }

    [Fact]
    public void Should_ReturnAllWriteablePropertiesAndFields()
    {
        var type = typeof(SampleTargetType);

        var members = MapperUtilities.GetWriteableFieldsAndProperties(type).Select(x => x.Name);
        Assert.Contains(nameof(SampleTargetType.Id), members);
        Assert.Contains(nameof(SampleTargetType.Name), members);
        Assert.Contains(nameof(SampleTargetType.Description), members);
        Assert.DoesNotContain(nameof(SampleTargetType.Age), members);
        Assert.DoesNotContain(nameof(SampleTargetType.MY_SECRET_CONSTANT), members);
    }

    [Fact]
    public void Should_ReturnAllMatchingPropertiesAndFields()
    {
        var sourceType = typeof(SampleSourceType);
        var targetType = typeof(SampleTargetType);

        var members = MapperUtilities.GetMatchingFieldsAndProperties(sourceType, targetType).Select(x => x.Name);
        Assert.Contains(nameof(SampleSourceType.Id), members);
        Assert.Contains(nameof(SampleSourceType.Name), members);
        Assert.Contains(nameof(SampleSourceType.Description), members);
        Assert.DoesNotContain(nameof(SampleSourceType.Age), members);
        Assert.DoesNotContain(nameof(SampleSourceType.MY_SUPER_CONSTANT), members);
    }

    [Fact]
    public void Should_CreateInjectedObjectCorrectly()
    {
        var provider = TestUtilities.CreateDefaultServiceProvider();

        var @object = (SampleInjectableType)MapperUtilities.CreateObject(typeof(SampleInjectableType), provider);
        Assert.Equal(20, @object.Age);
        Assert.Equal("Peter", @object.Name);
    }

    [Fact]
    public void Should_NotThrowIfEqualType()
    {
        Assert.Null(Record.Exception(() => MapperUtilities.AssertEqualType(typeof(string), typeof(string), "parameter")));
        Assert.Null(Record.Exception(() => MapperUtilities.AssertEqualType<string>(typeof(string), "parameter")));
    }

    [Fact]
    public void Should_ThrowIfUnequalType()
    {
        Assert.Throws<ArgumentException>(() => MapperUtilities.AssertEqualType(typeof(string), typeof(int), "parameter"));
        Assert.Throws<ArgumentException>(() => MapperUtilities.AssertEqualType<string>(typeof(int), "parameter"));
    }

    [Theory]
    [InlineData(typeof(bool))]
    [InlineData(typeof(byte))]
    [InlineData(typeof(sbyte))]
    [InlineData(typeof(ushort))]
    [InlineData(typeof(short))]
    [InlineData(typeof(uint))]
    [InlineData(typeof(int))]
    [InlineData(typeof(ulong))]
    [InlineData(typeof(long))]
    [InlineData(typeof(float))]
    [InlineData(typeof(double))]
    [InlineData(typeof(decimal))]
    [InlineData(typeof(string))]
    [InlineData(typeof(DateTime))]
    public void Should_DetectPrimitiveType(Type type)
    {
       Assert.Null(Record.Exception(() => MapperUtilities.IsPrimitiveType(type)));
    }

    [Fact]
    public void Should_GetCorrectSourceMemberNameFromExpression()
    {
        Expression<Func<SampleSourceType, string>> expression = s => s.Name;
        string? memberName = null;
        Assert.Null(Record.Exception(() => memberName = MapperUtilities.AssertValidMemberAndGetName(expression, true, "parameter")));
        Assert.NotNull(memberName);
        Assert.Equal(nameof(SampleSourceType.Name), memberName);
    }

    [Fact]
    public void Should_GetCorrectTargetMemberNameFromExpression()
    {
        Expression<Func<SampleTargetType, string>> expression = t => t.Name;
        string? memberName = null;
        Assert.Null(Record.Exception(() => memberName = MapperUtilities.AssertValidMemberAndGetName(expression, false, "parameter")));
        Assert.NotNull(memberName);
        Assert.Equal(nameof(SampleTargetType.Name), memberName);
    }

    [Fact]
    public void ShouldThrowIfInvalidTargetMemberNameInExpression()
    {
        Expression<Func<SampleTargetType, int>> expression = t => t.Age;
        Assert.Throws<ArgumentException>(() => _ = MapperUtilities.AssertValidMemberAndGetName(expression, false, "parameter"));
        
    }
}