using Tungsten.Compiler.Lexer;

namespace Tungsten.Compiler.Parser.Expression;

internal static class ExponentiationParser
{
    private static readonly Dictionary<TokenType, BinaryOperator> TokenToOperatorMap = new()
    {
        [TokenType.DoubleAsterisk] = BinaryOperator.Exponentiation
    };

    internal static ParseResult<ParseNode>? Parse(Token[] tokens, int position)
    {
        return BinaryOperationParser.Parse(
            TokenToOperatorMap,
            TerminalParser.Parse,
            tokens,
            position
        );
    }
}
