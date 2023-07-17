using System;
using System.IO;
using NUnit.Framework;
using TreeSitter.CSharp;
using TreeSitter.CSharp.Nodes;

namespace TreeSitter.Test;

public class TestShotgun
{
    private static void PerformTest<T>(string examplesDir, Language lang, Func<Node, T> modelCreator)
    {
        foreach (var example in Directory.EnumerateFiles(examplesDir))
        {
            var codeString = File.ReadAllText(example);
            using var parser = new Parser { Language = lang };
            var tree = parser.Parse(codeString);
            Assert.IsFalse(tree.Root.HasError, "has errors: {0} {1}", example, tree.Root);
            var res = modelCreator(tree.Root);
        }
    }

    [Test]
    public void TestCSharp()
    {
        PerformTest(
                    "../../../../langs-native/tree-sitter-c-sharp/examples",
                    CSharpLanguage.Create(),
                    CSharpLanguageNode.FromNode
                   );
    }
}