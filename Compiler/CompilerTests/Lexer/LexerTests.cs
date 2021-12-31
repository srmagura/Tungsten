using TungstenCompiler.Lexer;

namespace CompilerTests.Lexer;

[TestClass]
public class LexerTests
{
    [TestMethod]
    public void ItConsumesWhitespace()
    {
        Assert.AreEqual(0, TheLexer.Lex("\n \t   \r\n").Length);
    }

    [TestMethod]
    public void Identifier()
    {
        var token = TheLexer.Lex("print").Single();
        Assert.AreEqual(new Token(TokenType.Identifier, "print", 1, 1), token);
    }

    [TestMethod]
    public void IdentifierWithWhitespace()
    {
        var token = TheLexer.Lex("\n\t  print\r\n").Single();
        Assert.AreEqual(new Token(TokenType.Identifier, "print", 2, 4), token);
    }

    [TestMethod]
    public void MultipleIdentifiers()
    {
        var tokens = TheLexer.Lex("a b");

        var expected = new List<Token>
        {
            new Token(TokenType.Identifier, "a", 1, 1),
            new Token(TokenType.Identifier, "b", 1, 3),
        };

        CollectionAssert.AreEqual(expected, tokens);
    }

    [TestMethod]
    public void IntLiterals()
    {
        var token = TheLexer.Lex("0").Single();
        Assert.AreEqual(new Token(TokenType.IntLiteral, "0", 1, 1), token);

        token = TheLexer.Lex("34").Single();
        Assert.AreEqual(new Token(TokenType.IntLiteral, "34", 1, 1), token);

        Assert.AreEqual(2, TheLexer.Lex("-0").Length);
    }

    [TestMethod]
    public void StringLiterals()
    {
        var token = TheLexer.Lex("\"\"").Single();
        Assert.AreEqual(new Token(TokenType.StringLiteral, "\"\"", 1, 1), token);

        token = TheLexer.Lex("\"foo\"").Single();
        Assert.AreEqual(new Token(TokenType.StringLiteral, "\"foo\"", 1, 1), token);

        token = TheLexer.Lex("\"foo bar{\"").Single();
        Assert.AreEqual(new Token(TokenType.StringLiteral, "\"foo bar{\"", 1, 1), token);
    }

    [TestMethod]
    public void ItThrowsOnUnterminatedStringLiterals()
    {
        Assert.ThrowsException<Exception>(() => TheLexer.Lex("\""));
        Assert.ThrowsException<Exception>(() => TheLexer.Lex("\"\n\""));
    }

    [TestMethod]
    public void Separators()
    {
        var token = TheLexer.Lex("(").Single();
        Assert.AreEqual(new Token(TokenType.OpenParenthesis, "(", 1, 1), token);

        token = TheLexer.Lex(")").Single();
        Assert.AreEqual(new Token(TokenType.CloseParenthesis, ")", 1, 1), token);

        token = TheLexer.Lex("{").Single();
        Assert.AreEqual(new Token(TokenType.OpenCurlyBrace, "{", 1, 1), token);

        token = TheLexer.Lex("}").Single();
        Assert.AreEqual(new Token(TokenType.CloseCurlyBrace, "}", 1, 1), token);

        token = TheLexer.Lex(":").Single();
        Assert.AreEqual(new Token(TokenType.Colon, ":", 1, 1), token);
    }

    [TestMethod]
    public void Keywords()
    {
        var token = TheLexer.Lex("export").Single();
        Assert.AreEqual(new Token(TokenType.ExportKeyword, "export", 1, 1), token);

        token = TheLexer.Lex("fun").Single();
        Assert.AreEqual(new Token(TokenType.FunKeyword, "fun", 1, 1), token);

        token = TheLexer.Lex("void").Single();
        Assert.AreEqual(new Token(TokenType.VoidKeyword, "void", 1, 1), token);
    }

    [TestMethod]
    public void Operators()
    {
        var token = TheLexer.Lex("+").Single();
        Assert.AreEqual(new Token(TokenType.Plus, "+", 1, 1), token);

        token = TheLexer.Lex("-").Single();
        Assert.AreEqual(new Token(TokenType.Minus, "-", 1, 1), token);

        token = TheLexer.Lex("*").Single();
        Assert.AreEqual(new Token(TokenType.Asterisk, "*", 1, 1), token);

        token = TheLexer.Lex("**").Single();
        Assert.AreEqual(new Token(TokenType.DoubleAsterisk, "**", 1, 1), token);

        token = TheLexer.Lex("/").Single();
        Assert.AreEqual(new Token(TokenType.Slash, "/", 1, 1), token);

        token = TheLexer.Lex(@"\\").Single();
        Assert.AreEqual(new Token(TokenType.DoubleBackslash, @"\\", 1, 1), token);
    }

    private static SimpleToken ToSimpleToken(Token token)
    {
        return new SimpleToken(token.Type, token.Value);
    }

    [TestMethod]
    public void PrintIntLiteral()
    {
        var simpleTokens = TheLexer.Lex("print(3);")
            .Select(ToSimpleToken)
            .ToList();

        var expected = new List<SimpleToken>
        {
            new SimpleToken(TokenType.Identifier, "print"),
            new SimpleToken(TokenType.OpenParenthesis, "("),
            new SimpleToken(TokenType.IntLiteral, "3"),
            new SimpleToken(TokenType.CloseParenthesis, ")"),
            new SimpleToken(TokenType.Semicolon, ";"),
        };

        CollectionAssert.AreEqual(expected, simpleTokens);
    }

    [TestMethod]
    public void ConstVariable()
    {
        var simpleTokens = TheLexer.Lex(@"const a: string = ""apple"";")
            .Select(ToSimpleToken)
            .ToList();

        var expected = new List<SimpleToken>
        {
            new SimpleToken(TokenType.ConstKeyword, "const"),
            new SimpleToken(TokenType.Identifier, "a"),
            new SimpleToken(TokenType.Colon, ":"),
            new SimpleToken(TokenType.StringKeyword, "string"),
            new SimpleToken(TokenType.SingleEqual, "="),
            new SimpleToken(TokenType.StringLiteral, @"""apple"""),
            new SimpleToken(TokenType.Semicolon, ";"),
        };

        CollectionAssert.AreEqual(expected, simpleTokens);
    }

    [TestMethod]
    public void HelloWorld()
    {
        var code = @"
fun main(): void {
    print(""hello world"");
}
        ";

        var simpleTokens = TheLexer.Lex(code)
            .Select(ToSimpleToken)
            .ToList();

        var expected = new List<SimpleToken>
        {
            new SimpleToken(TokenType.FunKeyword, "fun"),
            new SimpleToken(TokenType.Identifier, "main"),
            new SimpleToken(TokenType.OpenParenthesis, "("),
            new SimpleToken(TokenType.CloseParenthesis, ")"),
            new SimpleToken(TokenType.Colon, ":"),
            new SimpleToken(TokenType.VoidKeyword, "void"),
            new SimpleToken(TokenType.OpenCurlyBrace, "{"),
            new SimpleToken(TokenType.Identifier, "print"),
            new SimpleToken(TokenType.OpenParenthesis, "("),
            new SimpleToken(TokenType.StringLiteral, "\"hello world\""),
            new SimpleToken(TokenType.CloseParenthesis, ")"),
            new SimpleToken(TokenType.Semicolon, ";"),
            new SimpleToken(TokenType.CloseCurlyBrace, "}"),
        };

        CollectionAssert.AreEqual(expected, simpleTokens);
    }
}
