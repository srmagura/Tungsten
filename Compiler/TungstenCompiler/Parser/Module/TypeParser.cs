using TungstenCompiler.Lexer;

namespace TungstenCompiler.Parser.Module;

internal static class TypeParser
{
    internal static ParseResult<TypeParseNode>? Parse(Token[] tokens, int position)
    {
        var i = position;

        switch (tokens[i].Type)
        {
            case TokenType.IntKeyword:
                return new ParseResult<TypeParseNode>(
                    new TypeParseNode("int"),
                    1
                );
            case TokenType.StringKeyword:
                return new ParseResult<TypeParseNode>(
                    new TypeParseNode("string"),
                    1
                );
        }

        return null;
    }
}
