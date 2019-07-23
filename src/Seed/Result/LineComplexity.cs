using Newtonsoft.Json;

namespace CodacyCSharp.Metrics.Seed.Result
{
    public sealed class LineComplexity
    {
        [JsonProperty(PropertyName = "line")] public long Line { get; set; }

        [JsonProperty(PropertyName = "value")] public long Value { get; set; }
    }
}
