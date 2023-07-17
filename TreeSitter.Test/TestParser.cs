using System.Text.RegularExpressions;
using NUnit.Framework;
using TreeSitter.CSharp;

namespace TreeSitter.Test
{
    public class TestParser
    {
        [Test]
        public void TestSetLanguage()
        {
            var parser = new Parser
            {
                Language = CSharpLanguage.Create()
            };

            var tree = parser.Parse("def foo():\n  bar()");

            Assert.AreEqual(Trim(@"(module (function_definition
                name: (identifier)
                parameters: (parameters)
                body: (block (expression_statement (call
                    function: (identifier)
                    arguments: (argument_list))))))"),
                            tree.Root.ToString());
        }

        private static string Trim(string a)
        {
            return Regex.Replace(a, @"\s+", " ");
        }
    }
}