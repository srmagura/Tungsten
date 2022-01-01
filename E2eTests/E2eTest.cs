using System.Diagnostics;
using Tungsten.Compiler;

namespace E2eTests;

public class E2eTest
{
    protected async Task<string> BuildAndRunAsync(string projectName)
    {
        var outputDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "E2eTestOutput");
        if (Directory.Exists(outputDirectory))
            Directory.Delete(outputDirectory, recursive: true);

        var e2eTestsDirectory = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory)!.Parent!.Parent!.Parent!.FullName;
        var projectPath = Path.Combine(e2eTestsDirectory, projectName, $"{projectName}.wproj");
        TungstenCompiler.Compile(projectPath, outputDirectory);

        var dllPath = Path.Combine(outputDirectory, $"{projectName}.dll");

        var processStartInfo = new ProcessStartInfo("dotnet", dllPath)
        {
            RedirectStandardOutput = true
        };

        using var process = Process.Start(processStartInfo);
        Assert.IsNotNull(process);

        await process.WaitForExitAsync();

        var output = await process.StandardOutput.ReadToEndAsync();
        output = output.ReplaceLineEndings("\n");

        // Remove final newline
        if (output.Length > 0 && output[^1] == '\n')
            output = output[0..^1];

        return output;
    }
}
