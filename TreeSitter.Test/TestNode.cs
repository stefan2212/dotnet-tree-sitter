using System.Reflection;
using NUnit.Framework;
using TreeSitter.CSharp;
using TreeSitter.CSharp.Nodes;

namespace TreeSitter.Test
{
    public class TestNode
    {
        [Test]
        public void TestChildByFieldId()
        {
            var language = CSharpLanguage.Create();
            var parser = new Parser {Language = language};
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
            var rootNode = tree.Root;
            var fnNode = tree.Root.Child(0);

            Assert.IsNull(language.FieldIdForName("randomField"));
            var nameFieldQ = language.FieldIdForName("name");
            var aliasFieldQ = language.FieldIdForName("alias");
            
            Assert.IsNotNull(nameFieldQ);
            Assert.IsNotNull(aliasFieldQ);

            var nameField = (ushort) nameFieldQ;
            var aliasField = (ushort) aliasFieldQ;
            
            Assert.IsNull(rootNode.ChildByFieldId(aliasField));
            Assert.IsNull(rootNode.ChildByFieldId(nameField));
            
            Assert.IsNull(fnNode.ChildByFieldId(aliasField));
            Assert.AreEqual("identifier", fnNode.ChildByFieldId(nameField).Kind);

            Assert.AreEqual("identifier", fnNode.ChildByFieldName("name").Kind);
            Assert.IsNull(fnNode.ChildByFieldName("asdfasdfname"));
        }

        [Test]
        public void TestChildren()
        {
            var language = CSharpLanguage.Create();
            var parser = new Parser {Language = language};
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

            var rootNode = tree.Root;
            Assert.AreEqual("compilation_unit", rootNode.Kind);

            var fnNode = rootNode.Child(0);
            Assert.AreEqual("namespace_declaration", fnNode.Kind);
        }

        [Test]
        public void TestNodeConvert()
        {
            var language = CSharpLanguage.Create();
            var parser = new Parser {Language = language};
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

            var res = CSharpLanguageNode.FromNode(tree.Root);
            Assert.IsInstanceOf<CompilationUnit>(res);

            var compilationUnit = (CompilationUnit)res!;
            Assert.IsInstanceOf<NamespaceDeclaration>(compilationUnit.Children[0]);
        }
    }
}