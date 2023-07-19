using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using NUnit.Framework;
using TreeSitter.CSharp;
using static System.Text.Encoding;

namespace TreeSitter.Test
{
    public class TestParser
    {
        [Test]
        public void Should_Find_Declared_Variables()
        {
            var language = CSharpLanguage.Create();
            var parser = new Parser { Language = language };
            string code = @"
            namespace HelloWorld
            {
                class Hello {
                    static void Main(string[] args)
                    {
                        var randomNameForBoolean  = true;
                        var b = 5;
                        bool c = true;
                        bool d;
                    }
                }
            }";
            var tree = parser.Parse(code);
            List<string> names = new() { "randomNameForBoolean", "b", "c", "d" };
            var nodes = Bfs(tree.Root, "variable_declaration");
            List<Node> identifierNodes = new();
            List<string> variableNames = new();

            foreach (var node in nodes)
            {
                identifierNodes.AddRange(Bfs(node, "identifier"));
            }

            foreach (var identifier in identifierNodes)
            {
                variableNames.Add(Default.GetString(ASCII.GetBytes(code)
                                                        [(identifier.StartByte / 2)..((identifier.EndByte) / 2)]));
            }

            Assert.True(names.SequenceEqual(variableNames));
        }

        private List<Node> Bfs(Node root, string nodeKind)
        {
            var nodeQueue = new Queue<Node>();
            List<Node> nodes = new();
            nodeQueue.Enqueue(root);
            while (nodeQueue.Any())
            {
                var currentNode = nodeQueue.Dequeue();

                if (currentNode.Kind.Equals(nodeKind))
                {
                    nodes.Add(currentNode);
                }

                foreach (var child in currentNode.Children)
                {
                    nodeQueue.Enqueue(child);
                }
            }

            return nodes;
        }

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