using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CodacyCSharp.Metrics.Analyzer.Runner;
using CodacyCSharp.Metrics.Seed;
using CodacyCSharp.Metrics.Seed.Result;
using SonarAnalyzer.Metrics.CSharp;

namespace CodacyCSharp.Metrics.Analyzer
{
    public class CodeAnalyzer : MetricsCodeAnalyzer
    {
        protected override async Task Analyze(CancellationToken cancellationToken)
        {
            foreach (var file in Config.Files)
                try
                {
                    Console.WriteLine(await Analyze(file, cancellationToken));
                }
                catch (Exception e)
                {
                    Console.Error.Write(e.StackTrace);
                    Environment.Exit(1);
                }
        }

        public async Task<CodacyMetricsResult> Analyze(string file, CancellationToken cancellationToken)
        {
            var result = new CodacyMetricsResult
            {
                Filename = file
            };

            var solution = CompilationHelper.GetSolutionFromFile(DefaultSourceFolder + file);
            var compilation = await solution.Projects.First().GetCompilationAsync(cancellationToken);
            var syntaxTree = compilation.SyntaxTrees.First();
            var document = solution.GetDocument(syntaxTree);
            var semanticModel = await document.GetSemanticModelAsync(cancellationToken);

            var metrics = new CSharpMetrics(syntaxTree, semanticModel);

            result.Loc = metrics.CodeLines.Count();
            result.Cloc = metrics.GetComments(true).NonBlank.Count();

            result.NrMethods = metrics.FunctionCount;
            result.NrClasses = metrics.ClassCount;

            // I'm under the impression that LineComplexities is not reported anywhere 
            // and we may not need it anymore anyway. which is good since the api dissapeared.

            result.LineComplexities = Enumerable.Empty<LineComplexity>();

            // we no longer have access to 'ComputeCyclomaticComplexity'
            // .Complexity is ComputeCognitiveComplexity for the ast tree root.
            // is this good enough?
            result.Complexity = metrics.Complexity;

            /*result.LineComplexities = metrics.FunctionNodes
                .GroupBy(row =>
                    row.GetLocation().GetMappedLineSpan().Span.Start.Line + 1
                )
                .Select(nodeGroup =>
                {
                    var lineComplexity = nodeGroup.Max(num => metrics.ComputeCyclomaticComplexity(num));
                    result.Complexity = Math.Max(result.Complexity, lineComplexity);
                    return new LineComplexity
                    {
                        Line = nodeGroup.Key,
                        Value = lineComplexity
                    };
                })
                .ToList();
            */
            return result;
        }
    }
}
