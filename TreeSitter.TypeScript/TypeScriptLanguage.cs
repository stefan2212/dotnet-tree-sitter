using System.Runtime.InteropServices;

namespace TreeSitter.TypeScript
{
    public class TypeScriptLanguage
    {
        private const string TypeScriptDllName = "tree-sitter-typescript";
        private const string TsxDllName = "tree-sitter-tsx";

        [DllImport(TypeScriptDllName)]
        private static extern IntPtr tree_sitter_typescript();

        [DllImport(TsxDllName)]
        private static extern IntPtr tree_sitter_tsx();
        
        public static Language Create() => new(tree_sitter_typescript());
        public static Language CreateTsx() => new(tree_sitter_tsx());
    }
}
