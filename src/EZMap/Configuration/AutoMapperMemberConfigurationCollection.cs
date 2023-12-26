using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace EZMap.Configuration
{
    internal sealed class AutoMapperMemberConfigurationCollection<TMemberConfiguration> : IEnumerable<KeyValuePair<string, TMemberConfiguration>>
        where TMemberConfiguration : class
    {
        private readonly Type objectType;
        private readonly Dictionary<string, TMemberConfiguration> configurations = new();

        public AutoMapperMemberConfigurationCollection(Type objectType)
        {
            this.objectType = objectType;
        }

        internal void Add(string memberName, TMemberConfiguration configuration)
        {
            AssertValidMember(memberName, nameof(memberName));
            configurations.Add(memberName, configuration);
        }

        internal TMemberConfiguration this[string memberName]
        {
            get => configurations[memberName];
            set
            {
                AssertValidMember(memberName, nameof(memberName));
                configurations[memberName] = value;
            }
        }

        internal bool TryGetMemberConfiguration(string memberName, [NotNullWhen(true)]out TMemberConfiguration? configuration)
        {
            return configurations.TryGetValue(memberName, out configuration);
        }

        public IEnumerator<KeyValuePair<string, TMemberConfiguration>> GetEnumerator() => configurations.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        internal void AssertValidMember(string memberName, string parameterName)
        {
            var members = objectType.GetMember(memberName);
            if (members != null && members.Length == 1)
            {
                var member = members[0];
                if (member.MemberType == MemberTypes.Property || 
                    member.MemberType == MemberTypes.Field)
                {
                    return;
                }
            }

            throw new ArgumentException($"Member {memberName} does not exist in type {objectType} or is not a field or property", parameterName);
        }
    }
}
