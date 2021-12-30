using System.Text.Json;
using System.Text.Json.Serialization;

namespace CompilerTests;

public abstract class AstTest
{
    private static readonly JsonSerializerOptions JsonSerializerOptions = new()
    {
        Converters = { new AstSerializer.AstNodeWriteConverter(), new JsonStringEnumConverter() },
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
