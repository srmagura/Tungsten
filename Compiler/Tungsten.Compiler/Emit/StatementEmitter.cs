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

    private static Type GetExpressionType(AstNode ast, FunctionContext context)
    {
        var typeString = ast switch
        {
            StringAstNode => "string",
            IntAstNode => "int",
            VariableReferenceAstNode variableReference =>
                context.Scope.GetOrThrow(variableReference.Identifier).Type,
            _ => throw new Exception($"Unsupported: {ast.GetType().Name}.")
        };

        return GetTypeFromString(typeString, context.ModuleContext.AssemblyContext.Module);
    }

    private static void EmitPrint(AstNode[] arguments, FunctionContext context)
    {
        if (arguments.Length != 1)
            throw new Exception("print expected exactly one argument.");

        var argument = arguments[0];
        var module = context.ModuleContext.AssemblyContext.Module;
        var il = context.IL;

        ExpressionEmitter.Emit(argument, context);

        var argumentType = GetExpressionType(argument, context);

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
        var variableType = GetTypeFromString(ast.Type, module);
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
