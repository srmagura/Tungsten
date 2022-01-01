using Tungsten.Compiler.AST;
using Tungsten.Compiler.Tests;
using Tungsten.Compiler.Lexer;
using Tungsten.Compiler.Parser;

namespace Tungsten.Compiler.Tests.Parser;

public abstract class ParserTest : AstTest
{
    internal static void TestDoesNotParseCore<T>(
        Func<Token[], int, ParseResult<T>?> parse,
        string code
    )
        where T : ParseNode
    {
        var tokens = TheLexer.Lex(code);
        var parseResult = parse(tokens, 0);
        Assert.IsNull(parseResult, "The parse result was non-null.");
    }

    internal static void TestParseCore<T>(
        Func<Token[], int, ParseResult<T>?> parse,
        Func<ParseNode, AstNode> convertToAst,
        string code,
        AstNode expected
    )
        where T : ParseNode
    {
        var tokens = TheLexer.Lex(code);
        var parseResult = parse(tokens, 0);
        Assert.IsNotNull(parseResult, "The parse result is null.");
        Assert.AreEqual(
            tokens.Length,
            parseResult.ConsumedTokens,
            "It did not consume all the tokens."
        );

        var actual = convertToAst(parseResult.Node);
        AssertAstsEqual(expected, actual);
    }
}
