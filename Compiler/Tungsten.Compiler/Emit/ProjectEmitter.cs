using Mono.Cecil;
using System.Text.Json;
using Tungsten.Compiler.AST;

namespace Tungsten.Compiler.Emit;

internal static class ProjectEmitter
{
    internal static void Emit(AssemblyAstNode ast, string outputDirectory)
    {
        var assembly = AssemblyEmitter.Emit(ast);

        var dllPath = Path.Combine(outputDirectory, $"{ast.Name}.dll");
        assembly.Write(dllPath, new WriterParameters());

        var runtimeOptions = new
        {
            runtimeOptions = new
            {
                tfm = "net6.0",
                framework = new
                {
                    name = "Microsoft.NETCore.App",
                    version = "6.0.0"
                }
            }
        };

        var runtimeOptionsJson = JsonSerializer.Serialize(runtimeOptions);
        var runtimeOptionsPath = Path.Combine(outputDirectory, $"{ast.Name}.runtimeconfig.json");
        File.WriteAllText(runtimeOptionsPath, runtimeOptionsJson);
    }
}
