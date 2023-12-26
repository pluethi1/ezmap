namespace EZMap.Configuration
{
    internal class AutoMapperConfiguration
    {
        internal AutoMapperConfiguration(AutoMapperMemberConfigurationCollection<AutoMapperSourceMemberConfiguration> sourceMemberConfigurations,
                                         AutoMapperMemberConfigurationCollection<AutoMapperTargetMemberConfiguration> targetMemberConfigurations)
        {
            SourceMemberConfigurations = sourceMemberConfigurations;
            TargetMemberConfigurations = targetMemberConfigurations;
        }

        internal AutoMapperMemberConfigurationCollection<AutoMapperSourceMemberConfiguration> SourceMemberConfigurations { get; }
        internal AutoMapperMemberConfigurationCollection<AutoMapperTargetMemberConfiguration> TargetMemberConfigurations { get; }
    }
}
