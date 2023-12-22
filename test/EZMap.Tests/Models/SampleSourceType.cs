namespace EZMap.Tests.Models
{
    public class SampleSourceType
    {
        public int Id { get; set; } = 1;
        public string Name { get; } = "Foo";
        public readonly string Description = "Bar";
        public int Age;
        public const string MY_SUPER_CONSTANT = "Hello I am a constant";
    }
}
