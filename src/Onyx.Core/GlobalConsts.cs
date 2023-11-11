namespace Onyx.Core
{
    [ExcludeFromCodeCoverage]
    public static class GlobalConsts
    {
        public static readonly JsonSerializerOptions JsonSerializerOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            NumberHandling = JsonNumberHandling.WriteAsString,
        };

        public static readonly char Delimiter = ';';
    }
}