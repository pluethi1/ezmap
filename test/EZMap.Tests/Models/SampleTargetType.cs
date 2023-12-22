namespace EZMap.Tests.Models
{
    public class SampleTargetType
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description = string.Empty;
        public int Age { get; }
        public const string MY_SECRET_CONSTANT = "I am a constant too";
    }
}
