using Tungsten.Compiler.AST;

namespace Tungsten.Compiler.TypeCheck;

internal static class ExpressionTypeChecker
{
    internal static void TypeCheck(ExpressionAstNode ast)
    {
        switch (ast)
        {
            case FunctionCallAstNode functionCall:
                TypeCheck(functionCall);
                break;
            case BinaryOperationAstNode binaryOperation:
                TypeCheck(binaryOperation);
                break;
            case BoolAstNode:
            case IntAstNode:
            case StringAstNode:
                break;
            default:
                throw new Exception($"Unsupported expression: {ast.GetType().Name}.");
        }
    }

    private static void TypeCheck(FunctionCallAstNode ast)
    {
        foreach(var argument in ast.Arguments)
        {
            TypeCheck(argument);
        }
    }

    private static void TypeCheck(BinaryOperationAstNode ast)
    {
        TypeCheck(ast.Operand0);
        TypeCheck(ast.Operand1);

        if (ast.Operand0.Type == WType.Int &&
            ast.Operand1.Type == WType.Int &&
            ast.Operator != BinaryOperator.Division)
        {
            ast.Type = WType.Int;
            return;
        }

        throw new Exception("Unsupported binary operation.");
    }
}
