from subprocess import run
from node_generator import generate

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


def build_lang(native_name, cs_name, *files):
    print(" -- building", native_name, "language support")
    print("    -- building native library")
    native_dir = f"langs-native/tree-sitter-{native_name}/src"
    dotnet_dir = f"TreeSitter.{cs_name}"
    system_lib_dir = f"/usr/local/lib"
    run([
        "gcc",
        "-fPIC",
        "-shared",
        "-o", f"{system_lib_dir}/libtree-sitter-{native_name}.{DLYB}",
        *[f"{native_dir}/{file}" for file in files],
        f"-I{native_name}"
    ], check=True)

    print("    -- generating support code")
    generate(f"{native_dir}/node-types.json", f"{dotnet_dir}/Generated.cs", cs_name)


def build_managed():
    print(" -- building dotnet libraries")
    run(["dotnet", "build"], check=True)


def main():
    build_main_lib()
    build_lang("c-sharp", "CSharp", "parser.c", "scanner.c")
    build_managed()


if __name__ == '__main__':
    main()
