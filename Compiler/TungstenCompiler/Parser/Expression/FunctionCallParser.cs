using TFlat.Compiler.Lexer;

namespace TFlat.Compiler.Parser.Expression;

internal static class FunctionCallParser
{
    internal static ParseResult<FunctionCallParseNode>? Parse(Token[] tokens, int position)
    {
        var i = position;

        if (tokens[i].Type != TokenType.Identifier) return null;
        i++;

        if (tokens[i].Type != TokenType.OpenParenthesis) return null;
        i++;

        if (tokens[i].Type == TokenType.CloseParenthesis)
        {
            // Function call with no arguments
            i++;
            var argumentList = new ArgumentListParseNode(Array.Empty<ParseNode>());

            return new ParseResult<FunctionCallParseNode>(
                new FunctionCallParseNode(tokens[position].Value, argumentList),
                i - position
            );
        }

        var argumentListResult = ParseArgumentList(tokens, i);
        if (argumentListResult == null) return null;
        i += argumentListResult.ConsumedTokens;

        if (tokens[i].Type != TokenType.CloseParenthesis) return null;
        i++;

        return new ParseResult<FunctionCallParseNode>(
            new FunctionCallParseNode(tokens[position].Value, argumentListResult.Node),
            i - position
        );
    }

    private static ParseResult<ArgumentListParseNode>? ParseArgumentList(Token[] tokens, int position)
    {
        // TODO support 2+ arguments
        var arg0Result = ExpressionParser.Parse(tokens, position);
        if (arg0Result == null) return null;

        return new ParseResult<ArgumentListParseNode>(
            new ArgumentListParseNode(new[] { arg0Result.Node }),
            arg0Result.ConsumedTokens
        );
    }
}
