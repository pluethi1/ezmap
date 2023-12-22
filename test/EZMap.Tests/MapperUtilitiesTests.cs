using EZMap.Tests.Models;
using EZMap.Utilities;

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
}