using System.Runtime.InteropServices;

namespace TreeSitter.Java
{
    public class JavaLanguage
    {
        private const string DllName = "tree-sitter-java";

        [DllImport(DllName)]
        private static extern IntPtr tree_sitter_java();

        public static Language Create() => new(tree_sitter_java());
    }
}