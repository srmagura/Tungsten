using TungstenCompiler.Lexer;

namespace TungstenCompiler.Parser.Expression;

internal static class AdditionParser
{
    private static readonly Dictionary<TokenType, BinaryOperator> TokenToOperatorMap = new()
    {
        [TokenType.Plus] = BinaryOperator.Addition,
        [TokenType.Minus] = BinaryOperator.Subtraction,
    };

    internal static ParseResult<ParseNode>? Parse(Token[] tokens, int position)
    {
        return BinaryOperationParser.Parse(
            TokenToOperatorMap,
            MultiplicationParser.Parse,
            tokens,
            position
        );
    }
}
