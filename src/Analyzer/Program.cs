namespace CodacyCSharp.Metrics.Analyzer
{
    internal static class Program
    {
        private static void Main()
        {
            new CodeAnalyzer().Run()
                .GetAwaiter().GetResult();
        }
    }
}
