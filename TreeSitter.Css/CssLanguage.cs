
using System.Runtime.InteropServices;

namespace TreeSitter.Css
{
    public class CssLanguage
    {
        private const string DllName = "tree-sitter-css";

        [DllImport(DllName)]
        private static extern IntPtr tree_sitter_css();

        public static Language Create() => new(tree_sitter_css());
    } 
}
