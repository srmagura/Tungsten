using Mono.Cecil;
using TungstenCompiler.AST;

namespace TungstenCompiler.Emit;

internal static class AssemblyEmitter
{
    internal static void Emit(ModuleAstNode ast)
    {
        var assemblyNameString = "ConsoleApp";
        var assemblyName = new AssemblyNameDefinition(assemblyNameString, new Version(1, 0, 0));
        var assembly = AssemblyDefinition.CreateAssembly(assemblyName, "Program", ModuleKind.Console);
        var mainModule = assembly.MainModule;

        var programClass = new TypeDefinition(
            assemblyNameString, 
            "Program", 
            TypeAttributes.Class | TypeAttributes.NotPublic
        );

        mainModule.Types.Add(programClass);
        mainModule.EntryPoint = null;
    }
}
