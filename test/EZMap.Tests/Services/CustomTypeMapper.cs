namespace EZMap.Tests.Services
{
    public class CustomTypeMapper : ITypeMapper<int, string>
    {
        public string Map(int source)
        {
            return $"The number is {source}";
        }
    }
}
