using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CodacyCSharp.Metrics.Seed.Configuration;
using Newtonsoft.Json;

namespace CodacyCSharp.Metrics.Seed
{
    public abstract class MetricsCodeAnalyzer
    {
        protected const string DefaultConfigFile = ".codacyrc";
        protected const string DefaultSourceFolder = "src/";
        protected const string CSharpExtension = ".cs";
        protected readonly CodacyMetricsConfiguration Config;

        protected MetricsCodeAnalyzer()
        {
            if (File.Exists(DefaultConfigFile))
            {
                var configJSON = File.ReadAllText(DefaultConfigFile);
                Config = JsonConvert.DeserializeObject<CodacyMetricsConfiguration>(configJSON);
            }
            else
            {
                Config = new CodacyMetricsConfiguration
                {
                    Files = GetSourceFiles(DefaultSourceFolder, CSharpExtension).ToArray()
                };
            }
        }

        protected abstract Task Analyze(CancellationToken cancellationToken);

        public async Task Run()
        {
            var timeoutEnv = Environment.GetEnvironmentVariable("TIMEOUT");

            if (timeoutEnv is null)
            {
                await Analyze(CancellationToken.None).ConfigureAwait(false);
            }
            else
            {
                try
                {
                    var timeout = TimeSpanHelper.Parse(timeoutEnv);
                    using (var cancellationTokenSource = new CancellationTokenSource())
                    {
                        var cancellationToken = cancellationTokenSource.Token;
                        var task = Analyze(cancellationToken);
                        if (await Task.WhenAny(task, Task.Delay(timeout)).ConfigureAwait(false) != task)
                        {
                            cancellationTokenSource.Cancel();
                            Environment.Exit(2);
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.Error.WriteLine($"can't parse 'TIMEOUT' environment variable ({timeoutEnv})");
                    Logger.Send(e.StackTrace);
                    Environment.Exit(1);
                }
            }
        }

        private static IEnumerable<string> GetSourceFiles(string folder, string fileExtension)
        {
            var files = Directory.GetFiles(folder, "*" + fileExtension, SearchOption.AllDirectories);
            return from string entry in files
                select entry.Substring(entry.IndexOf("/", StringComparison.InvariantCulture) + 1);
        }
    }
}
