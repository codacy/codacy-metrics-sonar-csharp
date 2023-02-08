using System;
using System.Collections.Generic;
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

            var functionMetrics = new CodacyCSharpFunctionMetrics(syntaxTree, semanticModel);

            // total complexity of file is the biggest complexity of the functions in the file.
            result.LineComplexities = functionMetrics.functionComplexities;
            result.Complexity = functionMetrics.functionComplexities
                                    .Select(functionComplexity => functionComplexity.Value)
                                    .DefaultIfEmpty(0)
                                    .Max();

            return result;
        }
    }

    // workaround to get stuff only available as protected.
    public class CodacyCSharpFunctionMetrics : CSharpMetrics
    {
        public IEnumerable<LineComplexity> functionComplexities { get; }

        public CodacyCSharpFunctionMetrics(
            Microsoft.CodeAnalysis.SyntaxTree tree,
            Microsoft.CodeAnalysis.SemanticModel semanticModel) : base(tree, semanticModel)
        {
            // each function found in the file and the cyclomatic complexity of each of those functions
            this.functionComplexities = tree.GetRoot()
                .DescendantNodes()
                .Where(IsFunction)
                .Select(functionNode =>
                    (functionNode.GetLocation().GetMappedLineSpan().Span.Start.Line + 1, functionNode)
                )
                .Select(lineAndFunctionNode =>
                {
                    (var line, var node) = lineAndFunctionNode;

                    return new LineComplexity
                    {
                        Line = line,
                        Value = this.ComputeCyclomaticComplexity(node)
                    };
                });
        }
    }
}
