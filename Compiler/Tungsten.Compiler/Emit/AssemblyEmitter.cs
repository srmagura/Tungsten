using Mono.Cecil;
using Tungsten.Compiler.AST;

namespace Tungsten.Compiler.Emit;

internal static class AssemblyEmitter
{
    internal static AssemblyDefinition Emit(AssemblyAstNode ast)
    {
        var assemblyName = new AssemblyNameDefinition(ast.Name, new Version(1, 0, 0));
        var assembly = AssemblyDefinition.CreateAssembly(assemblyName, "Main", ModuleKind.Dll);

        var assemblyContext = new AssemblyContext(assembly.MainModule, RootNamespace: ast.Name);

        var (moduleType, entryPoint) = ModuleEmitter.Emit(ast.Module, assemblyContext);

        assembly.MainModule.Types.Add(moduleType);
        assembly.MainModule.EntryPoint = entryPoint;

        return assembly;
    }
}
