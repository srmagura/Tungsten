using TFlat.Compiler.Lexer;

namespace TFlat.Compiler.Parser.Expression;

internal static class UnaryOperationParser
{
    internal static ParseResult<ParseNode>? Parse(Token[] tokens, int position)
    {
        var unaryOperation = ParseUnaryOperation(tokens, position);
        if(unaryOperation != null)
            return ParseResultUtil.Generic(unaryOperation);

        return ExponentiationParser.Parse(tokens, position);
    }

    private static ParseResult<UnaryOperationParseNode>? ParseUnaryOperation(Token[] tokens, int position)
    {
        if (position >= tokens.Length) return null;

        var i = position;

        UnaryOperator? unaryOperator = tokens[i].Type switch
        {
            TokenType.Minus => UnaryOperator.NumericNegation,
            _ => null
        };

        if (unaryOperator == null) return null;
        i++;

        var expressionResult = ParenthesizedExpressionParser.Parse(tokens, i); 
        if (expressionResult == null) return null;
        i += expressionResult.ConsumedTokens;

        return new ParseResult<UnaryOperationParseNode>(
            new UnaryOperationParseNode(unaryOperator.Value, expressionResult.Node),
            i - position
        );
    }
}
