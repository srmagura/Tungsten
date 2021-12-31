using System.Text.Json;
using System.Text.Json.Serialization;

namespace CompilerTests;

public abstract class AstTest
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
            writer.WriteString("Type", nodeType.Name.Replace("AstNode", ""));

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

    protected static string SerializeAst(AstNode ast)
    {
        return JsonSerializer.Serialize(ast, JsonSerializerOptions);
    }

    protected static void AssertAstsEqual(AstNode expected, AstNode? actual)
    {
        Assert.IsNotNull(actual);
        Assert.AreEqual(SerializeAst(expected), SerializeAst(actual));
    }
}
