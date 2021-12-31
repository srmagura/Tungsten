namespace TungstenCompiler.Lexer;

internal enum TokenType
{
    Identifier,

    // Literals
    IntLiteral,
    StringLiteral,

    // Keywords
    ExportKeyword,
    FunKeyword,
    LetKeyword,
    ConstKeyword,

    VoidKeyword,
    IntKeyword,
    StringKeyword,
    BoolKeyword,

    NullKeyword,
    FalseKeyword,
    TrueKeyword,

    // Separators
    Semicolon,
    Colon,
    OpenParenthesis,
    CloseParenthesis,
    OpenCurlyBrace,
    CloseCurlyBrace,

    // Operators
    Plus,
    Minus,
    Asterisk,
    DoubleAsterisk,
    Slash,
    DoubleBackslash,
    SingleEqual,
    PercentSign
}

internal record SimpleToken(TokenType Type, string Value);

internal record Token(TokenType Type, string Value, int Line, int Column);
