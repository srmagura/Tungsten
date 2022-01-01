using System.Text;

namespace Tungsten.Compiler.Lexer;

internal static class TheLexer
{
    public static Token[] Lex(string s)
    {
        s = Preprocess(s);

        var position = 0;
        var tokens = new List<Token>();

        var line = 1;
        var lineStartPosition = 0;

        // Returns true if any whitespace consumed
        bool ConsumeWhitespace()
        {
            var anyConsumed = false;

            while (position < s.Length)
            {
                if (!char.IsWhiteSpace(s[position]))
                    break;

                if (s[position] == '\n')
                {
                    line++;
                    lineStartPosition = position + 1;
                }

                position++;
                anyConsumed = true;
            }

            return anyConsumed;
        }

        // Returns true if func returned a token
        bool TryLex(Func<string, int, SimpleToken?> func)
        {
            var simpleToken = func(s, position);
            if (simpleToken == null) return false;

            tokens.Add(
                new Token(
                    simpleToken.Type,
                    simpleToken.Value,
                    line,
                    position - lineStartPosition + 1
                )
            );
            position += simpleToken.Value.Length;
            return true;
        }

        while (position < s.Length)
        {
            if (ConsumeWhitespace()) continue;

            if (TryLex(LexIntLiteral)) continue;
            if (TryLex(LexStringLiteral)) continue;
            if (TryLex(LexKeyword)) continue;
            if (TryLex(LexOperator)) continue;
            if (TryLex(LexIdentifier)) continue;
            if (TryLex(LexSeparator)) continue;

            throw new Exception("Failed to identify the next token.");
        }

        return tokens.ToArray();
    }

    private static string Preprocess(string s)
    {
        return s.Replace("\r\n", "\n");
    }

    private static SimpleToken? LexKeyword(string s, int position)
    {
        var sb = new StringBuilder();

        for (var i = position; i < s.Length; i++)
        {
            if (!char.IsLower(s[i])) break;
            sb.Append(s[i]);
        }

        var keyword = sb.ToString();

        return keyword switch
        {
            "export" => new SimpleToken(TokenType.ExportKeyword, keyword),
            "fun" => new SimpleToken(TokenType.FunKeyword, keyword),
            "let" => new SimpleToken(TokenType.LetKeyword, keyword),
            "const" => new SimpleToken(TokenType.ConstKeyword, keyword),

            "void" => new SimpleToken(TokenType.VoidKeyword, keyword),
            "int" => new SimpleToken(TokenType.IntKeyword, keyword),
            "string" => new SimpleToken(TokenType.StringKeyword, keyword),
            "bool" => new SimpleToken(TokenType.BoolKeyword, keyword),

            "null" => new SimpleToken(TokenType.NullKeyword, keyword),
            "false" => new SimpleToken(TokenType.FalseKeyword, keyword),
            "true" => new SimpleToken(TokenType.TrueKeyword, keyword),

            _ => null
        };
    }

    private static SimpleToken? LexOperator(string s, int position)
    {
        if (position + 1 < s.Length)
        {
            var cc = s.Substring(position, 2);

            switch (cc)
            {
                case "**": return new SimpleToken(TokenType.DoubleAsterisk, cc);
                case @"\\": return new SimpleToken(TokenType.DoubleBackslash, cc);
            };
        }

        var c = s[position];

        return c switch
        {
            '+' => new SimpleToken(TokenType.Plus, c.ToString()),
            '-' => new SimpleToken(TokenType.Minus, c.ToString()),
            '*' => new SimpleToken(TokenType.Asterisk, c.ToString()),
            '/' => new SimpleToken(TokenType.Slash, c.ToString()),
            '%' => new SimpleToken(TokenType.PercentSign, c.ToString()),
            '=' => new SimpleToken(TokenType.SingleEqual, c.ToString()),

            _ => null
        };
    }

    private static SimpleToken? LexIdentifier(string s, int position)
    {
        var sb = new StringBuilder();

        for (var i = position; i < s.Length; i++)
        {
            var isLetterOrUnderscore = char.IsLetter(s[i]) || s[i] == '_';

            if (i == position && !isLetterOrUnderscore) return null;
            if (!isLetterOrUnderscore && !char.IsDigit(s[i])) break;

            sb.Append(s[i]);
        }

        if (sb.Length == 0) return null;

        return new SimpleToken(TokenType.Identifier, sb.ToString());
    }

    private static SimpleToken? LexIntLiteral(string s, int position)
    {
        var sb = new StringBuilder();

        for (var i = position; i < s.Length; i++)
        {
            if (!char.IsDigit(s[i])) break;

            sb.Append(s[i]);
        }

        if (sb.Length == 0) return null;

        return new SimpleToken(TokenType.IntLiteral, sb.ToString());
    }

    private static SimpleToken? LexStringLiteral(string s, int position)
    {
        if (s[position] != '"') return null;

        var sb = new StringBuilder();

        for (var i = position + 1; i < s.Length; i++)
        {
            if (s[i] == '"')
            {
                return new SimpleToken(
                    TokenType.StringLiteral,
                    s.Substring(position, i - position + 1)
                );
            }

            if (s[i] == '\n') return null;

            sb.Append(s[i]);
        }

        return null;
    }

    private static SimpleToken? LexSeparator(string s, int position)
    {
        return s[position] switch
        {
            ';' => new SimpleToken(TokenType.Semicolon, s[position].ToString()),
            ':' => new SimpleToken(TokenType.Colon, s[position].ToString()),
            '(' => new SimpleToken(TokenType.OpenParenthesis, s[position].ToString()),
            ')' => new SimpleToken(TokenType.CloseParenthesis, s[position].ToString()),
            '{' => new SimpleToken(TokenType.OpenCurlyBrace, s[position].ToString()),
            '}' => new SimpleToken(TokenType.CloseCurlyBrace, s[position].ToString()),
            _ => null
        };
    }
}
