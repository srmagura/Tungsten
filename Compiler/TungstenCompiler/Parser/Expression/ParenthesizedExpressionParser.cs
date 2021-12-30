using TFlat.Compiler.Lexer;

namespace TFlat.Compiler.Parser.Expression;

internal static class ParenthesizedExpressionParser
{
    internal static ParseResult<ParseNode>? Parse(Token[] tokens, int position)
    {
        var parenthesizedExpression = ParseParenthesized(tokens, position);
        if(parenthesizedExpression != null) return parenthesizedExpression;

        return UnaryOperationParser.Parse(tokens, position);
    }

    private static ParseResult<ParseNode>? ParseParenthesized(Token[] tokens, int position)
    {
        if(position >= tokens.Length) return null;

        var i = position;
     
        if (tokens[i].Type != TokenType.OpenParenthesis) return null;
        i++;

        var expressionResult = ExpressionParser.Parse(tokens, i);
        if (expressionResult == null) return null;
        i += expressionResult.ConsumedTokens;

        if (tokens[i].Type != TokenType.CloseParenthesis) return null;
        i++;

        return new ParseResult<ParseNode>(
            expressionResult.Node,
            i - position
        );
    }
}
