using Tungsten.Compiler.Emit;
using Tungsten.Compiler.Lexer;
using Tungsten.Compiler.Parser.Module;

namespace Tungsten.Compiler.Tests.Emit;

[TestClass]
public class ProgramEmitterTests
{
    [TestMethod]
    public void HelloWorld()
    {
        var code = @"
fun main(): void {
    print(""hello world"");
}
        ";

        var tokens = TheLexer.Lex(code);
        var parseTree = ModuleParser.Parse(tokens);
        var ast = ModuleToAst.Convert(parseTree);

        var outputDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TestProgramOutput");
        if (!Directory.Exists(outputDirectory))
            Directory.CreateDirectory(outputDirectory);

        ProgramEmitter.Emit(ast, outputDirectory);
    }
}
