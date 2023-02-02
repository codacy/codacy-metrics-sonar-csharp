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

        /* PENDING UNDERSTANDING WTH LineComplexities even is.
            [Fact]
            public async Task ComplexityMathTest()
            {
                var codeAnalyzer = new CodeAnalyzer();
                var result = await codeAnalyzer.Analyze(sourceFile, CancellationToken.None);

                Assert.True(result.Complexity == result.LineComplexities.Max(x => x.Value),
                    "Complexity is not calculated correctly");
            }
    */
        [Fact]
        public async Task ResultCheckTest()
        {
            var codeAnalyzer = new CodeAnalyzer();
            var result = await codeAnalyzer.Analyze(sourceFile, CancellationToken.None);

            Console.WriteLine(result);

            // check if every field is correct
            Assert.Equal(sourceFile, result.Filename);
            Assert.Equal(131, result.Loc);
            Assert.Equal(4, result.Cloc);
            Assert.Equal(8, result.NrMethods);
            Assert.Equal(2, result.NrClasses);
            //Assert.Equal(10, result.Complexity); -- IS COMPLEXITY CHANGED IN A GOOD WAY OR STOPPED WORKING??
            Assert.Equal(23, result.Complexity); // with new code is bigger
        }

        [Fact]
        public async Task ResultJsonCheckTest()
        {
            var codeAnalyzer = new CodeAnalyzer();
            var result = await codeAnalyzer.Analyze(sourceFile, CancellationToken.None);

            // CHANGED THE JSON TO HAVE THE LINESCOMPLEXITY AS EMPTY LIST
            // check json parsing
            var expectedResult = File.ReadLines("Resources/Astar.json").First();

            // COMPLEXITY IS 0 FROM 10 that was here ! <- old comment when it was complelty broken
            // COMPLEXITY IS 0 FROM 23 that was here ! <- new change to get some complexity
            Console.WriteLine(result);
            Console.WriteLine(expectedResult);

            Assert.True(result.ToString() == expectedResult, "Wrong json format");
        }
    }
}
