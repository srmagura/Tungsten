using CommandLine;
using System.Text;
using TFlat.Compiler.AST;
using TFlat.Compiler.Lexer;
using TFlat.Compiler.Parser.Module;

namespace TFlat.Compiler;

public static class CompilerProgram
{
    public class Options
    {
        [Value(0, Required = true)]
        public string? ModulePath { get; set; }
    }

    public static async Task CompileModuleAsync(string code, Stream dllStream)
    {
        var tokens = TheLexer.Lex(code);
        var parseTree = ModuleParser.Parse(tokens);
        var ast = ModuleToAst.Convert(parseTree);
        
        await AstSerializer.SerializeAsync(ast, dllStream);
    }

    private static async Task RunAsync(Options options)
    {
        if (options.ModulePath == null)
            throw new Exception("ModulePath is null.");

        string code;

        using (var codeStream = File.OpenRead(options.ModulePath))
        {
            using var streamReader = new StreamReader(codeStream, Encoding.UTF8);
            code = await streamReader.ReadToEndAsync();
        }

        var moduleDirectory = Path.GetDirectoryName(options.ModulePath);
        if (moduleDirectory == null)
            throw new Exception("Could not get module directory.");

        var dllDirectory = Path.Combine(moduleDirectory, "bin");
        if (!Directory.Exists(dllDirectory))
            Directory.CreateDirectory(dllDirectory);

        var dllPath = Path.Combine(dllDirectory, "Program.dll");

        using var dllStream = File.OpenWrite(dllPath);
        await CompileModuleAsync(code, dllStream);
    }

    internal static async Task Main(string[] args)
    {
        await CommandLine.Parser.Default.ParseArguments<Options>(args)
            .WithParsedAsync(RunAsync);
    }
}
