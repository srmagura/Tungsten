using TFlat.Compiler.Lexer;

namespace TFlat.Compiler.Parser.Expression;

internal static class BinaryOperationParser
{
    internal static ParseResult<ParseNode>? Parse(
        Dictionary<TokenType, BinaryOperator> tokenToOperatorMap,
        Func<Token[], int, ParseResult<ParseNode>?> parseOperand,
        Token[] tokens,
        int position
    )
    {
        var i = position;

        var operandResult = parseOperand(tokens, i);
        if (operandResult == null) return null;
        i += operandResult.ConsumedTokens;

        var postResult = ParsePostBinaryOperation(tokenToOperatorMap, parseOperand, tokens, i);
        if (postResult == null) return null;
        i += postResult.ConsumedTokens;

        return new ParseResult<ParseNode>(
            new BinaryOperationParseNode(operandResult.Node, postResult.Node),
            i - position
        );
    }

    private static ParseResult<ParseNode>? ParsePostBinaryOperation(
        Dictionary<TokenType, BinaryOperator> tokenToOperatorMap,
        Func<Token[], int, ParseResult<ParseNode>?> parseOperand,
        Token[] tokens,
        int position
    )
    {
        var result = ParsePostBinaryOperationCore(tokenToOperatorMap, parseOperand, tokens, position);
        if (result != null)
            return ParseResultUtil.Generic(result);

        return ParseResultUtil.Empty;
    }

    private static ParseResult<PostBinaryOperationParseNode>? ParsePostBinaryOperationCore(
        Dictionary<TokenType, BinaryOperator> tokenToOperatorMap,
        Func<Token[], int, ParseResult<ParseNode>?> parseOperand,
        Token[] tokens, 
        int position
    )
    {
        if (position >= tokens.Length) return null;

        var i = position;

        if (!tokenToOperatorMap.TryGetValue(tokens[i].Type, out var binaryOperator))
            return null;
        i++;

        var operandResult = parseOperand(tokens, i);
        if (operandResult == null) return null;
        i += operandResult.ConsumedTokens;

        var postExpressionResult = ParsePostBinaryOperation(tokenToOperatorMap, parseOperand, tokens, i);
        if (postExpressionResult == null) return null;
        i += postExpressionResult.ConsumedTokens;

        return new ParseResult<PostBinaryOperationParseNode>(
            new PostBinaryOperationParseNode(binaryOperator, operandResult.Node, postExpressionResult.Node),
            i - position
        );
    }
}
