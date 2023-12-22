using System.Reflection;

namespace EZMap.Utilities
{
    internal record MappableMember(string Name, MemberInfo SourceMember, MemberInfo TargetMember);
}
