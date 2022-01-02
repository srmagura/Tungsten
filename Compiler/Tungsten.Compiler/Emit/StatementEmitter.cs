using Mono.Cecil;
using Mono.Cecil.Cil;
using Tungsten.Compiler.AST;

namespace Tungsten.Compiler.Emit;

internal static class StatementEmitter
{
    internal static void Emit(AstNode ast, FunctionContext context)
    {
        switch (ast)
        {
            case FunctionCallStatementAstNode functionCall:
                Emit(functionCall, context);
                break;
            case VariableDeclarationAndAssignmentStatementAstNode variableDeclarationAndAssignment:
                Emit(variableDeclarationAndAssignment, context);
                break;
            case AssignmentStatementAstNode assignment:
                Emit(assignment, context);
                break;
            default:
                throw new Exception($"Unsupported statement: {ast.GetType().Name}.");
        }
    }

    private static void Emit(FunctionCallStatementAstNode ast, FunctionContext context)
    {
        var functionCall = ast.FunctionCall;

        if (functionCall.Function == "print")
        {
            EmitPrint(functionCall.Arguments, context);
            return;
        }

        var method = context.ModuleContext.Class.Methods
            .FirstOrDefault(m => m.Name == functionCall.Function);

        if (method != null)
        {
            // TODO support arguments
            context.IL.Emit(OpCodes.Call, method);
            return;
        }

        throw new Exception($"Function not found: {functionCall.Function}.");
    }

    private static Type GetTypeFromString(string type, ModuleDefinition module)
    {
        return type switch
        {
            "string" => typeof(string),
            "int" => typeof(long),
            _ => throw new Exception($"Unsupported type: {type}.")
        };
    }

    private static void EmitPrint(AstNode[] arguments, FunctionContext context)
    {
        if (arguments.Length != 1)
            throw new Exception("print expected exactly one argument.");

        var argument = arguments[0];
        var module = context.ModuleContext.AssemblyContext.Module;
        var il = context.IL;

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

                    var variable = context.Scope.GetOrThrow(variableArgument.Identifier);
                    var variableType = GetTypeFromString(variable.Type, context.ModuleContext.AssemblyContext.Module);

                    var writeLine = module.ImportReference(
                        typeof(Console).GetMethod(nameof(Console.WriteLine), new[] { variableType })
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
        FunctionContext context
    )
    {
        var module = context.ModuleContext.AssemblyContext.Module;
        var variableType = GetTypeFromString(ast.Type, module);
        var variableTypeReference = module.ImportReference(variableType);

        context.IL.Body.Variables.Add(new VariableDefinition(variableTypeReference));
        context.Scope.Variables[ast.Identifier] = new Variable(ast.Identifier, ast.Type);

        ExpressionEmitter.Emit(ast.Value, context);

        // TODO 0 is hardcoded
        context.IL.Emit(OpCodes.Stloc_0);
    }

    private static void Emit(
        AssignmentStatementAstNode ast,
        FunctionContext context
    )
    {
        var variable = context.Scope.GetOrThrow(ast.Identifier);

        ExpressionEmitter.Emit(ast.Value, context);

        // TODO 0 is hardcoded
        context.IL.Emit(OpCodes.Stloc_0);
    }
}
