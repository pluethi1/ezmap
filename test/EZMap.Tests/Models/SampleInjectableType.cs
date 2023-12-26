using EZMap.Tests.Services;

namespace EZMap.Tests.Models
{
    public class SampleInjectableType
    {
        private readonly IAgeProvider? ageProvider;
        private readonly INameProvider? nameProvider;

        private string? name;
        private int age = -1;

        public SampleInjectableType()
        {
        }

        public SampleInjectableType(IAgeProvider ageProvider)
        {
            this.ageProvider = ageProvider;
        }

        public SampleInjectableType(INameProvider nameProvider)
        {
            this.nameProvider = nameProvider;
        }

        public SampleInjectableType(IAgeProvider? ageProvider, INameProvider? nameProvider)
        {
            this.ageProvider = ageProvider;
            this.nameProvider = nameProvider;
        }

        public int Age
        {
            get => age == -1 ? ageProvider?.GetAge() ?? 0 : 0;
            set => age = value;
        }

        public string Name
        {
            get => name ??= nameProvider?.GetName() ?? string.Empty;
            set => name = value;
        }
    }
}