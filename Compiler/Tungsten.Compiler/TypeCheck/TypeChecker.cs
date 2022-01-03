using Tungsten.Compiler.AST;

namespace Tungsten.Compiler.TypeCheck;

internal static class TypeChecker
{
    // Recurse the AST and fill in any nodes that have Type=null
    internal static void TypeCheck(AssemblyAstNode ast)
    {
        TypeCheck(ast.Module);
    }

    private static void TypeCheck(ModuleAstNode ast)
    {
        foreach (var functionDeclaration in ast.FunctionDeclarations)
        {
            TypeCheck(functionDeclaration);
        }
    }

    private static void TypeCheck(FunctionDeclarationAstNode ast)
    {
        foreach (var statement in ast.Statements)
        {
            TypeCheckStatement(statement);
        }
    }

    private static void TypeCheckStatement(AstNode ast)
    {
        switch (ast)
        {
            case FunctionCallStatementAstNode functionCall:
                ExpressionTypeChecker.TypeCheck(functionCall.FunctionCall);
                break;
            case VariableDeclarationAndAssignmentStatementAstNode variableDeclarationAndAssignment:
                TypeCheck(variableDeclarationAndAssignment);
                break;
            default:
                throw new Exception($"Unsupported statement: {ast.GetType().Name}.");
        }
    }

    private static void TypeCheck(VariableDeclarationAndAssignmentStatementAstNode ast)
    {
        ExpressionTypeChecker.TypeCheck(ast.Value);

        if (ast.Type != ast.Value.Type)
        {
            throw new Exception($"Type error: {ast.Value.Type} is not assignable to {ast.Type}.");
        }
    }
}
