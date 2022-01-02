using Mono.Cecil;
using Mono.Cecil.Cil;
using Tungsten.Compiler.AST;

namespace Tungsten.Compiler.Emit;

internal static class StatementEmitter
{
    internal static void Emit(AstNode ast, ModuleDefinition module, ILProcessor il)
    {
        switch(ast)
        {
            case FunctionCallStatementAstNode functionCall:
                Emit(functionCall, module, il);
                break;
            default:
                throw new Exception($"Unsupported statement: {ast.GetType().Name}.");
        }
    }

    private static void Emit(FunctionCallStatementAstNode ast, ModuleDefinition module, ILProcessor il)
    {
        var functionCall = ast.FunctionCall;
        
        if(functionCall.Function == "print")
        {
            EmitPrint(functionCall.Arguments, module, il);
            return;
        }

        throw new Exception($"Function not found: {functionCall.Function}.");

    }

    private static void EmitPrint(AstNode[] arguments, ModuleDefinition module, ILProcessor il)
    {
        if (arguments.Length != 1)
            throw new Exception("print expected exactly one argument.");

        var argument = arguments[0];
        if (argument is not StringAstNode s)
            throw new Exception("The argument to print must be a string.");

        il.Emit(OpCodes.Ldstr, s.Value);

        var writeLine = module.ImportReference(
            typeof(Console).GetMethod(nameof(Console.WriteLine), new[] { typeof(string) })
        );
        il.Emit(OpCodes.Call, writeLine);
    }
}
