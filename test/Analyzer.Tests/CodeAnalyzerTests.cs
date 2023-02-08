using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace CodacyCSharp.Metrics.Analyzer.Tests
{
    public class CodeAnalyzerTests
    {
        private const string sourceFile = "Astar.cs";

        [Fact]
        public async Task ComplexityMathTest()
        {
            var codeAnalyzer = new CodeAnalyzer();
            var result = await codeAnalyzer.Analyze(sourceFile, CancellationToken.None);

            Assert.True(result.LineComplexities.ToArray().Length == result.NrMethods,
                "Line complexity doesn't match");

            Assert.True(result.Complexity == result.LineComplexities.Max(x => x.Value),
                "Complexity is not calculated correctly");
        }

        [Fact]
        public async Task ResultCheckTest()
        {
            var codeAnalyzer = new CodeAnalyzer();
            var result = await codeAnalyzer.Analyze(sourceFile, CancellationToken.None);

            // check if every field is correct
            Assert.Equal(sourceFile, result.Filename);
            Assert.Equal(131, result.Loc);
            Assert.Equal(4, result.Cloc);
            Assert.Equal(8, result.NrMethods);
            Assert.Equal(2, result.NrClasses);
            Assert.Equal(10, result.Complexity);
        }

        [Fact]
        public async Task ResultJsonCheckTest()
        {
            var codeAnalyzer = new CodeAnalyzer();
            var result = await codeAnalyzer.Analyze(sourceFile, CancellationToken.None);

            // check json parsing
            var expectedResult = File.ReadLines("Resources/Astar.json").First();

            Assert.True(result.ToString() == expectedResult, "Wrong json format");
        }
    }
}
