namespace EZMap.Configuration
{
    internal record AutoMapperTargetMemberConfiguration(bool Inject, Func<object>? DefaultValueFactory);
}
