using CommandLine;
using Tungsten.Compiler;

namespace Tungsten.CLI;

internal static class CLIProgram
{
    private static string GetProjectPath(string? projectPath)
    {
        if (projectPath == null)
        {
            var projectFiles = Directory.EnumerateFiles(Directory.GetCurrentDirectory(), "*.wproj").ToArray();

            if (projectFiles.Length == 0)
                throw new Exception("There is no .wproj or .wsln file in the current directory.");

            if (projectFiles.Length > 1)
                throw new Exception("There are multiple .wproj or .wsln files in the current directory.");

            projectPath = projectFiles[0];
        }

        return Path.GetFullPath(projectPath);
    }

    [Verb("build", HelpText = "Compile a Tungsten project or solution.")]
    internal class BuildOptions
    {
        [Value(0, MetaName = "project", HelpText = "The path to the .wproj or .wsln to build.")]
        public string? Project { get; set; }
    }

    private static int Build(BuildOptions options)
    {
        var projectPath = GetProjectPath(options.Project);
        TungstenCompiler.Compile(projectPath, outputDirectory: null);

        return 0;
    }

    [Verb("clean")]
    internal class CleanOptions { }

    private static int Clean(CleanOptions options)
    {
        throw new NotImplementedException();
    }

    private static int HandleParseArgumentsErrors(IEnumerable<Error> errors)
    {
        Console.Error.WriteLine("Argument parsing failed.");
        return 1;
    }

    internal static int Main(string[] args)
    {
        return Parser.Default.ParseArguments<BuildOptions, CleanOptions>(args)
             .MapResult<BuildOptions, CleanOptions, int>(
                 Build,
                 Clean,
                 HandleParseArgumentsErrors
             );
    }
}
