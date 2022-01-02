using Mono.Cecil;
using Mono.Cecil.Cil;
using Tungsten.Compiler.AST;

namespace Tungsten.Compiler.Emit;

internal static class StatementEmitter
{
    internal static void Emit(AstNode ast, ModuleDefinition module, TypeDefinition @class, ILProcessor il)
    {
        switch (ast)
        {
            case FunctionCallStatementAstNode functionCall:
                Emit(functionCall, module, @class, il);
                break;
            case VariableDeclarationAndAssignmentStatementAstNode variableDeclarationAndAssignment:
                Emit(variableDeclarationAndAssignment, module, @class, il);
                break;
            default:
                throw new Exception($"Unsupported statement: {ast.GetType().Name}.");
        }
    }

    private static void Emit(
        FunctionCallStatementAstNode ast,
        ModuleDefinition module,
        TypeDefinition @class,
        ILProcessor il
    )
    {
        var functionCall = ast.FunctionCall;

        if (functionCall.Function == "print")
        {
            EmitPrint(functionCall.Arguments, module, il);
            return;
        }

        var method = @class.Methods.FirstOrDefault(m => m.Name == functionCall.Function);

        if (method != null)
        {
            // TODO support arguments
            il.Emit(OpCodes.Call, method);
            return;
        }

        throw new Exception($"Function not found: {functionCall.Function}.");

    }

    private static void EmitPrint(AstNode[] arguments, ModuleDefinition module, ILProcessor il)
    {
        if (arguments.Length != 1)
            throw new Exception("print expected exactly one argument.");

        var argument = arguments[0];

        switch (argument)
        {
            case StringAstNode stringArgument:
                {
                    il.Emit(OpCodes.Ldstr, stringArgument.Value);

                    var writeLine = module.ImportReference(
                        typeof(Console).GetMethod(nameof(Console.WriteLine), new[] { typeof(string) })
                    );
                    il.Emit(OpCodes.Call, writeLine);
                    break;
                }
            case IntAstNode intArgument:
                {
                    il.Emit(OpCodes.Ldc_I8, intArgument.Value);

                    var writeLine = module.ImportReference(
                        typeof(Console).GetMethod(nameof(Console.WriteLine), new[] { typeof(long) })
                    );
                    il.Emit(OpCodes.Call, writeLine);
                    break;
                }
            case VariableReferenceAstNode variableArgument:
                {
                    il.Emit(OpCodes.Ldloc_0);

                    var writeLine = module.ImportReference(
                        typeof(Console).GetMethod(nameof(Console.WriteLine), new[] { typeof(string) })
                    );
                    il.Emit(OpCodes.Call, writeLine);
                    break;
                }
            default:
                throw new Exception($"Unsupported print argument: {argument.GetType().Name}.");
        }
    }

    private static void Emit(
        VariableDeclarationAndAssignmentStatementAstNode ast,
        ModuleDefinition module,
        TypeDefinition @class,
        ILProcessor il
    )
    {
        if (ast.Value is not StringAstNode stringAstNode)
            throw new Exception("Only string is supported right now.");

        var @string = module.ImportReference(typeof(string));
        il.Body.Variables.Add(new VariableDefinition(@string));

        il.Emit(OpCodes.Ldstr, stringAstNode.Value);
        il.Emit(OpCodes.Stloc_0);
    }
}
