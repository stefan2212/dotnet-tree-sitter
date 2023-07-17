using System;
using System.Diagnostics.SymbolStore;
using System.Runtime.InteropServices;

namespace TreeSitter.CSharp {
    public class CSharpLanguage
    {
        private const string DllName = "tree-sitter-c-sharp";

        [DllImport(DllName)]
        private static extern IntPtr tree_sitter_c_sharp();

        public static Language Create() => new Language(tree_sitter_c_sharp());
    }
}