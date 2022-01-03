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

    private static Type GetTypeFromWType(WType wType)
    {
        return wType switch
        {
            WType.String => typeof(string),
            WType.Int => typeof(long),
            _ => throw new Exception($"Unsupported type: {wType}.")
        };
    }

    private static void EmitPrint(ExpressionAstNode[] arguments, FunctionContext context)
    {
        if (arguments.Length != 1)
            throw new Exception("print expected exactly one argument.");

        var argument = arguments[0];
        var module = context.ModuleContext.AssemblyContext.Module;
        var il = context.IL;

        ExpressionEmitter.Emit(argument, context);

        if (argument.Type == null)
            throw new Exception("argument.Type is null.");

        var argumentType = GetTypeFromWType(argument.Type.Value);
        var writeLine = module.ImportReference(
            typeof(Console).GetMethod(nameof(Console.WriteLine), new[] { argumentType })
        );
        il.Emit(OpCodes.Call, writeLine);
    }

    private static void Emit(
        VariableDeclarationAndAssignmentStatementAstNode ast,
        FunctionContext context
    )
    {
        ExpressionEmitter.Emit(ast.Value, context);

        var module = context.ModuleContext.AssemblyContext.Module;
        var variableType = GetTypeFromWType(ast.Type);
        var variableTypeReference = module.ImportReference(variableType);

        context.IL.Body.Variables.Add(new VariableDefinition(variableTypeReference));
        var variableIndex = context.Scope.Define(ast.Identifier, ast.Type);

        context.IL.Emit(OpCodes.Stloc, variableIndex);
    }

    private static void Emit(
        AssignmentStatementAstNode ast,
        FunctionContext context
    )
    {
        ExpressionEmitter.Emit(ast.Value, context);

        var variable = context.Scope.GetOrThrow(ast.Identifier);
        context.IL.Emit(OpCodes.Stloc, variable.Index);
    }
}
