from subprocess import run
from node_generator import generate
import os

DLYB = "dylib"


def build_main_lib():
    print(" -- building main libraries")
    native_lib_dir = "tree-sitter/lib/src"
    system_lib_dir = "/usr/local/lib"
    run([
        "gcc",
        "-fPIC",
        "-shared",
        "-o", f"{system_lib_dir}/libtree-sitter.{DLYB}",
        f"{native_lib_dir}/lib.c",
        "free.c",
        f"-I{native_lib_dir}",
        f"-I{native_lib_dir}/../include",
    ], check=True)


def build_lang(native_name, cs_name, additional_path, *files):
    print(" -- building", native_name, "language support")
    native_dir = os.path.join(
        "langs-native", f"tree-sitter-{native_name}", additional_path, "src")
    print(f"Building the native dir {native_dir}")
    print("    -- building native library")
    dotnet_dir = f"TreeSitter.{cs_name}"
    system_lib_dir = f"/usr/local/lib"
    run([
        "gcc",
        "-fPIC",
        "-shared",
        "-o", f"{dotnet_dir}/{get_library_name(native_name, additional_path)}.{DLYB}",
        *[f"{native_dir}/{file}" for file in files],
        f"-I{native_name}"
    ], check=True)

    print("    -- generating support code")
    generate(f"{native_dir}/node-types.json",
             f"{dotnet_dir}/Generated.cs", cs_name)

def get_library_name(native_name, additional_path):
    return f"libtree-sitter-{native_name if additional_path == '' else additional_path}"

def build_managed():
    print(" -- building dotnet libraries")
    run(["dotnet", "build"], check=True)


def main():
    build_main_lib()
    build_lang("typescript", "TypeScript", "typescript", "parser.c", "scanner.c")
    build_lang("typescript", "TypeScript", "tsx", "parser.c", "scanner.c")
    build_lang("javascript", "JavaScript", "", "parser.c", "scanner.c")
    build_lang("c-sharp", "CSharp", "", "parser.c", "scanner.c")
    build_lang("python", "Python", "", "parser.c", "scanner.c")
    build_lang("html", "Html", "", "parser.c", "scanner.c")
    build_lang("css", "Css", "", "parser.c", "scanner.c")
    build_lang("java", "Java", "", "parser.c")
    build_managed()


if __name__ == '__main__':
    main()
