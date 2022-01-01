using Tungsten.Compiler.AST;
using Tungsten.Compiler.Emit;
using Tungsten.Compiler.Lexer;
using Tungsten.Compiler.Parser.Module;

namespace Tungsten.Compiler;

public static class TungstenCompiler
{
    private static void CompileModule(string projectName, string moduleName, string code, string outputDirectory)
    {
        var tokens = TheLexer.Lex(code);
        var parseTree = ModuleParser.Parse(tokens);
        var moduleAst = ModuleToAst.Convert(parseTree, moduleName);
        var ast = new AssemblyAstNode(projectName, moduleAst);

        ProjectEmitter.Emit(ast, outputDirectory);
    }

    private static string GetModulePath(string projectDirectory)
    {
        var files = Directory.EnumerateFiles(projectDirectory, "*.w").ToArray();

        if (files.Length == 0)
            throw new Exception($"There are no .w files in {projectDirectory}.");

        return files.First();
    }

    public static void Compile(string projectPath, string? outputDirectory)
    {
        var projectName = Path.GetFileNameWithoutExtension(projectPath);

        var projectDirectory = Path.GetDirectoryName(projectPath);
        if (projectDirectory == null)
            throw new Exception("Could not get project directory.");

        var modulePath = GetModulePath(projectDirectory);
        var moduleName = Path.GetFileNameWithoutExtension(modulePath);
        if (moduleName == null)
            throw new Exception($"Could not get module name from path {modulePath}.");

        var code = File.ReadAllText(modulePath);

        outputDirectory ??= Path.Combine(projectDirectory, "bin");
        if (!Directory.Exists(outputDirectory))
            Directory.CreateDirectory(outputDirectory);

        CompileModule(projectName, moduleName, code, outputDirectory);
    }
}
