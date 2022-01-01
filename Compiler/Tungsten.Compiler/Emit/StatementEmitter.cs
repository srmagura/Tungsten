using Mono.Cecil;
using Mono.Cecil.Cil;
using Tungsten.Compiler.AST;

namespace Tungsten.Compiler.Emit;

internal static class StatementEmitter
{
    internal static void Emit(AstNode ast, ModuleDefinition module, ILProcessor il)
    {
        il.Emit(OpCodes.Ldstr, "hello world");

        var writeLine = module.ImportReference(
            typeof(Console).GetMethod(nameof(Console.WriteLine), new[] { typeof(string) })
        );
        il.Emit(OpCodes.Call, writeLine);
    }
}
