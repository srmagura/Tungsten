using Tungsten.Compiler.Lexer;
using Tungsten.Compiler.Parser;

namespace Tungsten.Compiler.Parser.Expression;

internal static class MultiplicationParser
{
    private static readonly Dictionary<TokenType, BinaryOperator> TokenToOperatorMap = new()
    {
        [TokenType.Asterisk] = BinaryOperator.Multiplication,
        [TokenType.Slash] = BinaryOperator.Division,
        [TokenType.DoubleBackslash] = BinaryOperator.IntegerDivision,
        [TokenType.PercentSign] = BinaryOperator.Modulus,
    };

    internal static ParseResult<ParseNode>? Parse(Token[] tokens, int position)
    {
        return BinaryOperationParser.Parse(
            TokenToOperatorMap,
            ParenthesizedExpressionParser.Parse,
            tokens,
            position
        );
    }
}
