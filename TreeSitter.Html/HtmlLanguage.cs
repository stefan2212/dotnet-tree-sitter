using System.Runtime.InteropServices;

namespace TreeSitter.Html
{
    public class HtmlLanguage
    {
        private const string DllName = "tree-sitter-html";

        [DllImport(DllName)]
        private static extern IntPtr tree_sitter_html();

        public static Language Create() => new(tree_sitter_html());
    }
}