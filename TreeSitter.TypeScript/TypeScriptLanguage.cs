using System.Runtime.InteropServices;

namespace TreeSitter.TypeScript
{
    public class TypeScriptLanguage
    {
        private const string DllName = "tree-sitter-typescript";

        [DllImport(DllName)]
        private static extern IntPtr tree_sitter_typescript();

        [DllImport(DllName)]
        private static extern IntPtr tree_sitter_tsx();
        
        public static Language Create() => new(tree_sitter_typescript());
        public static Language CreateTsx() => new(tree_sitter_tsx());
    }
}
