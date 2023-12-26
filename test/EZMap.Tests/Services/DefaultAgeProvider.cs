namespace EZMap.Tests.Services
{
    public class DefaultAgeProvider : IAgeProvider
    {
        private readonly int age;

        public DefaultAgeProvider(int age)
        {
            this.age = age;
        }

        public int GetAge()
        {
            return age;
        }
    }
}
