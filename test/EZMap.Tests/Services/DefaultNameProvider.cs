namespace EZMap.Tests.Services
{
    public class DefaultNameProvider : INameProvider
    {
        private readonly string name;

        public DefaultNameProvider(string name)
        {
            this.name = name;
        }

        public string GetName()
        {
            return name;
        }
    }
}
