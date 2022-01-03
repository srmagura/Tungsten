using Tungsten.Compiler.Lexer;
using Tungsten.Compiler.Parser;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace Tungsten.Compiler.Tests.CodeToAst;

public abstract class CodeToAstTest
{
    private class AstNodeWriteConverter : JsonConverter<AstNode>
    {
        public override Boolean CanConvert(Type typeToConvert)
        {
            return typeToConvert.IsAssignableTo(typeof(AstNode));
        }

        public override AstNode? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            throw new NotSupportedException();
        }

        public override void Write(Utf8JsonWriter writer, AstNode value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();

            var nodeType = value.GetType();
            writer.WriteString("NodeType", nodeType.Name.Replace("AstNode", ""));

            foreach (var property in nodeType.GetProperties())
            {
                writer.WritePropertyName(property.Name);
                var propertyValue = property.GetValue(value);

                if (property.PropertyType.IsArray && propertyValue != null)
                {
                    writer.WriteStartArray();

                    foreach (var element in (IEnumerable<AstNode>)propertyValue)
                    {
                        JsonSerializer.Serialize(writer, element, options);
                    }

                    writer.WriteEndArray();
                }
                else
                {
                    JsonSerializer.Serialize(writer, propertyValue, options);
                }
            }

            writer.WriteEndObject();
        }
    }

    private static readonly JsonSerializerOptions JsonSerializerOptions = new()
    {
        Converters = { new AstNodeWriteConverter() },
        WriteIndented = true
    };

    internal static string SerializeAst(AstNode ast)
    {
        return JsonSerializer.Serialize(ast, JsonSerializerOptions);
    }

    internal static void AssertAstsEqual(AstNode expected, AstNode? actual)
    {
        Assert.IsNotNull(actual);
        Assert.AreEqual(SerializeAst(expected), SerializeAst(actual));
    }

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
