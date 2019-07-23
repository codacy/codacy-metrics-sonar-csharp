using Newtonsoft.Json;

namespace CodacyCSharp.Metrics.Seed.Configuration
{
    public class CodacyMetricsConfiguration
    {
        [JsonProperty(PropertyName = "files")] public string[] Files { get; set; }

        [JsonProperty(PropertyName = "language")]
        public string Language { get; set; }
    }
}
