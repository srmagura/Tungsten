using TungstenCompiler.Lexer;

namespace TungstenCompiler.Parser.Expression;

internal static class TerminalParser
{
    internal static ParseResult<ParseNode>? Parse(Token[] tokens, int position)
    {
        var identifierExpression = ParseIdentifierExpression(tokens, position);
        if (identifierExpression != null)
            return ParseResultUtil.Generic(identifierExpression);

        var intLiteral = ParseIntLiteral(tokens, position);
        if (intLiteral != null)
            return ParseResultUtil.Generic(intLiteral);

        var stringLiteral = ParseStringLiteral(tokens, position);
        if (stringLiteral != null)
            return ParseResultUtil.Generic(stringLiteral);

        var boolLiteral = ParseBoolLiteral(tokens, position);
        if (boolLiteral != null)
            return ParseResultUtil.Generic(boolLiteral);

        return null;
    }

    private static ParseResult<IntLiteralParseNode>? ParseIntLiteral(Token[] tokens, int position)
    {
        if (position >= tokens.Length) return null;

        if (tokens[position].Type != TokenType.IntLiteral)
            return null;

        var value = int.Parse(tokens[position].Value);

        return new ParseResult<IntLiteralParseNode>(
            new IntLiteralParseNode(value),
            1
        );
    }

    private static ParseResult<StringLiteralParseNode>? ParseStringLiteral(Token[] tokens, int position)
    {
        if (position >= tokens.Length) return null;

        if (tokens[position].Type != TokenType.StringLiteral)
            return null;

        // Remove quotes
        var value = tokens[position].Value[1..^1];

        return new ParseResult<StringLiteralParseNode>(
            new StringLiteralParseNode(value),
            1
        );
    }

    private static ParseResult<BoolLiteralParseNode>? ParseBoolLiteral(Token[] tokens, int position)
    {
        if (position >= tokens.Length) return null;

        return tokens[position].Type switch
        {
            TokenType.FalseKeyword => new ParseResult<BoolLiteralParseNode>(new BoolLiteralParseNode(false), 1),
            TokenType.TrueKeyword => new ParseResult<BoolLiteralParseNode>(new BoolLiteralParseNode(true), 1),
            _ => null
        };
    }

    private static ParseResult<IdentifierExpressionParseNode>? ParseIdentifierExpression(Token[] tokens, int position)
    {
        if (position >= tokens.Length) return null;

        if (tokens[position].Type != TokenType.Identifier)
            return null;

        return new ParseResult<IdentifierExpressionParseNode>(
            new IdentifierExpressionParseNode(tokens[position].Value),
            1
        );
    }
}
