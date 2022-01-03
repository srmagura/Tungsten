using Tungsten.Compiler.Lexer;

namespace Tungsten.Compiler.Parser.Module;

internal static class TypeParser
{
    internal static ParseResult<TypeParseNode>? Parse(Token[] tokens, int position)
    {
        var i = position;

        switch (tokens[i].Type)
        {
            case TokenType.IntKeyword:
                return new ParseResult<TypeParseNode>(
                    new TypeParseNode(WType.Int),
                    1
                );
            case TokenType.StringKeyword:
                return new ParseResult<TypeParseNode>(
                    new TypeParseNode(WType.String),
                    1
                );
        }

        return null;
    }
}
