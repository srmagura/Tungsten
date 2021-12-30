using System.IO.Compression;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace TFlat.Compiler.AST;

internal class AstSerializer
{
    internal class AstNodeWriteConverter : JsonConverter<AstNode>
    {
        public override AstNode? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            throw new NotSupportedException();
        }

        public override void Write(Utf8JsonWriter writer, AstNode value, JsonSerializerOptions options)
        {
            // typeof(object) makes it serialize the properties of the runtime type,
            // not the declared type
            JsonSerializer.Serialize(writer, value, typeof(object), options);
        }
    }

    private static readonly JsonSerializerOptions JsonSerializerOptions = new()
    {
        Converters = { new AstNodeWriteConverter() },
    };

    public static async Task SerializeAsync(ModuleAstNode ast, Stream outStream)
    {
        using var brotliStream = new BrotliStream(outStream, CompressionLevel.Fastest, leaveOpen: true);

        await JsonSerializer.SerializeAsync(brotliStream, ast, JsonSerializerOptions);
        await brotliStream.FlushAsync();
    }
}
