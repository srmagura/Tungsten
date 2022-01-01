using Mono.Cecil;
using TungstenCompiler.AST;

namespace TungstenCompiler.Emit;

internal static class AssemblyEmitter
{
    internal static AssemblyDefinition Emit(ModuleAstNode ast)
    {
        var assemblyNameString = "TungstenApp";
        var assemblyName = new AssemblyNameDefinition(assemblyNameString, new Version(1, 0, 0));
        var assembly = AssemblyDefinition.CreateAssembly(assemblyName, "Program", ModuleKind.Console);
        
        var (moduleType, entryPoint) = ModuleEmitter.Emit(ast, assembly.MainModule, assemblyNameString);

        assembly.MainModule.Types.Add(moduleType);
        assembly.MainModule.EntryPoint = entryPoint;

        return assembly;
    }
}
