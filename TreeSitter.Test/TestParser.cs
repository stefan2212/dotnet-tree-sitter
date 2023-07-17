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
            var language = CSharpLanguage.Create();
            var parser = new Parser { Language = language };
            var tree = parser.Parse(@"
            namespace HelloWorld
            {
                class Hello {
                    static void Main(string[] args)
                    {
                        var a  = 5;
                    }
                }
            }");

            Assert.AreEqual(Trim(@"(compilation_unit 
            (namespace_declaration name: (identifier) body: 
            (declaration_list (class_declaration name: (identifier) body: 
            (declaration_list (method_declaration (modifier) type: (predefined_type) name: 
            (identifier) parameters: (parameter_list (parameter type: (array_type type: 
            (predefined_type) rank: (array_rank_specifier)) name: (identifier))) body: 
            (block (local_declaration_statement (variable_declaration type: (implicit_type) (variable_declarator name: 
            (identifier) (equals_value_clause (integer_literal))))))))))))"),
                            tree.Root.ToString());
        }

        private static string Trim(string a)
        {
            return Regex.Replace(a, @"\s+", " ");
        }
    }
}