using Tungsten.Compiler.Parser;

namespace Tungsten.Compiler.AST;

internal static class ExpressionToAst
{
    internal static AstNode Convert(ParseNode parseNode)
    {
        return parseNode switch
        {
            IntLiteralParseNode intLiteral => ConvertIntLiteral(intLiteral),
            StringLiteralParseNode stringLiteral => ConvertStringLiteral(stringLiteral),
            BoolLiteralParseNode boolLiteral => ConvertBoolLiteral(boolLiteral),

            UnaryOperationParseNode unaryOperation => ConvertUnaryOperation(unaryOperation),
            BinaryOperationParseNode binaryOperation => ConvertBinaryOperation(binaryOperation),

            IdentifierExpressionParseNode identifierExpression => ConvertIdentifierExpression(identifierExpression),

            _ => throw new Exception($"Could not convert to expression: {parseNode.GetType().Name}.")
        };
    }

    internal static FunctionCallAstNode ConvertFunctionCall(FunctionCallParseNode parseNode)
    {
        var arguments = parseNode.ArgumentList.Arguments
            .Select(Convert)
            .ToArray();

        return new FunctionCallAstNode(parseNode.Function, arguments);
    }

    private static AstNode ConvertBinaryOperation(BinaryOperationParseNode parseNode)
    {
        var operand0 = Convert(parseNode.Operand);

        if (parseNode.Post is PostBinaryOperationParseNode postMultiplication)
        {
            return ConvertPostBinaryOperation(postMultiplication, operand0);
        }
        else if (parseNode.Post is EmptyParseNode)
        {
            return operand0;
        }
        else
        {
            var property = $"{nameof(BinaryOperationParseNode)}.{nameof(BinaryOperationParseNode.Post)}";
            throw new Exception($"{property} was ${parseNode.Post.GetType().Name} which is not allowed.");
        }
    }

    private static AstNode ConvertPostBinaryOperation(PostBinaryOperationParseNode parseNode, AstNode operand0)
    {
        var operand1 = Convert(parseNode.Operand);
        var binaryOperation = new BinaryOperationAstNode(parseNode.Operator, operand0, operand1);

        if (parseNode.Post is PostBinaryOperationParseNode postMultiplication)
        {
            return ConvertPostBinaryOperation(postMultiplication, binaryOperation);
        }
        else if (parseNode.Post is EmptyParseNode)
        {
            return binaryOperation;
        }
        else
        {
            var property = $"{nameof(PostBinaryOperationParseNode)}.{nameof(PostBinaryOperationParseNode.Post)}";
            throw new Exception($"{property} was {parseNode.Post.GetType().Name} which is not allowed.");
        }
    }

    private static IntAstNode ConvertIntLiteral(IntLiteralParseNode parseNode)
    {
        return new IntAstNode(parseNode.Value);
    }

    private static StringAstNode ConvertStringLiteral(StringLiteralParseNode parseNode)
    {
        return new StringAstNode(parseNode.Value);
    }

    private static BoolAstNode ConvertBoolLiteral(BoolLiteralParseNode parseNode)
    {
        return new BoolAstNode(parseNode.Value);
    }

    private static VariableReferenceAstNode ConvertIdentifierExpression(IdentifierExpressionParseNode parseNode)
    {
        return new VariableReferenceAstNode(parseNode.Identifier);
    }

    private static UnaryOperationAstNode ConvertUnaryOperation(UnaryOperationParseNode parseNode)
    {
        return new UnaryOperationAstNode(parseNode.Operator, Convert(parseNode.Operand));
    }
}
