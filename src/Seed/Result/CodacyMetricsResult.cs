using System.Collections.Generic;
using Newtonsoft.Json;

namespace CodacyCSharp.Metrics.Seed.Result
{
    public sealed class CodacyMetricsResult
    {
        [JsonProperty(PropertyName = "filename")]
        public string Filename { get; set; }

        [JsonProperty(PropertyName = "complexity")]
        public long Complexity { get; set; }

        [JsonProperty(PropertyName = "loc")] public long Loc { get; set; }

        [JsonProperty(PropertyName = "cloc")] public long Cloc { get; set; }

        [JsonProperty(PropertyName = "nrMethods")]
        public long NrMethods { get; set; }

        [JsonProperty(PropertyName = "nrClasses")]
        public long NrClasses { get; set; }

        [JsonProperty(PropertyName = "lineComplexities")]
        public IEnumerable<LineComplexity> LineComplexities { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this,
                Formatting.None,
                new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore
                });
        }
    }
}
