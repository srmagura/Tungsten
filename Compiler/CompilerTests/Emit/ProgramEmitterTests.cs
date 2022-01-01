using TungstenCompiler.Emit;
using TungstenCompiler.Lexer;
using TungstenCompiler.Parser.Module;

namespace CompilerTests.Emit;

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
        if(!Directory.Exists(outputDirectory))
            Directory.CreateDirectory(outputDirectory);

        ProgramEmitter.Emit(ast, outputDirectory);
    }
}
